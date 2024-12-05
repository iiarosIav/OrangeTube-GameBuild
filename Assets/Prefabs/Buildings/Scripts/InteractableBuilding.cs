using UnityEngine.UI;
using UnityEngine;

public class InteractableBuilding : Building
{
    protected bool _menu;

    public bool hasMenu
    {
        get { return _menu; }
        set
        {
            _menu = value;
            Menu.SetActive(_menu);
        }
    }

    [Header("UI Menu")]
    public GameObject Menu;

    public override void Interact()
    {
        hasMenu = !hasMenu;
    }
}
