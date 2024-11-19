using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCollider : MonoBehaviour
{
    [SerializeField] private Building _building;
    private int _obstacles;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 3)
        {
            Debug.Log(other.gameObject.name);
            _obstacles++;
            _building.SetMaterial(1);
        }
        
        if (_obstacles <= 0)
        {
            _building.SetMaterial(0);
            _obstacles = 0;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != 3)
        {
            _obstacles--;
        }

        if (_obstacles <= 0)
        {
            _building.SetMaterial(0);
            _obstacles = 0;
        }
    }
}
