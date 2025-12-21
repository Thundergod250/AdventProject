using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_Interaction_Tab : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI interactionName;
    [SerializeField] private Image highlightFrame;

    public void Show(string name)
    {
        interactionName.text = $"{name}";
        interactionName.gameObject.SetActive(true);
        if (highlightFrame != null)
            highlightFrame.enabled = true;
    }
}