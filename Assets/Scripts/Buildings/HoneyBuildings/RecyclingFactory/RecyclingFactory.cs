using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecyclingFactory : HoneyBuilding
{
    [SerializeField]protected float _totalEnergyHoney;
    protected float _maxEnHoneyCapacity = 5;
    [SerializeField]protected float _honey;

    private float _recycleTime = 20;

    public Slider ReadyBar;
    public Slider CapacityBar;

    public TextMeshProUGUI CapacityText;

    float timer;

    private void Update()
    {
        Recycle();
        ChangeStats();
    }

    void ChangeStats()
    {
        if (!CapacityBar && !CapacityText) return;
        CapacityBar.maxValue = _maxEnHoneyCapacity;
        CapacityBar.value = _totalEnergyHoney;
        CapacityText.text = $"{_totalEnergyHoney}/{_maxEnHoneyCapacity}";
    }

    public override void SetValue() //Игрок отдал мед
    {
        Player player = Player.Instance;
        if (player.GetFlask() == null) return;
        if (_honey + _totalEnergyHoney >= _maxEnHoneyCapacity) return;
        if (player.GetFlask().GetFlaskType() == Flask.FlaskType.Honey)
        {
            _honey += 1;
            player.SetFlaskNull();
            Debug.Log("Player give honey");
        }
    }
    public override void TakeValue() //Игрок забрал мед
    {
        Player player = Player.Instance;
        if (player.GetFlask() != null) return;
        if (_totalEnergyHoney >= 1)
        {
            player.SetFlusk(Flask.FlaskType.EnergyHoney);
            _totalEnergyHoney -= 1;
            Debug.Log($"Player take flask with type {player.GetFlask().GetFlaskType()}");
        }
    }

    void Recycle()
    {
        if (_honey <= 0)
        {
            timer = Time.time;
            return;
        }
        if (Time.time - timer > _recycleTime)
        {
            timer = Time.time;
            _honey -= 1;
            _totalEnergyHoney += 1;
        }
        ReadyBar.value = 100 * (Time.time - timer) / _recycleTime;
    }
}
