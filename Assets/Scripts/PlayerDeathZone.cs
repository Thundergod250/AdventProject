using UnityEngine;

public class PlayerDeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerManipulator player = other.GetComponent<PlayerManipulator>();

        if (player != null)
            GameManager.Instance.PlayerDeathManager.CallDeathAndRespawnRoutine();
    }
}
