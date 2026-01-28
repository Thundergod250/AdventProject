using UnityEngine;
using System.Collections.Generic;

public class UI_Manager : MonoBehaviour
{
    private Dictionary<UIPanelType, GameObject> panelLookup = new();
    private GameObject currentUI;
    private GameObject previousUI;

    private void Awake()
    {
        foreach (UIPanelIdentifier identifier in GetComponentsInChildren<UIPanelIdentifier>(true))
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
    }

    public void CloseCurrentUI()
    {
        if (currentUI != null)
        {
            currentUI.SetActive(false);
            currentUI = null;
        }
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