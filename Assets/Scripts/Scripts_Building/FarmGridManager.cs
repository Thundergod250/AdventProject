using UnityEngine;

public class FarmGridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public int gridWidth = 4;
    public int gridHeight = 4;
    public float tileSpacing = 2f;

    [Header("Prefabs")]
    public GameObject buyPlatformTilePrefab;
    public GameObject farmTilePrefab;

    private void Start() => GenerateGrid();

    private void GenerateGrid()
    {
        if (buyPlatformTilePrefab == null || farmTilePrefab == null)
        {
            Debug.LogError("Prefabs are not assigned!");
            return;
        }

        float offsetX = (gridWidth - 1) * tileSpacing / 2f;
        float offsetZ = (gridHeight - 1) * tileSpacing / 2f;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 localPos = new Vector3(x * tileSpacing - offsetX, 0f, y * tileSpacing - offsetZ);
                Vector3 worldPos = transform.TransformPoint(localPos);

                GameObject tile = Instantiate(buyPlatformTilePrefab, worldPos, transform.rotation);
                tile.name = $"Tile_{x}_{y}";
                tile.transform.parent = transform;

                var upgradePlatform = tile.GetComponent<Building_UpgradePlatform>();
                if (upgradePlatform != null)
                {
                    upgradePlatform.gridManager = this;
                    upgradePlatform.gridX = x;
                    upgradePlatform.gridY = y;
                }

                bool isTopRight = (x == gridWidth - 1) && (y == gridHeight - 1);
                tile.SetActive(isTopRight);
            }
        }
    }
}