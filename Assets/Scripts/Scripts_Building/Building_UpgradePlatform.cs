using UnityEngine;

public class Building_UpgradePlatform : MonoBehaviour
{
    [HideInInspector] public FarmGridManager gridManager;
    [HideInInspector] public int gridX;
    [HideInInspector] public int gridY;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            EnableAdjacentTiles();
        }
    }

    private void EnableAdjacentTiles()
    {
        if (gridManager == null) return;

        EnableTileAt(gridX - 1, gridY);     // West
        EnableTileAt(gridX + 1, gridY);     // East
        EnableTileAt(gridX, gridY - 1);     // South
        EnableTileAt(gridX, gridY + 1);     // North
    }

    private void EnableTileAt(int x, int y)
    {
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