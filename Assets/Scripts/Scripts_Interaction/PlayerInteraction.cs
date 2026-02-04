using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Raycast Settings")]
    [SerializeField] private float rayLength = 5f;

    [Header("References")]
    [SerializeField] private Camera playerCamera;

    [Header("OverlapBox Fallback")]
    [SerializeField] private Vector3 boxSize = new Vector3(1f, 1f, 1f);
    [SerializeField] private Vector3 boxOffset = Vector3.zero;

    [Header("General Settings")]
    [SerializeField] private LayerMask interactableMask;
    [SerializeField] private UI_Interaction ui_interactionTab;
    [SerializeField] private float enableDelay = 0.1f;

    private Interactable currentInteractable;
    private Coroutine raycastRoutine;
    private Coroutine enableRoutine;
    private WaitForSeconds raycastInterval = new WaitForSeconds(0.1f);

    private void OnEnable()
    {
        if (enableRoutine == null)
            enableRoutine = StartCoroutine(EnableWithDelay());
    }

    private void OnDisable()
    {
        if (raycastRoutine != null)
        {
            StopCoroutine(raycastRoutine);
            raycastRoutine = null;
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

        if (raycastRoutine == null)
            raycastRoutine = StartCoroutine(RaycastRoutine());

        enableRoutine = null;
    }

    private IEnumerator RaycastRoutine()
    {
        while (true)
        {
            Interactable closest = null;
            float closestDistance = Mathf.Infinity;

            // 🔍 Primary: Camera Raycast (FPS view)
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, rayLength, interactableMask))
            {
                var interactable = hit.collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    float dist = Vector3.Distance(transform.position, hit.point);
                    closest = interactable;
                    closestDistance = dist;
                }
            }

            // 📦 Fallback: OverlapBox
            //if (closest == null)
            //{
            //    Vector3 boxCenter = transform.position + transform.TransformDirection(boxOffset);
            //    Collider[] hits = Physics.OverlapBox(boxCenter, boxSize * 0.5f, transform.rotation, interactableMask);

            //    foreach (var col in hits)
            //    {
            //        var interactable = col.GetComponent<Interactable>();
            //        if (interactable != null)
            //        {
            //            float dist = Vector3.Distance(transform.position, col.transform.position);
            //            if (dist < closestDistance)
            //            {
            //                closest = interactable;
            //                closestDistance = dist;
            //            }
            //        }
            //    }
            //}

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

            yield return raycastInterval;
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
        Gizmos.color = Color.cyan;
        if (playerCamera != null)
        {
            Gizmos.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * rayLength);
        }

        Gizmos.color = Color.yellow;
        Vector3 boxCenter = transform.position + transform.TransformDirection(boxOffset);
        Gizmos.matrix = Matrix4x4.TRS(boxCenter, transform.rotation, boxSize);
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}
