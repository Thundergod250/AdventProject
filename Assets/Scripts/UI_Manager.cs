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
        foreach (var identifier in GetComponentsInChildren<UIPanelIdentifier>(true))
        {
            if (!panelLookup.ContainsKey(identifier.PanelType))
                panelLookup.Add(identifier.PanelType, identifier.gameObject);
        }
    }

    public void OpenUI(UIPanelType type)
    {
        if (!panelLookup.TryGetValue(type, out var targetUI)) return;

        foreach (var panel in panelLookup.Values)
            panel.SetActive(panel == targetUI);

        previousUI = currentUI;
        currentUI = targetUI;

        // 🔒 Disable player control when UI is open
        playerManipulator?._DisableAllMovement();
    }

    public void CloseCurrentUI()
    {
        if (currentUI != null)
        {
            currentUI.SetActive(false);
            currentUI = null;

            // 🔓 Re-enable player control when UI closes
            playerManipulator?._EnableAllMovement();
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
}