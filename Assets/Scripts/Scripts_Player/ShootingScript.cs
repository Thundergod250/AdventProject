using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootingScript : MonoBehaviour
{
    public void CallShooting(InputAction.CallbackContext context)
    {
        Debug.LogWarning("Shooting a Bullety");
    }
}
