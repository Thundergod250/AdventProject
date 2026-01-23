using System.Collections;
using UnityEngine;
using System.Threading.Tasks;

public class ResourceInteractable : MonoBehaviour
{
    [Header("Effects")]
    public float hitDisableTime = 0.1f;

    [Header("Drop")]
    public GameObject ResourcePrefab;
    public Transform dropPoint;

    private bool isDead = false;

    public async void Choptask()
    {
        await HitEffect();
    }

    public async Task HitEffect()
    {
        if(this.gameObject != null)
        {
            gameObject.SetActive(false);
            await Task.Delay(1000); // 1 second
            gameObject.SetActive(true);
        }
    }

    public void SpawnResource()
    {
        if (ResourcePrefab != null)
        {
            Vector3 spawnPos = dropPoint != null ? dropPoint.position : transform.position;
            Instantiate(ResourcePrefab, spawnPos, Quaternion.identity);
        }

        Destroy(gameObject);
    }

}
