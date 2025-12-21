using UnityEngine;
using UnityEngine.Events;

public class CursorManager : MonoBehaviour
{
    public UnityEvent EvtOnEnable, EvtOnDisable;

    [Header("Cursor Settings")]
    [SerializeField] CursorLockMode lockModeWhenEnabled = CursorLockMode.None;
    [SerializeField] CursorLockMode lockModeWhenDisabled = CursorLockMode.Locked;

    private void OnEnable() => EvtOnEnable?.Invoke();
    private void OnDisable() => EvtOnDisable?.Invoke();

    public void _ToggleCursor(bool value)
    {
        Cursor.visible = value;
        Cursor.lockState = value ? lockModeWhenEnabled : lockModeWhenDisabled;
    }
}