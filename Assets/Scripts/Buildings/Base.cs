using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : InteractiveObject
{
    [SerializeField] private Camera _buildingCamera;
    
    private bool _isActive;

    private void Start()
    {
        ChangeBuildingView(_isActive);
    }
    
    public override void Interact()
    {
        ChangeBuildingView(_isActive);
    }

    private void ChangeBuildingView(bool isActive)
    {
        _buildingCamera.gameObject.SetActive(isActive);
        Player.Instance.gameObject.SetActive(!isActive);
        BuildingManager.Instance.gameObject.SetActive(isActive);
        _isActive = !_isActive;
    }
}
