using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    bool isActive;
    [SerializeField] GameObject _menu;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) ChangeState();
        _menu.SetActive(isActive);
        if (isActive) { Time.timeScale = 0f; Player.Instance.FreezeCamera(); }
        else { Time.timeScale = 1f; Player.Instance.UnFreezeCamera(); }
    }

    public void ChangeState()
    {
        isActive = !isActive;
    }
}
