using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HiveBehaviour : InteractiveObject
{
    public Bee BeeModel;
    public int BeesAmount;

    private float _honeyCapacity;
    private float _maxCapacity = 100;
    private int _level;

    private List<Bee> _bees_Exists = new List<Bee>();
    public Slider Slider;
    [SerializeField] private TakeAndGiveQuest TakeQuest;

    public float HoneyCapacity
    {
        get { return _honeyCapacity; }
        set
        {
            _honeyCapacity = Mathf.Min(_maxCapacity, value);
            UpdateSlider();
        }
    }

    public int Level
    {
        get { return _level; }
        set
        {
            if (value > 1) _level = value;
            else _level = 1;
        }
    }

    void UpdateSlider()
    {
        if (!Slider) return;
        Slider.maxValue = _maxCapacity;
        Slider.value = _honeyCapacity;
    }

    void OnLevelChanged(int level)
    {

    }

    private void SpawnBee()
    {
        Bee bee = Instantiate(BeeModel, transform);

        bee.transform.position = transform.position;
        bee.transform.rotation = Quaternion.identity;

        bee.ParentHive = this;

        _bees_Exists.Add(bee);
    }

    private void Update()
    {
        if (_bees_Exists.Count < BeesAmount)
        {
            SpawnBee();
        }
    }
    
    public override void Interact() // взаимодействие с ульем
    {
        if (HoneyCapacity >= Flask.FlaskCapacity) // если полон, то опустошаем
        {
            Player player = Player.Instance;
            if (player.GetFlask() != null) return;
            Player.Instance.SetFlusk(Flask.FlaskType.Honey);
            HoneyCapacity -= Flask.FlaskCapacity;
            Debug.Log("Devastated");
            TakeQuest.Click();
        }
        else // если нет, то выводим количество меда
        {
            Debug.Log($"Unfilled, honey level is {Convert.ToInt32(HoneyCapacity)}");
        }
    }
}
