using Unity.VisualScripting;
using UnityEngine;

public class MapRender : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private MapGeneration dataGenerator;

    [Header("Prefabs")]
    //[SerializeField] private GameObject cubePrefab;
    [SerializeField] private GameObject FloorPrefab;
    [SerializeField] private GameObject WallPrefab;
    [SerializeField] private GameObject ForestPrefab;

    [Header("Visual Settings")]
    [SerializeField] public float spacing = 1.1f;

    [Header("Materials")]
    [SerializeField] private Material FloorMaterial;
    [SerializeField] private Material WallMaterial;
    [SerializeField] private Material ForestMaterial;

    private Vector3 SideCenter;
    private Vector3 TopCenter;
    

    [Header("Gameplay Scripts")]
    [SerializeField] private AIplacementManager aiPlacementManager;

    public Vector3 playerSpawnPosition;

    private GameObject mapParent;

    public void RenderMap()
    {
        // 1. Clean up old map if it exists
        if (mapParent != null) Destroy(mapParent);

        mapParent = new GameObject("GeneratedMap_Visuals");

        int[,] grid = dataGenerator.Grid;
        int size = grid.GetLength(0);
        aiPlacementManager.GenerateEnemySpawnPositions();

        // 2. Loop through the data and spawn visuals
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                // Calculate world position
                Vector3 pos = new Vector3(x * spacing, 0, y * spacing);

                if (aiPlacementManager.CheckList(x, y) && grid[x, y] == MapGeneration.WALL)
                {
                    grid[x, y] = MapGeneration.FLOOR; // Change wall to floor if it's an enemy spawn point
                }

                // 3. Apply Visuals based on Data
                switch (grid[x, y])
                {
                    case MapGeneration.WALL:
                        GameObject wall = Instantiate(WallPrefab, pos, Quaternion.identity, mapParent.transform);
                        wall.name = $"Cell_{x}_{y}";

                        MeshRenderer renderer_w = wall.GetComponent<MeshRenderer>();
                        renderer_w.material = WallMaterial;
                        // wall.transform.position += Vector3.up * 0.5f;
                        break;

                    case MapGeneration.FOREST:
                        GameObject forest = Instantiate(ForestPrefab, pos, Quaternion.identity, mapParent.transform);
                        forest.name = $"Cell_{x}_{y}";

                        MeshRenderer renderer_fo = forest.GetComponent<MeshRenderer>();
                        renderer_fo.material = ForestMaterial;
                        break;

                    case MapGeneration.FLOOR:
                    default:
                        GameObject floor = Instantiate(FloorPrefab, pos, Quaternion.identity, mapParent.transform);
                        floor.name = $"Cell_{x}_{y}";

                        MeshRenderer renderer_fl = floor.GetComponent<MeshRenderer>();
                        renderer_fl.material = FloorMaterial;
                        break;
                }
                if (aiPlacementManager.CheckList(x, y))
                    aiPlacementManager.AddSpawnPosition(new Vector2(x, y), pos + Vector3.up * 1.05f); // Adjust height to sit on floor

                if (dataGenerator.spawnPos.x == x && dataGenerator.spawnPos.y == y)
                    playerSpawnPosition = pos + Vector3.up * 1.2f; // Adjust height to sit on floor

            }
        }
        
    }
}