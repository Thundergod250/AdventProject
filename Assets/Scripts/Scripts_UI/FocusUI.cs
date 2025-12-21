using UnityEngine;

public class FocusUI : MonoBehaviour
{
    // === Player Movement Control ===
    public void _FocusInterActionGroup() => GameManager.Instance.UIManager?.FocusMainUIGroup();

    public void _FocusTowerUpgrades() => GameManager.Instance.UIManager?.FocusTowerUpgrades();

    public void _FocusTowerUpgradesWithCondition(TowerController towerController) => GameManager.Instance.UIManager?.FocusTowerUpgradesWithCondition(towerController);
}