using UnityEngine;

public class Flask
{
    public static float FlaskCapacity = 50;
    public enum FlaskType
    {
        None,
        Honey,
        EnergyHoney,
        Metal
    }
    FlaskType _type;

    public Flask(FlaskType type)
    {
        _type = type;
    }
    public FlaskType GetFlaskType() => _type;

    public float GetFlaskCapacity() => FlaskCapacity;
}
