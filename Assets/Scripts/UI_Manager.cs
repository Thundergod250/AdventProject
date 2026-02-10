using UnityEngine;
using System.Collections.Generic;

public class UI_Manager : MonoBehaviour
{
    private Dictionary<UIPanelType, GameObject> panelLookup = new();
    private GameObject currentUI;
    private GameObject previousUI;

    [SerializeField] private PlayerManipulator playerManipulator;

    private void Awake()
    {
        // Register all panels with identifiers
        foreach (var identifier in GetComponentsInChildren<UIPanelIdentifier>(true))
        {
            if (!panelLookup.ContainsKey(identifier.PanelType))
                panelLookup.Add(identifier.PanelType, identifier.gameObject);
        }

        // ✅ Default to MainUI as the initial panel
        if (panelLookup.TryGetValue(UIPanelType.MainUI, out var mainUI))
        {
            previousUI = mainUI;
            currentUI = mainUI;
            mainUI.SetActive(true);

            // Enable player control when starting on MainUI
            playerManipulator?._EnableAllMovement();
            SetCursorVisibility(false); // hide cursor in gameplay
        }
    }

    public void OpenUI(UIPanelType type)
    {
        if (!panelLookup.TryGetValue(type, out var targetUI)) return;

        // Activate only the target panel
        foreach (var panel in panelLookup.Values)
            panel.SetActive(panel == targetUI);

        previousUI = currentUI;
        currentUI = targetUI;

        // 🔒 Lock or 🔓 unlock player depending on panel type
        if (type == UIPanelType.MainUI)
        {
            playerManipulator?._EnableAllMovement();
            SetCursorVisibility(false); // hide cursor
        }
        else
        {
            playerManipulator?._DisableAllMovement();
            SetCursorVisibility(true); // show cursor
        }
    }

    public void CloseCurrentUI()
    {
        if (currentUI != null)
        {
            currentUI.SetActive(false);
            currentUI = null;

            // 🔓 Always return to MainUI when closing
            if (panelLookup.TryGetValue(UIPanelType.MainUI, out var mainUI))
            {
                mainUI.SetActive(true);
                currentUI = mainUI;

                playerManipulator?._EnableAllMovement();
                GameManager.Instance.PlayerController.PlayerInput.enabled = true;
                SetCursorVisibility(false); // hide cursor
            }
        }
    }

    public void ToggleUI(UIPanelType type)
    {
        if (currentUI != null && GetPanelType(currentUI) == type)
            CloseCurrentUI();
        else
            OpenUI(type);
    }

    public void GoBackToPreviousUI()
    {
        if (previousUI != null)
            OpenUI(GetPanelType(previousUI));
    }

    private UIPanelType GetPanelType(GameObject panel)
    {
        var id = panel.GetComponent<UIPanelIdentifier>();
        return id != null ? id.PanelType : UIPanelType.None;
    }
    
    public bool IsUIBlockingGameplay() => GetPanelType(currentUI) != UIPanelType.MainUI;

    // === Cursor helper ===
    private void SetCursorVisibility(bool visible)
    {
        Cursor.visible = visible;
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
