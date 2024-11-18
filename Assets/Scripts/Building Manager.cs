using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance;
    
    private Ray _ray;
    private RaycastHit _hit;
    private LayerMask _mask;
    [SerializeField] private GameObject _buildingPrefab; // Ноль префаба по оси y должен быть внизу
    private Building _building;
    [SerializeField] private Transform _buildingCarette;
    
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
        ChooseBuilding(0);
    }
    
    void Update()
    {
        _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(_ray, out _hit, Mathf.Infinity,  _mask)  && _building != null)
        {
            _buildingCarette.position = _hit.point;
            if (Input.GetMouseButtonDown(0) && _building.CanBuild)
            {
                Instantiate(_buildingPrefab, _buildingCarette.position,
                    _building.transform.rotation).GetComponent<Building>().SetMaterial(2);
            }
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            _building.transform.localEulerAngles += new Vector3(0, 90, 0);
        }
    }

    public void ChooseBuilding(int id)
    {
        _building = Instantiate(_buildingPrefab, _buildingCarette.position,
            Quaternion.identity, _buildingCarette).GetComponent<Building>();
        _building.SetMaterial(0);
    }
}
