using Unity;
using UnityEngine;
using UnityEngine.UI;

public class HoneyStorage : HoneyBuilding
{
    protected float _totalEnergyHoney;
    protected float _maxCapacity = 5;

    public Slider CapacityBar;

    private void Update()
    {
        ChangeStats();
    }

    void ChangeStats()
    {
        CapacityBar.maxValue = _maxCapacity;
        CapacityBar.value = _totalEnergyHoney;
    }

    public override void TakeValue() //Игрок забрал мед
    {
        Player player = Player.Instance;
        if (player.GetFlask() != null) return;
        if(_totalEnergyHoney >= 1)
        {
            player.SetFlusk(Flask.FlaskType.EnergyHoney);
            _totalEnergyHoney -= 1;
            Debug.Log($"Player take flask with type {player.GetFlask().GetFlaskType()}");
        }
    }
    public override void SetValue() //Игрок отдал мед
    {
        Player player = Player.Instance;
        if (player.GetFlask() == null) return;
        if(player.GetFlask().GetFlaskType() == Flask.FlaskType.EnergyHoney)
        {
            _totalEnergyHoney += 1;
            player.SetFlaskNull();
            Debug.Log("Player give honey");
        }
    }
}
