using TMPro;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    public TextMeshProUGUI NPCDialogueText;
    public GameObject Canvas; 

    public void _SetDialogueText(string text)
    {
        Canvas.SetActive(true);
        NPCDialogueText.text = text;
    }

    public void _DisableDialogue()
    {
        Canvas.SetActive(false);
        NPCDialogueText.text = " ";
    }
}
