using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RotationType
{
    None,
    Top,
    Right,
    Bottom,
    Left
}

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance;
    
    private Ray _ray;
    private RaycastHit _hit;
    private LayerMask _mask;
    [SerializeField] private Building[] _buildingPrefabs; // Ноль префаба по оси y должен быть внизу
    private Building _building;
    [SerializeField] private Transform _buildingCarette;

    private int _prefabIndex;

    [SerializeField] private Base _base;
    
    private RotationType[] _rotationTypes = new RotationType[]
        { RotationType.Top, RotationType.Right, RotationType.Bottom, RotationType.Left };
    private int _currentRotation = 0;
    
    private RotationType _rotationType = RotationType.Top;

    private bool _isBlock, _deleteMod;

    [SerializeField] private TakeAndGiveQuest GiveQuest, TakeQuest, WaitQuest;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _mask = LayerMask.GetMask("Floor");
        _buildingCarette.gameObject.SetActive(false);
    }
    
    void Update()
    {
        if (!_building) return;
        _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool canPurchase = _building.CheckResources(MainStorage.Instance.ReturnResourcesCount());
        if (Physics.Raycast(_ray, out _hit, Mathf.Infinity,  _mask)  && _building != null && canPurchase)
        {
            
            if (_isBlock) return;
            Transform platform = _hit.collider.transform;
            float width = platform.localScale.x; // ширина
            float height = platform.localScale.z; // высота
            
            float xl = platform.position.x - width / 2; //"левый" x
            float xr = platform.position.x + width / 2; //"правый" x
            
            float yt = platform.position.z + height / 2; //"верхний" y
            float yb = platform.position.z - height / 2; //"нижний" y
            
            _buildingCarette.gameObject.SetActive(true);
            _buildingCarette.position = _hit.point;
            
            // Debug.Log(_building.Size.x + ", " +  _building.Size.y);
            if ( _rotationType == RotationType.Top && _hit.point.x - _building.Size.x > xl 
                 && _hit.point.z + _building.Size.y < yt && _building.GetObstacles() == 0)
            {
                _building.SetMaterial(0);
            }
            
            else if ( _rotationType == RotationType.Right && _hit.point.x + _building.Size.x < xr 
                && _hit.point.z + _building.Size.y < yt && _building.GetObstacles() == 0)
            {
                _building.SetMaterial(0);
            }
            
            else if ( _rotationType == RotationType.Bottom && _hit.point.x + _building.Size.x < xr 
                && _hit.point.z - _building.Size.y > yb && _building.GetObstacles() == 0)
            {
                _building.SetMaterial(0);
            }
            
            else if ( _rotationType == RotationType.Left && _hit.point.x - _building.Size.x > xl 
                && _hit.point.z - _building.Size.y > yb && _building.GetObstacles() == 0)
            {
                _building.SetMaterial(0);
            }
            else
            {
                _building.SetMaterial(1);
            }

            if (Input.GetMouseButtonDown(0) && _building.CanBuild)
            {
                _building.Buy(MainStorage.Instance.ReturnResourcesCount());
                var build = Instantiate(_buildingPrefabs[_prefabIndex], _buildingCarette.position,
                    _building.transform.rotation).GetComponent<Building>();
                build.SetMaterial(2);
                build.SetActions(GiveQuest.Click, TakeQuest.Click);
                build.Wait = WaitQuest;
            }
        }
        else
        {
            _buildingCarette.gameObject.SetActive(false);
        }

        if (Input.GetMouseButtonDown(1))
        {
            DeleteBuiding();
        }
        if (Input.mouseScrollDelta.y > 0)
        {
            _building.RotateCollider();
            _rotationType = _rotationTypes[++_currentRotation % _rotationTypes.Length];
            _building.transform.localEulerAngles += new Vector3(0, -90, 0);
        } else if(Input.mouseScrollDelta.y < 0)
        {
            _building.RotateCollider();
            _rotationType = _rotationTypes[++_currentRotation % _rotationTypes.Length];
            _building.transform.localEulerAngles += new Vector3(0, 90, 0);
        }
    }

    public bool GetCanBuilding() { if (_building) return _building.CanBuild; else return false; }

    public void ChooseBuilding(int id)
    {
        _prefabIndex = id;
        _building = Instantiate(_buildingPrefabs[_prefabIndex], new Vector3(_buildingCarette.position.x - _buildingPrefabs[_prefabIndex].Size.x, 
                _buildingCarette.position.y, _buildingCarette.position.z - _buildingPrefabs[_prefabIndex].Size.y),
            Quaternion.identity, _buildingCarette).GetComponent<Building>();
        _building.SetMaterial(0);
    }

    public void StateBlock(bool state)
    {
        _isBlock = state;
    }

    public void DeleteBuiding()
    {
        if (!_building) return;
        Destroy(_building.gameObject);
        _building = null;
    }

    public void DeleteMod()
    {
        _deleteMod = !_deleteMod;
    }

    public void QuitBuildingMode()
    {
        _base.Interact();
    }
}
