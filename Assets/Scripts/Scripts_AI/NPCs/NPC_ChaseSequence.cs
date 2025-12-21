using UnityEngine;

public class NPC_ChaseSequence : MonoBehaviour
{
    private Transform centerPoint;
    public float Radius = 5f;
    public float Speed = 2f;

    private float angle;

    void Update()
    {
        MoveInACircle(centerPoint);
    }

    public void MoveInACircle(Transform centerPoint)
    {
        if (centerPoint == null) return;

        Debug.LogWarning("I am moving in a circle");
        angle += Speed * Time.deltaTime;

        float x = centerPoint.position.x + Mathf.Cos(angle) * Radius;
        float z = centerPoint.position.z + Mathf.Sin(angle) * Radius;

        transform.position = new Vector3(x, transform.position.y, z);
    }
}
