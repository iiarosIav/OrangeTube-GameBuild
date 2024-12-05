using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecyclingFactory : RecycleBuidling
{
    protected override void OnEnable()
    {
        base.OnEnable();
        Started();
    }
}
