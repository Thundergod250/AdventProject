using UnityEngine;

public class SetTowerSpawn : MonoBehaviour
{
    public void SetTowerSpawnTransform(TowerNodeManager node) => GameManager.Instance.CurrentTowerNode = node;

    public void SpawnTower(GameObject obj) => GameManager.Instance.SpawnTower(obj);
}
