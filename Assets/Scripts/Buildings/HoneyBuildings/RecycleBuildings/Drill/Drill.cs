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

    private void OnEnable()
    {
        Started();
        _mask = LayerMask.GetMask(OreName);
    }

    protected override void Update()
    {

        if (!_isStatic) switch (CheckOre())
            {
                case true:
                    WarningText.text = ValidText;
                    WarningText.color = Color.green;
                    break;
                case false:
                    WarningText.text = InvalidText;
                    WarningText.color = Color.red;
                    break;
            }
        else Destroy(WarningText);

        base.Update();
    }

    bool CheckOre()
    {
        RaycastHit _hit;
        return 
            Physics.Raycast(transform.position, Vector3.down, out _hit, _mask);
    }
}
