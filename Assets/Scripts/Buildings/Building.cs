using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Building : InteractiveObject
{
    [SerializeField] private Renderer _model;
    [SerializeField] private BuildingCollider _collider;
    private Material _material;

    public (float x, float y) Size;
    
    public bool CanBuild;

    private bool _isStatic;
    
    [SerializeField] private Material _valid;
    [SerializeField] private Material _invalid;
    [SerializeField] private Material _static;

    private void Awake()
    {
        _material = _model.material;
        _material.color = _static.color;
        Vector3 size = _collider.gameObject.GetComponent<Collider>().bounds.size;
        Debug.Log(size);
        float x = size.x;
        float y = size.z;
        Size = (x, y);
    }

    public void SetMaterial(int materialIndex)
    {
        if (_isStatic) return;
        
        if (materialIndex == 0)
        {
            _material.color = _valid.color;
            CanBuild = true;
        }
        else if (materialIndex == 1)
        {
            _material.color = _invalid.color;
            CanBuild = false;
        }
        else
        {
            _material.color = _static.color;
            _isStatic = true;
        }
    }

    public void RotateCollider()
    {
        Size = (Size.y, Size.x);
    }

    public int GetObstacles() => _collider.Obstacles;

    // private void OnCollisionEnter(Collision other)
    // {
    //     if (other.gameObject.layer != LayerMask.GetMask("Floor"))
    //     {
    //         _material.color = _invalid.color;
    //     }
    // }
    //
    // private void OnCollisionExit(Collision other)
    // {
    //     if (other.gameObject.layer != LayerMask.GetMask("Floor"))
    //     { 
    //         _material.color = _valid.color;
    //     }
    // }
}
