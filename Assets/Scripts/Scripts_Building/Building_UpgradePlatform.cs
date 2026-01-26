using UnityEngine;
using UnityEngine.Events;

public class Building_UpgradePlatform : MonoBehaviour
{
    public UnityEvent EvtOnPlayerActivated; 
    private bool hasActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasActivated) return;

        if (other.GetComponent<PlayerController>())
        {
            EvtOnPlayerActivated?.Invoke();
            hasActivated = true; 
        }
    }
}