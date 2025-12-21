using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [Header("Interaction Settings")]
    public string interactName;
    [SerializeField] private bool isInteractable = true; // 👈 flag to control availability

    [Header("Interaction Events")]
    public UnityEvent EvtOnFocus;             
    public UnityEvent EvtOnFocusExit;         
    public UnityEvent EvtOnInteract;          
    public UnityEvent<GameObject> EvtOnInteractWithObj; 


    // Called by PlayerInteraction when this is the current target
    public void Focus()
    {
        if (!isInteractable) return;
        EvtOnFocus?.Invoke();
    }

    // Called when no longer targeted
    public void FocusExit()
    {
        if (!isInteractable) return;
        EvtOnFocusExit?.Invoke();
    }

    // Called when player presses interact key
    public void Interact()
    {
        if (!isInteractable) return;

        EvtOnInteract?.Invoke();
        EvtOnInteractWithObj?.Invoke(gameObject);
    }

    public bool GetIsInteractable() => isInteractable;
    public void _EnableInteraction() => isInteractable = true;

    public void _DisableInteraction() => isInteractable = false;
}