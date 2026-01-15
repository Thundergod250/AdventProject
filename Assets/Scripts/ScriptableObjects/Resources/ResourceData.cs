using UnityEngine;

[CreateAssetMenu(fileName = "NewResourceData", menuName = "Resource/Resource Data")]
public class ResourceData : ScriptableObject
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
    public GarbageGroup Group;
    public GarbageSubtype Subtype;
    public string ObjectName;
    [TextArea] public string ObjectDescription;
    public int ObjectWeight;
}
