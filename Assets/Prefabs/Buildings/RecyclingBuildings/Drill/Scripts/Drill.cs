using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Drill : RecycleBuidling
{
    public string OreName;
    public string ValidText = "Здесь есть руда";
    public string InvalidText = "Здесь нет руды";

    private LayerMask _mask;

    public TextMeshProUGUI WarningText;

    protected override void OnEnable()
    {
        base.OnEnable();
        Started();
        _mask = LayerMask.GetMask(OreName);
    }

    protected override void Update()
    {
        base.Update();

        if (!_isStatic) switch (CheckOre())
            {
                case true:
                    WarningText.text = ValidText;
                    WarningText.color = Color.green;
                    SetMaterial(0);
                    break;
                case false:
                    WarningText.text = InvalidText;
                    WarningText.color = Color.red;
                    SetMaterial(1);
                    break;
            }
        else Destroy(WarningText);
    }

    bool CheckOre()
    {

        RaycastHit _hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return 
            Physics.Raycast(ray, out _hit, Mathf.Infinity, _mask);
    }
}
