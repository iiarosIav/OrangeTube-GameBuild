using UnityEngine;

public class Flask
{
    public static float FlaskCapacity = 50;
    public enum FlaskType
    {
        Honey,
        EnergyHoney
    }
    FlaskType _type;

    public Flask(FlaskType type)
    {
        _type = type;
    }
    public FlaskType GetFlaskType() => _type;

    public float GetFlaskCapacity() => FlaskCapacity;
}
