using System;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Resource
{
    [Header("Parameters")]
    [SerializeField]private int _capacity;
    [SerializeField] private int _count;

    [Header("Resource type")]
    public Flask.FlaskType ResourceType;

    [Header("Buttons")]
    public Button GetButton;
    public Button SetButton;

    [Header("FillBar(For storage-like)")]
    public Slider Bar;
    public TextMeshProUGUI BarText;

    public Resource()
    {
        Initialize();
        SetCapacity(10);
    }

    public int GetAmount() => _count;
    public void SetAmount(int amount) => _count = Mathf.Min(amount, _capacity);

    public int GetCapacity() => _capacity;

    public void SetCapacity(int capacity) => _capacity = capacity; 

    public void GetResource() //Игрок забрал мед
    {
        if (Player.Instance.GetFlask() != null || _count <= 0) return;
        Player.Instance.SetFlusk(ResourceType);
        _count--;
        Debug.Log($"Player take honey with type {ResourceType}");
    }

    public void SetResource() //Игрок отдал мед
    {
        Player player = Player.Instance;
        if (player.GetFlask() == null) return;
        if (player.GetFlask().GetFlaskType() == ResourceType && _count + 1 <= _capacity)
        {
            _count++;
            player.SetFlaskNull();
            Debug.Log($"Player give {ResourceType}");
        }
    }

    public void Initialize()
    {
        SetCapacity(10);
        if (GetButton) GetButton.onClick.AddListener(GetResource);
        if (SetButton) SetButton.onClick.AddListener(SetResource);
    }
}
