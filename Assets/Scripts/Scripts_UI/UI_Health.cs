using UnityEngine;
using UnityEngine.UI;

public class UI_Health : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image healthBar;
    [SerializeField] private Health healthSource;

    [Header("Proximity Settings")]
    [SerializeField] private bool proximityShow = true; // default true
    [SerializeField] private float showDistance = 8f;   // editable distance threshold
    [SerializeField] private Canvas canvas;

    private Transform playerTransform;

    private void Start()
    {
        playerTransform = GameManager.Instance.PlayerController.gameObject.transform;

        if (healthSource != null)
        {
            healthSource.OnDamaged.AddListener(UpdateHealthBar);
            healthSource.OnDeath.AddListener(HideHealthBar);
        }

        UpdateHealthBar(healthSource.GetCurrentHealth());
    }

    private void Update()
    {
        if (!proximityShow || playerTransform == null || canvas == null) return;

        float dist = Vector3.Distance(playerTransform.position, transform.position);
        bool shouldShow = dist <= showDistance && !healthSource.Equals(null);

        if (canvas.enabled != shouldShow)
            canvas.enabled = shouldShow;
    }

    private void UpdateHealthBar(int currentHealth)
    {
        if (healthBar == null || healthSource == null) return;
        float fillAmount = (float)currentHealth / healthSource.GetMaxHealth();
        healthBar.fillAmount = fillAmount;
    }

    private void HideHealthBar()
    {
        if (canvas != null)
            canvas.enabled = false;
    }
}