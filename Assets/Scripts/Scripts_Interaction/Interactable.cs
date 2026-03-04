using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [Header("Interaction Settings")]
    public string interactName;
    public bool IsInteractable = true; // 👈 flag to control availability

    [Header("Interaction Events")]
    public UnityEvent EvtOnFocus;             
    public UnityEvent EvtOnFocusExit;         
    public UnityEvent EvtOnInteract;          
    public UnityEvent<GameObject> EvtOnInteractWithObj; 
    [SerializeField] private UIPanelType uiPanelToOpen;

    // Called by PlayerInteraction when this is the current target
    public void Focus()
    {
        if (!IsInteractable) return;
        EvtOnFocus?.Invoke();
    }

    // Called when no longer targeted
    public void FocusExit()
    {
        if (!IsInteractable) return;
        EvtOnFocusExit?.Invoke();
    }

    // Called when player presses interact key
    public void Interact()
    {
        if (!IsInteractable) return;

        EvtOnInteract?.Invoke();
        EvtOnInteractWithObj?.Invoke(gameObject);
        
        if (uiPanelToOpen != UIPanelType.None) 
            _OpenUI(uiPanelToOpen);
    }

    public bool GetIsInteractable() => IsInteractable;
    public void _EnableInteraction() => IsInteractable = true;
    public void _DisableInteraction() => IsInteractable = false;

    // 👇 Helper method for UnityEvents
    public void _OpenUI(UIPanelType type) => GameManager.Instance.UIManager.OpenUI(type);
}