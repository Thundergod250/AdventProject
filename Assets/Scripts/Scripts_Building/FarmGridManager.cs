using UnityEngine;

public class FarmGridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public int gridWidth = 4;
    public int gridHeight = 4;
    public float tileSpacing = 2f;

    [Header("Prefabs")]
    public GameObject buyPlatformTilePrefab;

    private void Start() => GenerateGrid();

    private void GenerateGrid()
    {
        if (buyPlatformTilePrefab == null)
        {
            Debug.LogError("BuyPlatformTilePrefab is not assigned!");
            return;
        }

        // Calculate offsets so grid is centered
        float offsetX = (gridWidth - 1) * tileSpacing / 2f;
        float offsetZ = (gridHeight - 1) * tileSpacing / 2f;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                // Local position relative to manager
                Vector3 localPos = new Vector3(x * tileSpacing - offsetX, 0f, y * tileSpacing - offsetZ);

                // Convert local position into world space using manager’s transform
                Vector3 worldPos = transform.TransformPoint(localPos);

                // Instantiate with manager’s rotation
                GameObject tile = Instantiate(buyPlatformTilePrefab, worldPos, transform.rotation);
                tile.name = $"Tile_{x}_{y}";
                tile.transform.parent = transform;

                // Assign grid data directly
                var upgradePlatform = tile.GetComponent<Building_UpgradePlatform>();
                if (upgradePlatform != null)
                {
                    upgradePlatform.gridManager = this;
                    upgradePlatform.gridX = x;
                    upgradePlatform.gridY = y;
                }

                // Disable all tiles except the top‑right one
                bool isTopRight = (x == gridWidth - 1) && (y == gridHeight - 1);
                tile.SetActive(isTopRight);
            }
        }
    }
}