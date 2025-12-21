using TMPro;
using UnityEngine;

public class UI_Gold : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private GoldManager goldManager;

    private void Update()
    {
        if (goldManager != null)
            goldText.text = goldManager.PlayerGold.ToString(); 
    }
}