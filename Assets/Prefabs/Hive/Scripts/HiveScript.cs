using System.Collections.Generic;
using UnityEngine;

public class HiveBehaviour : MonoBehaviour
{
    public Bee _BeeModel;
    public int BeesAmount;

    public float _HoneyAmount;
    private float _MaxHoney = 100;

    private List<Bee> _Bees_Exists = new List<Bee>();

    private float HoneyAmount
    {
        get { return _HoneyAmount; }
        set
        {
            Mathf.Min(_MaxHoney, value);
        }
    }

    private void SpawnBee()
    {
        Bee bee = Instantiate(_BeeModel, transform);

        bee.transform.position = transform.position;
        bee.transform.rotation = Quaternion.identity;

        bee.ParentHive = this;

        _Bees_Exists.Add(bee);
    }

    private void Update()
    {
        if (_Bees_Exists.Count < BeesAmount)
        {
            SpawnBee();
        }
    }
}
