using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TakeAndGiveQuest : IQuest
{
    public override event Action OnComplete;
    [SerializeField] private GameObject tipPanel;
    bool isClicked = false;

    public override void RunQuest()
    {
        tipPanel.SetActive(true);
    }

    public void Click() => isClicked = true;

    public override void UpdateQuest()
    {
        if (isClicked)
        {
            tipPanel.SetActive(false);
            OnComplete?.Invoke();
        }
    }
}
