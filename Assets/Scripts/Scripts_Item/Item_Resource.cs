using UnityEngine;

public enum ResourceType
{
    Wood,
    Stone,
    Iron,
    // add more later
}

public class Item_Resource : Item
{
    [Header("Resource Settings")]
    public ResourceType resourceType;
}
