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
using UnityEngine.UI;

[Serializable]
public class PlayerSave
{
    public string name;
    public Resources resources;
}

[Serializable]
// public class Resources
// {
//     public int honey = 10;
// }

public class Resources
{
    public PlayerState playerState = new PlayerState();
    public List<BuildingState> buildingStates = new List<BuildingState>() { new BuildingState() };
}

[Serializable]
public class PlayerState
{
    public Vector3 position = Vector3.zero;
    public Flask.FlaskType resourceType = Flask.FlaskType.None;
}

[Serializable]
public class BuildingState
{
    public string buildingType = "Building";
    // public Building.Type buildingType;
    public Vector3 position = Vector3.zero;
    public Quaternion rotation = Quaternion.identity;
    public bool hasResources = false;
    public Resource[] resources = Array.Empty<Resource>();
    
}

public class Progress : MonoBehaviour
{
    private static HttpClient client = new HttpClient();
    
    private string game_uuid = "c26f88d5-41f6-4443-81ce-cf55303a8f44";
    
    private string _username = "user_with_apples1"; // надо запрос на имя пользователя
    
    [ContextMenu("Save")]
    public void Save()
    {
        string username = _username;
        
        bool injson = CheckInJson(username);
        
        PlayerSave playerSave = new PlayerSave();
        playerSave.name = username;
        
        // playerSave.resources = GetResources();
        
        playerSave.resources = new Resources();

        HttpWebRequest httpWebRequest;
        
        if (injson)
        {
            Delete(username);
        }
        
        string json = JsonUtility.ToJson(playerSave);
        httpWebRequest = (HttpWebRequest)WebRequest.Create($"https://2025.nti-gamedev.ru/api/games/{game_uuid}/players/");
        httpWebRequest.Method = "POST";
        
        httpWebRequest.ContentType = "application/json";
        
        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        {
            streamWriter.Write(json);
        }
        
        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
            Debug.Log(result);
        }
    }

    private Resources GetResources()
    {
        Resources resources = new Resources();
        resources.playerState = GetPlayerState();
        resources.buildingStates = GetBuildingStates();
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
            // buildingState.buildingType = building.Type; // Нужна переменная у класса Builduing - Building.Type
            buildingState.position = building.transform.position;
            buildingState.rotation = building.transform.rotation;
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

    [ContextMenu("GetJson")]
    private void GetJson()
    {
        Task<string> response = client.GetStringAsync(
            $"https://2025.nti-gamedev.ru/api/games/{game_uuid}/players/");
        
        string json =  response.Result;
        Debug.Log(json);
    }

    private bool CheckInJson(string username)
    {
        Task<string> response = client.GetStringAsync(
            $"https://2025.nti-gamedev.ru/api/games/{game_uuid}/players/");
        
        string json =  response.Result;
        
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

    [ContextMenu("Load")]
    public void Load()
    {
        Task<string> response = client.GetStringAsync(
            $"https://2025.nti-gamedev.ru/api/games/{game_uuid}/players/");
        
        string json =  response.Result;
        
        PlayerSave[] saveList = ZVJson.FromJson<PlayerSave>(json, true);
        
        foreach (var player in saveList)
        {
            if (player.name == _username)
            {
                Building[] buildings = FindObjectsOfType<Building>();
                foreach (Building building in buildings)
                {
                    Destroy(building.gameObject);
                }
                
                PlayerState playerState = player.resources.playerState;
                
                Player.Instance.transform.position = playerState.position;
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
                    Building building = Instantiate(buildingPrefab, buildingstate.position,
                        buildingstate.rotation); // Нужны buildingPrefab'ы по типам
                    if (buildingstate.hasResources == false) continue;
                    building.GetComponent<Storage>().SetResources(buildingstate.resources);
                }
            }
        }
    }

    public void Delete(string username)
    {
        Task<HttpResponseMessage> response = client.DeleteAsync(
            $"https://2025.nti-gamedev.ru/api/games/{game_uuid}/players/{username}");
    }
}
