using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(Resource))]
public class Storage : InteractableBuilding
{
    [SerializeField]protected Resource[] Resources;
    public int Capacity;
    private void OnEnable()
    {
        foreach (Resource r in Resources) { r.Initialize(); r.SetCapacity(Capacity); }
    }
    private void Update()
    {
        foreach (Resource r in Resources)
        {
            r.Initialize(); r.SetCapacity(Capacity);
            ChangeValues(r, r.Bar, r.BarText);
        }
    }
    void ChangeValues(Resource resource, Slider CapacityBar, TextMeshProUGUI CapacityText)
    {
        if (CapacityBar) { CapacityBar.maxValue = resource.GetCapacity(); CapacityBar.value = resource.GetAmount(); }
        if (CapacityText) CapacityText.text = $"Заполненность: {resource.GetAmount()}/{resource.GetCapacity()}";
    }

    public Resource[] ReturnResourcesCount()
    {
        return Resources;
    }

    public void SetResources(Product[] products)
    {
        for (int i = 0; i < Resources.Length; i++)
        {
            for (int j = 0; j < products.Length; j++)
            {
                if (Resources[i].ResourceType != products[j].ProductType) continue;
                Resources[i].SetAmount(Resources[i].GetAmount() + products[j].GetCost());
            }
        }
    }
}
