using UnityEngine;

public class GarbageObject : MonoBehaviour
{
    public enum GarbageGroup
    {
        Plastic,
        Organic,
        Metal,
        Glass,
        Electronic
    }

    public enum GarbageSubtype
    {
        // Plastic
        Bottle,

        // Organic
        Wood,
        PizzaBox,

        // Metal
        Can,
        Pipe,

        // Electronic
        CircuitBoard,
        Battery
    }

    [Header("Garbage Classification")]
    public GarbageGroup group;
    public GarbageSubtype subtype;
}