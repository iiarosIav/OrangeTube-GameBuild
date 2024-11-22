using UnityEngine.UI;
using UnityEngine;

public abstract class HoneyBuilding : Building
{
    protected float _honeyCapacity = 1000;
    protected float _totalHoney;

    protected bool _menu;

    public bool hasMenu
    {
        get { return _menu; }
        set
        {
            _menu = value;
            Menu.SetActive(value);
        }
    }

    public GameObject Menu;

    public abstract void TakeValue();
    public abstract void SetValue();

    private void Awake()
    {
        hasMenu = false;
    }

    public override void Interact()
    {
        hasMenu = !hasMenu;
    }
}
