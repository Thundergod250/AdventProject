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

    public override bool IsSameType(Item other)
    {
        if (other is Item_Resource res)
            return res.resourceType == resourceType;
        return false;
    }
}
