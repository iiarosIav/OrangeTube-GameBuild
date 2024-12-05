using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class MovementQuest : IQuest
{
    public override event Action OnComplete;
    [SerializeField] private GameObject tipPanel;

    public override void RunQuest()
    {
        tipPanel.SetActive(true);
    }

    public override void UpdateQuest()
    {
        bool isPressed = Input.GetKeyDown(KeyCode.W) | Input.GetKeyDown(KeyCode.S)| Input.GetKeyDown(KeyCode.A)| Input.GetKeyDown(KeyCode.D);
        if (isPressed) { tipPanel.SetActive(false); OnComplete?.Invoke(); }
    }
}
