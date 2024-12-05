using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class Product
{
    [SerializeField] private int _count;
    public Flask.FlaskType ProductType;
    private string _name;
    public string GetName() => _name;
    public string SetName(string name) => _name = name;
    public int GetCost() => _count;
}

public class Building : InteractiveObject
{
    public enum BuildType
    {
        None,
        RecyclingFactory,
        Drill,
        HoneyStorage,
        MetalStorage,
        MainStorage
    }
    [SerializeField] private Renderer _model;
    [SerializeField] private BuildingCollider _collider;
    public Material _material;

    public (float x, float y) Size;
    
    public bool CanBuild;

    [SerializeField]protected Product[] _price;

    public bool _isStatic;

    [Header("Materials")]
    [SerializeField] private Material _valid;
    [SerializeField] private Material _invalid;
    [SerializeField] private Material _static;

    [Header("Buttons")]
    [SerializeField] private Button _deleteButton;

    [SerializeField] private Button Give, Take;
    [HideInInspector] public TakeAndGiveQuest Wait;

    public void SetActions(UnityAction a, UnityAction b)
    {
        Give.onClick.AddListener(a);
        Take.onClick.AddListener(b);
    }

    public void SetDeleteEvent(UnityAction action) => _deleteButton.onClick.AddListener(action);

    protected void DestroyThis()
    {
        Destroy(gameObject);
        MainStorage.Instance.ReturnResources(GetProduct());
    }

    private void Awake()
    {
        if (!_deleteButton) return;
        SetDeleteEvent(DestroyThis);
    }

    public bool CheckResources(Resource[] _resources)
    {
        int c = 0;
        for (int i = 0; i < _price.Length; i++)
        {
            for (int j = 0; j < _resources.Length; j++)
            {
                if (_price[i].ProductType != _resources[j].ResourceType) continue;
                if (_price[i].GetCost() <= _resources[j].GetAmount()) c++;
            }
        }
        if (c == _price.Length) { return true; }
        else return false;
    }

    public void Buy(Resource[] _resources)
    {
        for (int i = 0; i < _price.Length; i++)
        {
            for (int j = 0; j < _resources.Length; j++)
            {
                if (_price[i].ProductType != _resources[j].ResourceType) continue;
                if (_price[i].GetCost() <= _resources[j].GetAmount())
                    _resources[j].SetAmount(_resources[j].GetAmount() - _price[i].GetCost());
            }
        }
    }

    protected void Started()
    {
        _material = _model.material;
        _material.color = _static.color;
        Vector3 size = _collider.gameObject.GetComponent<Collider>().bounds.size;
        Debug.Log(size);
        float x = size.x;
        float y = size.z;
        Size = (x, y);
    }

    public bool GetState() { return _isStatic; }
    public Product[] GetProduct() { return _price; }

    public void SetMaterial(int materialIndex)
    {
        if (_isStatic) return;
        if (!_material) return;
        
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
