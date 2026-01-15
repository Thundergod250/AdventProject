using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;

public class PlayerGrab : MonoBehaviour
{
    public bool IsPlayerCarryingObject;

    // === Events ===
    public UnityEvent<GameObject> EvtOnGrab;
    public UnityEvent<GameObject> EvtOnReleaseGrabObj;
    public UnityEvent<GameObject> EvtOnRemovedGrabbedObject;

    [Header("Grab Settings")]
    [SerializeField] private float tossForce = 5f;
    [SerializeField] private float grabDelay = 2f;

    [Header("Downward Ramp Settings")]
    [SerializeField] private float downwardForceMultiplier = 0f; // starting downward force
    [SerializeField] private float downwardRampRate = 5f;        // how fast it increases

    private GameObject currentGrabbedObj;
    private bool isOnCooldown = false;

    public void GrabObject(GameObject obj)
    {
        if(GameManager.Instance.PlayerInventory.TotalWeight < GameManager.Instance.PlayerInventory.MaxWeight)
        {
            GameManager.Instance.PlayerInventory.AddToInventory(obj);
            Destroy(obj);
        }
        else
        {
            GameManager.Instance.PlayerInventory.UITimerCall();
            //GameManager.Instance.PlayerInventory.InventoryStatusUI.SetActive(!GameManager.Instance.PlayerInventory.InventoryStatusUI.activeSelf);
            Debug.LogWarning("No More Space");

        }
    }

    public void ReleaseGrabbedObject()
    {
        if (isOnCooldown || !IsPlayerCarryingObject || currentGrabbedObj == null) return;

        Rigidbody rb = currentGrabbedObj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Re-enable physics + colliders first
            rb.isKinematic = false;
            rb.detectCollisions = true;

            foreach (var col in currentGrabbedObj.GetComponents<Collider>())
                col.enabled = true;

            // ✅ Combine player velocity with toss force
            Vector3 playerVelocity = GameManager.Instance.PlayerController.PlayerMovement.GetVelocity();
            Vector3 releaseForce = playerVelocity + (transform.forward * tossForce);

            rb.linearVelocity = releaseForce;

            // ✅ Start coroutine to apply downward ramp
            StartCoroutine(ApplyDownwardRamp(rb));
        }

        // Unparent immediately after applying velocity
        currentGrabbedObj.transform.SetParent(null);

        IsPlayerCarryingObject = false;

        // Trigger event
        EvtOnReleaseGrabObj?.Invoke(currentGrabbedObj);

        currentGrabbedObj = null;

        // Start cooldown
        StartCoroutine(ActionCooldown());
    }

    public void RemoveGrabObject()
    {
        if (!IsPlayerCarryingObject || currentGrabbedObj == null) return;

        currentGrabbedObj.transform.SetParent(null);

        Rigidbody rb = currentGrabbedObj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.detectCollisions = true;
        }

        foreach (var col in currentGrabbedObj.GetComponents<Collider>())
            col.enabled = true;

        IsPlayerCarryingObject = false;

        EvtOnRemovedGrabbedObject?.Invoke(currentGrabbedObj);

        currentGrabbedObj = null;
    }

    private IEnumerator ApplyDownwardRamp(Rigidbody rb)
    {
        float currentMultiplier = downwardForceMultiplier;

        // Keep applying until object lands (when Rigidbody goes to sleep or touches ground)
        while (rb != null && !rb.IsSleeping())
        {
            rb.AddForce(Vector3.down * currentMultiplier, ForceMode.Acceleration);

            // Increase multiplier over time
            currentMultiplier += downwardRampRate * Time.deltaTime;

            yield return null; // wait until next frame
        }
    }

    private IEnumerator ActionCooldown()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(grabDelay);
        isOnCooldown = false;
    }
}
