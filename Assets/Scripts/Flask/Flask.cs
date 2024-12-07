using UnityEngine;

public class Flask
{
    public static float FlaskCapacity = 50;
    public enum FlaskType
    {
        Honey,
        EnergyHoney,
        Metal,
        None
    }
    FlaskType _type;

    public Flask(FlaskType type)
    {
        _type = type;
    }
    public FlaskType GetFlaskType() => _type;

    public float GetFlaskCapacity() => FlaskCapacity;
}
