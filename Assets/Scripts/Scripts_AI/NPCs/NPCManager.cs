using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class NPCManager : MonoBehaviour
{
    public NPCDialogue NpcDialogue; 
    public NPCMovement NpcMovement;

    [Header("NPC Events")]
    public List<UnityEvent> npcEvents = new();

    public void ActivateEvent(int index)
    {
        if (index >= 0 && index < npcEvents.Count)
            npcEvents[index]?.Invoke();
    }
}