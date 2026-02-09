using UnityEngine;

public class UI_Proximity : MonoBehaviour
{
    [Header("Proximity Settings")]
    [SerializeField] private bool proximityShow = true; // default true
    [SerializeField] private float showDistance = 8f;   // editable distance threshold
    [SerializeField] private GameObject targetCanvas;   // canvas GameObject to toggle

    private Transform playerTransform;

    private void Start()
    {
        // Grab player transform from GameManager
        if (GameManager.Instance != null && GameManager.Instance.PlayerController != null)
            playerTransform = GameManager.Instance.PlayerController.transform;

        if (targetCanvas == null)
            Debug.LogWarning($"{name}: TargetCanvas not assigned!");
    }

    private void Update()
    {
        if (!proximityShow || playerTransform == null || targetCanvas == null) return;

        float dist = Vector3.Distance(playerTransform.position, transform.position);
        bool shouldShow = dist <= showDistance;

        // Use activeSelf to check current state, SetActive to change
        if (targetCanvas.activeSelf != shouldShow)
            targetCanvas.SetActive(shouldShow);
    }
}