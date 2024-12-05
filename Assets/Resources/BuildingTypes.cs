using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="BuildingTypes", menuName="Resources/BuildingTypes")]

[Serializable]
public class BuildingTypes : ScriptableObject
{
    public List<BuildType> BuildTypes;
}

[Serializable]
public class BuildType
{
    [SerializeField]private Building.BuildType type;
    [SerializeField]private GameObject Prefab;

    public Building.BuildType GetBuildingType() => type;
    public GameObject GetPrefab() => Prefab;
}
