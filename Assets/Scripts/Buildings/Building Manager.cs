using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] private Building _buildingPrefab; // Ноль префаба по оси y должен быть внизу
    private Building _building;
    [SerializeField] private Transform _buildingCarette;
    
    [SerializeField] private Base _base;
    
    private RotationType[] _rotationTypes = new RotationType[]
        { RotationType.Top, RotationType.Right, RotationType.Bottom, RotationType.Left };
    private int _currentRotation = 0;
    
    private RotationType _rotationType = RotationType.Top;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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
        ChooseBuilding(0);
    }
    
    void Update()
    {
        _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(_ray, out _hit, Mathf.Infinity,  _mask)  && _building != null)
        {
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
                Instantiate(_buildingPrefab, _buildingCarette.position,
                    _building.transform.rotation).GetComponent<Building>().SetMaterial(2);
            }
        }
        else
        {
            _buildingCarette.gameObject.SetActive(false);
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            _building.RotateCollider();
            _rotationType = _rotationTypes[++_currentRotation % _rotationTypes.Length];
            _building.transform.localEulerAngles += new Vector3(0, 90, 0);
        }
    }

    public void ChooseBuilding(int id)
    {
        _building = Instantiate(_buildingPrefab, new Vector3(_buildingCarette.position.x - _buildingPrefab.Size.x, 
                _buildingCarette.position.y, _buildingCarette.position.z - _buildingPrefab.Size.y),
            Quaternion.identity, _buildingCarette).GetComponent<Building>();
        _building.SetMaterial(0);
    }

    public void QuitBuildingMode()
    {
        _base.Interact();
    }
}
