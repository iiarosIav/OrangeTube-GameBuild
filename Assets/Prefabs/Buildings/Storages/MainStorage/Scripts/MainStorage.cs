using UnityEngine;
public class MainStorage : Storage
{
    public static MainStorage Instance;
    [SerializeField] private TakeAndGiveQuest GiveQuest;
    [SerializeField] private GameObject Win;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        Instance = this;
    }

    protected void Update()
    {
        int a = 0;
        foreach(var item in ReturnResourcesCount())
        {
            if (item.ResourceType == Flask.FlaskType.EnergyHoney && item.GetAmount() >= 500) a++;
            else if (item.ResourceType == Flask.FlaskType.Metal && item.GetAmount() >= 200) a++;
        }
        if(a >= 2) Win.SetActive(true);
    }
}
