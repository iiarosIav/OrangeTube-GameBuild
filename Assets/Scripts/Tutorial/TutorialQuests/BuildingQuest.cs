using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingQuest : IQuest
{
    public override event Action OnComplete;
    [SerializeField] private GameObject tipPanel;

    private bool isClicked;

    public override void RunQuest()
    {
        tipPanel.SetActive(true);
    }

    public override void UpdateQuest()
    {
        if (isClicked)
        {
            tipPanel.SetActive(false);
            OnComplete?.Invoke();
        }
    }

    public void Click() => isClicked = true;
}
