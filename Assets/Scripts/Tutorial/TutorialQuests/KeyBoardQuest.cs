using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoardQuest : IQuest
{
    public override event Action OnComplete;
    [SerializeField] private GameObject tipPanel;

    public override void RunQuest()
    {
        tipPanel.SetActive(true);
    }

    public override void UpdateQuest()
    {
        if (BuildingManager.Instance.GetCanBuilding() && Input.GetKeyDown(KeyCode.Mouse0))
        {
            tipPanel.SetActive(false);
            OnComplete?.Invoke();
        }
    }
}
