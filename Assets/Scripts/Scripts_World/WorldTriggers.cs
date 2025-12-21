using UnityEngine;
using UnityEngine.Events;

public class WorldTriggers : MonoBehaviour
{
    public UnityEvent EvtOnPlayerTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovement>()) 
            EvtOnPlayerTrigger?.Invoke();
    }
}
