using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class PlayerSave
{
    public string name;
    public ResourcesClass resources;
}

[Serializable]
public class ResourcesClass
{
    public PlayerState playerState = new PlayerState();
    public List<BuildingState> buildingStates = new List<BuildingState>() { new BuildingState() };
    public List<HiveState> hiveStates = new List<HiveState>() { new HiveState() };
}

[Serializable]
public class PlayerState
{
    public Vector3 position = Vector3.zero;
    public Flask.FlaskType resourceType = Flask.FlaskType.None;
    public bool completeTutorial = false;
    public int tutorialIndex = 0;
}

[Serializable]
public class BuildingState
{
    public Building.BuildType buildingType;
    public Vector3 position = Vector3.zero;
    public Quaternion rotation = Quaternion.identity;
    public bool hasResources = false;
    public Resource[] resources = Array.Empty<Resource>();
}

[Serializable]
public class HiveState
{
    public Building.BuildType buildingType = Building.BuildType.Hive;
    public Vector3 position = Vector3.zero;
    public Quaternion rotation = Quaternion.identity;
    public float honeyCapacity = 0f;
    public TakeAndGiveQuest takeAndGiveQuest = null;
}

[Serializable]
public class LogClass
{
    public string comment;
    public string player_name;
    public ResourcesClass resources_changed;
}

[Serializable]
public class ResourcesSave
{
    public ResourcesClass resources;
}

public class Progress : MonoBehaviour
{
    public static Progress Instance;

    private static HttpClient client = new HttpClient();

    private static string game_uuid = "c26f88d5-41f6-4443-81ce-cf55303a8f44";

    [SerializeField] private string _username; // надо запрос на имя пользователя

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        _username = PlayerData.GetNickname();
    }

    private void Start()
    {
        if (PlayerData.IsContinue()) Load();
        else if (CheckInJson(_username)) Delete();
        else FindObjectOfType<TutorialManager>().Starter();
    }

    [ContextMenu("Save")]
    public void a()
    {
        Save();
    }
    
    public void Save(string comment = null)
    {
        string username = _username;

        bool injson = CheckInJson(username);

        PlayerSave playerSave = new PlayerSave();
        playerSave.name = username;

        playerSave.resources = GetResources();

        HttpWebRequest httpWebRequest;
        string json;

        if (injson)
        {
            ResourcesSave resourcesSave = new ResourcesSave();
            resourcesSave.resources = playerSave.resources;
            json = JsonUtility.ToJson(resourcesSave);
            httpWebRequest =
                (HttpWebRequest)WebRequest.Create($"https://2025.nti-gamedev.ru/api/games/{game_uuid}/players/{username}/");
            httpWebRequest.Method = "PUT";
        }
        
        else
        {
            json = JsonUtility.ToJson(playerSave);
            
            httpWebRequest =
                (HttpWebRequest)WebRequest.Create($"https://2025.nti-gamedev.ru/api/games/{game_uuid}/players/");
            httpWebRequest.Method = "POST";
        }

        httpWebRequest.ContentType = "application/json";

        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        {
            streamWriter.Write(json);
        }
        
        if (comment != null) SendLog(comment, playerSave.resources);
    }

    private void SendLog(string comment, ResourcesClass resources)
    {
        LogClass log = new LogClass();
        log.comment = $"{DateTime.Now} - {comment}";
        log.player_name = _username;
        log.resources_changed = resources;
        string json = JsonUtility.ToJson(log);
        
        HttpWebRequest httpWebRequest =
            (HttpWebRequest)WebRequest.Create($"https://2025.nti-gamedev.ru/api/games/{game_uuid}/logs/");
        httpWebRequest.Method = "POST";

        httpWebRequest.ContentType = "application/json";

        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        {
            streamWriter.Write(json);
        }
    }

    private ResourcesClass GetResources()
    {
        ResourcesClass resources = new ResourcesClass();
        resources.playerState = GetPlayerState();
        resources.buildingStates = GetBuildingStates();
        resources.hiveStates = GetHiveStates();
        return resources;
    }

    private PlayerState GetPlayerState()
    {
        PlayerState playerState = new PlayerState();

        var flask = Player.Instance.GetFlask();

        if (flask != null)
        {
            playerState.resourceType = flask.GetFlaskType();
        }

        TutorialManager tutorialManager = FindObjectOfType<TutorialManager>();

        playerState.completeTutorial = tutorialManager.GetIsComplete();
        
        playerState.tutorialIndex = tutorialManager.GetTutorialIndex();

        playerState.position = Player.Instance.transform.position;

        return playerState;
    }

    private List<BuildingState> GetBuildingStates()
    {
        List<BuildingState> buildingStates = new List<BuildingState>();
        Building[] buildings = FindObjectsOfType<Building>();
        foreach (Building building in buildings)
        {
            BuildingState buildingState = new BuildingState();
            buildingState.position = building.transform.position;
            buildingState.rotation = building.transform.rotation;
            buildingState.buildingType = building.GetBuildType();
            if (building.gameObject.GetComponent<Storage>())
            {
                buildingState.hasResources = true;
                buildingState.resources = building.gameObject.GetComponent<Storage>().ReturnResourcesCount();
            }
            else
            {
                buildingState.resources = Array.Empty<Resource>();
            }

            buildingStates.Add(buildingState);
        }

        return buildingStates;
    }

    private List<HiveState> GetHiveStates()
    {
        List<HiveState> hiveStates = new List<HiveState>();
        HiveBehaviour[] hives = FindObjectsOfType<HiveBehaviour>();
        foreach (HiveBehaviour hive in hives)
        {
            HiveState hiveState = new HiveState();
            hiveState.position = hive.transform.position;
            hiveState.rotation = hive.transform.rotation;
            hiveState.honeyCapacity = hive.HoneyCapacity;
            hiveState.takeAndGiveQuest = hive.GetQuest();
            hiveStates.Add(hiveState);
        }

        return hiveStates;
    }

    private bool CheckInJson(string username)
    {
        Task<string> response = client.GetStringAsync(
            $"https://2025.nti-gamedev.ru/api/games/{game_uuid}/players/");

        string json = response.Result;

        PlayerSave[] saveList = ZVJson.FromJson<PlayerSave>(json, true);

        foreach (var player in saveList)
        {
            if (player.name == username)
            {
                return true;
            }
        }

        return false;
    }

    public static bool isExistPlayer(string usname)
    {
        int a = 0;
        Task<string> response = client.GetStringAsync(
            $"https://2025.nti-gamedev.ru/api/games/{game_uuid}/players/");

        string json = response.Result;

        PlayerSave[] saveList = ZVJson.FromJson<PlayerSave>(json, true);

        foreach (var player in saveList)
        {
            if (player.name == usname)
            {
                a++;
            }
        }
        if (a > 0) return true;
        else return false;
    }

    // public void SaveAndLeaveMenu()
    // {
    //     Save();
    //     SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    //     SceneManager.LoadScene("Menu");
    // }

    public void SaveAndQuit()
    {
        string comment = $"Игрок {_username} вышел";
        Save(comment);
        Application.Quit();
    }

    [ContextMenu("Load")]
    public void Load()
    {
        Task<string> response = client.GetStringAsync(
            $"https://2025.nti-gamedev.ru/api/games/{game_uuid}/players/");

        string json = response.Result;

        PlayerSave[] saveList = ZVJson.FromJson<PlayerSave>(json, true);

        foreach (var player in saveList)
        {
            if (player.name == _username)
            {
                Building[] buildings = FindObjectsOfType<Building>();
                HiveBehaviour[] hiveBehaviours = FindObjectsOfType<HiveBehaviour>();
        
                foreach (Building building in buildings)
                {
                    Destroy(building.gameObject);
                }

                foreach (HiveBehaviour hiveBehaviour in hiveBehaviours)
                {
                    Destroy(hiveBehaviour.gameObject);
                }

                PlayerState playerState = player.resources.playerState;
                
                TutorialManager tutorialManager = FindObjectOfType<TutorialManager>();

                if (playerState.completeTutorial) tutorialManager.Complete();
                
                tutorialManager.FinishFirstQuest();
                
                tutorialManager.Run(playerState.tutorialIndex);

                Player.Instance.SetPosition(playerState.position);
                if (playerState.resourceType != Flask.FlaskType.None)
                {
                    Player.Instance.SetFlusk(playerState.resourceType);
                }
                else
                {
                    Player.Instance.SetFlaskNull();
                }

                foreach (BuildingState buildingstate in player.resources.buildingStates)
                {
                    Building building = Instantiate(PickBuilding(buildingstate.buildingType), buildingstate.position,
                        buildingstate.rotation).GetComponent<Building>();
                    Debug.Log(buildingstate.hasResources + " ---- " + building.GetBuildType());
                    building.Initialize();
                    if (buildingstate.hasResources == false) continue;
                    building.GetComponent<Storage>().SetResources(buildingstate.resources);
                }

                foreach (HiveState hiveState in player.resources.hiveStates)
                {
                    HiveBehaviour hive = Instantiate(PickBuilding(hiveState.buildingType), 
                        hiveState.position, hiveState.rotation).GetComponent<HiveBehaviour>();
                    hive.HoneyCapacity = hiveState.honeyCapacity;
                    hive.SetQuest(hiveState.takeAndGiveQuest);
                }
                
                return;
            }
        }
    }

    private GameObject PickBuilding(Building.BuildType type)
    {
        var bTypes = Resources.Load<BuildingTypes>("BuildingTypes");
        if (!bTypes) return null;
        GameObject gameObject = null;
        foreach (var bType in bTypes.BuildTypes)
        {
            if (bType.GetBuildingType() == type) { gameObject = bType.GetPrefab(); }
        }

        return gameObject;
    }
    
    [ContextMenu("GetJson")]
    private void GetJson()
    {
        Task<string> response = client.GetStringAsync(
            $"https://2025.nti-gamedev.ru/api/games/{game_uuid}/players/");

        string json = response.Result;
        Debug.Log(json);
    }

    [ContextMenu("Delete")]
    public void Delete()
    {
        Task<HttpResponseMessage> response = client.DeleteAsync(
            $"https://2025.nti-gamedev.ru/api/games/{game_uuid}/players/{_username}/");
    }
    
    public string GetUsername() => _username;
}