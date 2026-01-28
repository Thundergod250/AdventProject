using TMPro;
using UnityEngine;

public class UI_ProfitBoard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private CampFunding campFunding; // reference to the funding system

    private void Awake()
    {
        if (campFunding != null) 
            campFunding.EvtOnMoneyChange.AddListener(UpdateMoneyUI);
    }

    private void OnDestroy()
    {
        if (campFunding != null) 
            campFunding.EvtOnMoneyChange.RemoveListener(UpdateMoneyUI);
    }

    private void UpdateMoneyUI(int money)
    {
        if (text != null) 
            text.text = $"{money}";
    }
}