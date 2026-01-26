using UnityEngine;

public class FarmGridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public int gridWidth = 4;
    public int gridHeight = 4;
    public float tileSpacing = 2f;

    [Header("Prefabs")]
    public GameObject farmTilePrefab;

    void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        if (farmTilePrefab == null)
        {
            Debug.LogError("FarmTilePrefab is not assigned!");
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
                Vector3 localPos = new Vector3(
                    x * tileSpacing - offsetX,
                    0f,
                    y * tileSpacing - offsetZ
                );

                // Convert local position into world space using manager’s transform
                Vector3 worldPos = transform.TransformPoint(localPos);

                // Instantiate with manager’s rotation
                GameObject tile = Instantiate(farmTilePrefab, worldPos, transform.rotation);
                tile.name = $"Tile_{x}_{y}";
                tile.transform.parent = transform; // keep hierarchy tidy
            }
        }
    }
}