using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class UI_ReticleRaycast : MonoBehaviour
{
    public Camera PlayerCamera;
    public RectTransform CrosshairUI;
    public float RayLength = 100f;
    public Ray ray;

    [SerializeField] private Vector2 screenPos;
    private Interactable currentInteractable;
    [SerializeField] private UI_Interaction ui_interactionTab;

    [Header("General Settings")]
    [SerializeField] private LayerMask interactableMask;
    [SerializeField] private float enableDelay = 0.1f;

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
    private void OnDrawGizmos()
    {
        if (PlayerCamera == null || CrosshairUI == null)
            return;

        Gizmos.color = Color.blue;

        // Convert UI position → screen position
        screenPos =
            RectTransformUtility.WorldToScreenPoint(null, CrosshairUI.position);

        // Create ray from camera through crosshair
        ray = PlayerCamera.ScreenPointToRay(screenPos);

        Gizmos.DrawRay(ray.origin, ray.direction * RayLength);

    }

    public bool GetRaycastHit(out RaycastHit hit)
    {
        hit = new RaycastHit();

        if (PlayerCamera == null || CrosshairUI == null) return false;

        if (Physics.Raycast(ray, out hit, RayLength))
        {
            return true;
        }


        return false;

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
            if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out RaycastHit hit, RayLength, interactableMask))
            {
                var interactable = hit.collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    float dist = Vector3.Distance(transform.position, hit.point);
                    closest = interactable;
                    closestDistance = dist;
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
}
