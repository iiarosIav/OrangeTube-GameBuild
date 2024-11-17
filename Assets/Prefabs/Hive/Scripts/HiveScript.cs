using System;
using System.Collections.Generic;
using UnityEngine;

public class HiveBehaviour : InteractiveObject
{
    public Bee BeeModel;
    public int BeesAmount;

    public float HoneyAmount;
    private float _maxHoney = 100;

    private List<Bee> _bees_Exists = new List<Bee>();

    private float _honeyAmount
    {
        get { return HoneyAmount; }
        set
        {
            Mathf.Min(_maxHoney, value);
        }
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
        if (_honeyAmount == _maxHoney) // если полон, то опустошаем
        {
            _honeyAmount = 0;
            Player.Instance.GetHoney(_maxHoney);
            Debug.Log("Devastated");
        }
        else // если нет, то выводим количество меда
        {
            Debug.Log($"Unfilled, honey level is {Convert.ToInt32(_honeyAmount)}");
        }
    }
}
