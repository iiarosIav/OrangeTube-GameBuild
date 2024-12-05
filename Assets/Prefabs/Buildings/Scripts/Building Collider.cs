using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCollider : MonoBehaviour
{
    [SerializeField] private Building _building;
    public int Obstacles;

    private void OnTriggerEnter(Collider other)
    {
        if (_building.GetState())
        {
            GetComponent<Collider>().isTrigger = false;
            Destroy(this);
        }
        if (other.gameObject.layer != 3)
        {
            Debug.Log(other.gameObject.name);
            Obstacles++;
            _building.SetMaterial(1);
        }

        if (Obstacles <= 0)
        {
            _building.SetMaterial(0);
            Obstacles = 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != 3)
        {
            Obstacles--;
        }

        if (Obstacles <= 0)
        {
            _building.SetMaterial(0);
            Obstacles = 0;
        }
    }
}
