using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerInteraction : MonoBehaviour
{
    [Header("OverlapBox Settings")]
    [SerializeField] private float checkInterval = 0.1f;
    [SerializeField] private Vector3 boxSize = new Vector3(1.5f, 1.5f, 1.5f);
    [SerializeField] private Vector3 boxOffset = new Vector3(0f, 0.5f, 1.5f); // forward offset

    [Header("General Settings")]
    [SerializeField] private LayerMask interactableMask;
    [SerializeField] private UI_Interaction ui_interactionTab;
    [SerializeField] private float enableDelay = 0.1f;

    private Interactable currentInteractable;
    private Coroutine overlapRoutine;
    private Coroutine enableRoutine;
    private WaitForSeconds intervalWait;

    private void Awake()
    {
        intervalWait = new WaitForSeconds(checkInterval);
    }

    private void OnEnable()
    {
        if (enableRoutine == null)
            enableRoutine = StartCoroutine(EnableWithDelay());
    }

    private void OnDisable()
    {
        if (overlapRoutine != null)
        {
            StopCoroutine(overlapRoutine);
            overlapRoutine = null;
        }

        if (enableRoutine != null)
        {
            StopCoroutine(enableRoutine);
            enableRoutine = null;
        }
    }

    private IEnumerator EnableWithDelay()
    {
        yield return new WaitForSeconds(enableDelay);

        if (overlapRoutine == null)
            overlapRoutine = StartCoroutine(OverlapRoutine());

        enableRoutine = null;
    }

    private IEnumerator OverlapRoutine()
    {
        while (true)
        {
            Interactable closest = null;
            float closestDistance = Mathf.Infinity;

            // 📦 OverlapBox in front of player
            Vector3 boxCenter = transform.position + transform.TransformDirection(boxOffset);
            Collider[] hits = Physics.OverlapBox(boxCenter, boxSize * 0.5f, transform.rotation, interactableMask);

            foreach (var col in hits)
            {
                var interactable = col.GetComponent<Interactable>();
                if (interactable != null)
                {
                    float dist = Vector3.Distance(transform.position, col.transform.position);
                    if (dist < closestDistance)
                    {
                        closest = interactable;
                        closestDistance = dist;
                    }
                }
            }

            // 🔁 Update UI and focus
            if (closest != currentInteractable)
            {
                if (currentInteractable != null)
                {
                    currentInteractable.FocusExit();
                    ui_interactionTab.Hide();
                }

                currentInteractable = closest;
                if (currentInteractable != null && currentInteractable.GetIsInteractable())
                {
                    currentInteractable.Focus();
                    ui_interactionTab.Show(currentInteractable.interactName);
                }
            }
            else if (currentInteractable != null)
            {
                currentInteractable.Focus();
            }

            yield return intervalWait;
        }
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (!enabled || !ctx.performed || currentInteractable == null)
            return;

        currentInteractable.Interact();
        currentInteractable = null;
        ui_interactionTab.Hide();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 boxCenter = transform.position + transform.TransformDirection(boxOffset);
        Gizmos.matrix = Matrix4x4.TRS(boxCenter, transform.rotation, boxSize);
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}
