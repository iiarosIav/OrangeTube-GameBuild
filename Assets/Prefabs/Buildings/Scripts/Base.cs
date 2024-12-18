using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : InteractiveObject
{
    [SerializeField] private Camera _buildingCamera;
    
    private bool _isActive;

    private void Start()
    {
        ChangeBuildingView();
    }
    
    public override void Interact()
    {
        ChangeBuildingView();
    }

    private void ChangeBuildingView()
    {
        _buildingCamera.gameObject.SetActive(_isActive);
        Player.Instance.SetIsStatic(_isActive);
        BuildingManager.Instance.gameObject.SetActive(_isActive);
        _isActive = !_isActive;

        if(_isActive)
        {
            Progress progress = Progress.Instance;

            string comment = $"Игрок {progress.GetUsername()} построил здания";
            progress.Save(comment);
        }
    }
}
