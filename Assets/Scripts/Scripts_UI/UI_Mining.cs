using UnityEngine;

public class UI_Mining : MonoBehaviour
{
    public void ToggleMiningUI()
    {
        GameManager.Instance.UIManager.ToggleUI(UIPanelType.Mine);
    }
}
