using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickQuest : IQuest
{
    public override event Action OnComplete;
    [SerializeField] private GameObject tipPanel;

    public override void RunQuest()
    {
        tipPanel.SetActive(true);
    }

    public override void UpdateQuest()
    {
        if (Input.anyKeyDown)
        {
            tipPanel.SetActive(false);
            OnComplete?.Invoke();
        }
    }
}
