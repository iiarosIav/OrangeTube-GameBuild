using UnityEngine;
public class MainStorage : Storage
{
    public static MainStorage Instance;
    [SerializeField] private TakeAndGiveQuest GiveQuest;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
}
