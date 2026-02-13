using UnityEngine;

public class PlayerDeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null)
            GameManager.Instance.PlayerDeathManager.CallDeathAndRespawnRoutine();
    }
}
