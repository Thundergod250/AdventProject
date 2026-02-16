using System;
using UnityEngine;
using UnityEngine.Events;

public class Collider_PlayerDetect : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent OnPlayerDeath;

    [Header("Settings")]
    [Tooltip("If true, this component will be disabled after the event is called.")]
    [SerializeField] private bool disableAfterTrigger = false;

    [Tooltip("If true, the trigger will only fire once.")]
    [SerializeField] private bool triggerOnlyOnce = false;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        // Prevent multiple triggers if enabled
        if (triggerOnlyOnce && hasTriggered)
            return;
        
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            Debug.Log(this.name + "Collider Detected Player");
            hasTriggered = true;
            OnPlayerDeath?.Invoke();
            if (disableAfterTrigger) 
                gameObject.SetActive(false); 
        }
    }
}