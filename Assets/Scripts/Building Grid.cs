using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGrid : MonoBehaviour
{
    [SerializeField] private int _gridSize;
    
    public List<Tuple<float, float>> Coordinates = new List<Tuple<float, float>>();
    private float _w;
    private float _h;
    
    void Awake()
    {
        Vector3 p1 = gameObject.transform.TransformPoint(0, 0, 0);
        Vector3 p2 = gameObject.transform.TransformPoint(1, 1, 0);
        _w = p2.x - p1.x;
        _h = p2.y - p1.y;
        for (int i = 0; i < _gridSize; i++)
        {
            Coordinates.Add(new Tuple<float, float>(_w / _gridSize * i, _h / _gridSize * i));
        }
    }
    
    void Update()
    {
        
    }
}
