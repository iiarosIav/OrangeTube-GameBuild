using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSettings : MonoBehaviour
{
    bool _activeState;
    bool activeState
    {
        get => _activeState; set { _activeState = value; ChangeState(); }
    }
    public void ChangeState()
    {
        _activeState = !_activeState;
        transform.gameObject.SetActive(_activeState);
    }
}
