using UnityEngine;
using System.Collections.Generic;

public class UI_Manager : MonoBehaviour
{
    public UI_Interaction UI_Interaction;
    public UI_Gold UI_Gold;
    public UI_Grab_Tab UI_Grab_Tab;

    [SerializeField] private GameObject mainUiGroup;
    [SerializeField] private GameObject towerUpgrades;

    private List<GameObject> uiGroups;

    private void Awake()
    {
        uiGroups = new List<GameObject> { mainUiGroup, towerUpgrades };
    }

    public void FocusUI(GameObject targetGroup)
    {
        foreach (var group in uiGroups)
        {
            if (group != null)
                group.SetActive(group == targetGroup);
        }
    }

    public void FocusMainUIGroup() => FocusUI(mainUiGroup);

    public void FocusTowerUpgrades() => FocusUI(towerUpgrades);
    
    public void RegisterUIGroup(GameObject newGroup)
    {
        if (newGroup != null && !uiGroups.Contains(newGroup))
            uiGroups.Add(newGroup);
    }
}