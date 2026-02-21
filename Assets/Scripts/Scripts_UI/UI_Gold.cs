using TMPro;
using UnityEngine;

public class UI_Gold : MonoBehaviour
{
    [SerializeField] private GoldManager goldManager;
    [SerializeField] private TextMeshProUGUI[] goldTexts; // assign all gold text fields in Inspector

    private void Update()
    {
        if (goldManager != null)
        {
            string goldValue = goldManager.PlayerGold.ToString();

            // Update all gold text fields in one loop
            foreach (var goldText in goldTexts)
            {
                if (goldText != null)
                    goldText.text = goldValue;
            }
        }
    }
}
