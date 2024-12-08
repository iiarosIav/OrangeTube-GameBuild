using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;

public class RecycleBuidling : InteractableBuilding
{
    public Resource GivenResource;
    public Resource RecievedResource;

    public int Capacity;

    [SerializeField]private float _recycleTime = 20;
    protected float timer;

    protected bool isDrill;

    public Slider ReadyBar;
    public Slider CapacityBar;
    public TextMeshProUGUI CapacityText;

    protected virtual void OnEnable()
    {
        GivenResource.SetCapacity(Capacity);
        RecievedResource.SetCapacity(Capacity);
        GivenResource.Initialize();
        RecievedResource.Initialize();
    }

    protected virtual void Update()
    {
        Recycle();
        ChangeValues();
        GivenResource.SetCapacity(Capacity);
        RecievedResource.SetCapacity(Capacity);
        GivenResource.Initialize();
        RecievedResource.Initialize();
    }

    protected void ChangeValues()
    {
        if(CapacityBar) { CapacityBar.maxValue = RecievedResource.GetCapacity(); CapacityBar.value = RecievedResource.GetAmount(); }
        if (CapacityText) CapacityText.text = $"Всего: {RecievedResource.GetAmount()}/{RecievedResource.GetCapacity()}";
    }

    public void SetValue() //����� ����� ���
    {
        Player player = Player.Instance;
        if (player.GetFlask() == null) return;
        if (GivenResource.GetAmount() + RecievedResource.GetAmount() >= RecievedResource.GetCapacity()) return;
        if (player.GetFlask().GetFlaskType() == GivenResource.ResourceType)
        {
            GivenResource.SetAmount(GivenResource.GetAmount() + 1);
            player.SetFlaskNull();
            Debug.Log($"Player give honey, given capacity: {GivenResource.GetAmount()}, recieved capacity: {RecievedResource.GetCapacity()}");
        }
    }
    public void TakeValue() //����� ������ ���
    {
        Player player = Player.Instance;
        if (player.GetFlask() != null) return;
        if (RecievedResource.GetAmount() >= 1)
        {
            player.SetFlusk(RecievedResource.ResourceType);
            RecievedResource.SetAmount(RecievedResource.GetAmount() - 1);
            Debug.Log($"Player take flask with type {player.GetFlask().GetFlaskType()}");
        }
    }

    protected void Recycle()
    {
        if (GivenResource.GetAmount() <= 0)
        {
            timer = Time.time;
            isDrill = true;
            return;
        }
        if (Time.time - timer > _recycleTime)
        {
            timer = Time.time;
            GivenResource.SetAmount(GivenResource.GetAmount() - 1);
            RecievedResource.SetAmount(RecievedResource.GetAmount() + 1);
            Wait.Click();
            isDrill = false;
        }
        if (!ReadyBar) return;
        ReadyBar.value = 100 * (Time.time - timer) / _recycleTime;
    }

    public override void Initialize()
    {
        base.Initialize();
        
        GivenResource.SetCapacity(Capacity);
        RecievedResource.SetCapacity(Capacity);
        GivenResource.Initialize();
        RecievedResource.Initialize();
    }
}
