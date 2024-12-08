using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCodeQuest : IQuest
{
    public override event Action OnComplete;
    [SerializeField] private GameObject tipPanel;
    public KeyCode Key;

    public override void RunQuest()
    {
        tipPanel.SetActive(true);
    }

    public override void UpdateQuest()
    {
        if (Input.GetKeyDown(Key))
        {
            tipPanel.SetActive(false);
            OnComplete?.Invoke();
        }
    }
}
