using System;
using UnityEngine;
using UnityEngine.Events;

public class Building_UpgradePlatform : MonoBehaviour
{
    public UnityEvent EvtOnPlayerSteppedIn; 
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>()) 
            EvtOnPlayerSteppedIn.Invoke();
    }
}
