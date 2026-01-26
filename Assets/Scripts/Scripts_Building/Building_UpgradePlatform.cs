using UnityEngine;

public class Building_UpgradePlatform : MonoBehaviour
{
    [HideInInspector] public FarmGridManager gridManager;
    [HideInInspector] public int gridX;
    [HideInInspector] public int gridY;

    private bool hasUpgraded = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasUpgraded) return;

        if (other.GetComponent<PlayerController>())
        {
            UpgradeToFarmTile();
            EnableAdjacentTiles();
            hasUpgraded = true;
        }
    }

    private void UpgradeToFarmTile()
    {
        if (gridManager == null || gridManager.farmTilePrefab == null) return;

        Vector3 position = transform.position;
        Quaternion rotation = transform.rotation;
        Transform parent = transform.parent;

        Destroy(gameObject);

        GameObject farmTile = Instantiate(gridManager.farmTilePrefab, position, rotation);
        farmTile.name = $"FarmTile_{gridX}_{gridY}";
        farmTile.transform.parent = parent;
    }

    private void EnableAdjacentTiles()
    {
        EnableTileAt(gridX - 1, gridY); // West
        EnableTileAt(gridX + 1, gridY); // East
        EnableTileAt(gridX, gridY - 1); // South
        EnableTileAt(gridX, gridY + 1); // North
    }

    private void EnableTileAt(int x, int y)
    {
        if (gridManager == null) return;
        if (x < 0 || y < 0 || x >= gridManager.gridWidth || y >= gridManager.gridHeight)
            return;

        string tileName = $"Tile_{x}_{y}";
        Transform tileTransform = gridManager.transform.Find(tileName);

        if (tileTransform != null && !tileTransform.gameObject.activeSelf)
        {
            tileTransform.gameObject.SetActive(true);
        }
    }
}