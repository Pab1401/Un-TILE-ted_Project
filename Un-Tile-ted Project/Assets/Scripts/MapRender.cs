using UnityEngine;

public class MapRender : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private MapGeneration dataGenerator;

    [Header("Prefabs")]
    [SerializeField] private GameObject cubePrefab;

    [Header("Visual Settings")]
    [SerializeField] public float spacing = 1.1f;
    [SerializeField] private Material floorMaterial;
    [SerializeField] private Material wallMaterial;
    [SerializeField] private Material forestMaterial;
    [SerializeField] private GameObject floorPrefab;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject forestPrefab;

    public GameObject playerSpawnPosition;

    private GameObject mapParent;

    public void RenderMap()
    {
        // 1. Clean up old map if it exists
        if (mapParent != null) Destroy(mapParent);

        mapParent = new GameObject("GeneratedMap_Visuals");

        int[,] grid = dataGenerator.Grid;
        int size = grid.GetLength(0);

        // 2. Loop through the data and spawn visuals
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                // Calculate world position
                Vector3 pos = new Vector3(x * spacing, 0, y * spacing);

                // 3. Apply Visuals based on Data
                switch (grid[x, y])
                {
                    case MapGeneration.WALL:
                        GameObject wall = Instantiate(wallPrefab, pos, Quaternion.identity, mapParent.transform);
                        wall.name = $"Cell_{x}_{y}";

                        MeshRenderer renderer_w = wall.GetComponent<MeshRenderer>();
                        renderer_w.material = wallMaterial;
                        // Visual flair: make walls taller
                        wall.transform.localScale = new Vector3(1, 2, 1);
                        wall.transform.position += Vector3.up * 0.5f; // Adjust height so it sits on floor
                        break;

                    case MapGeneration.FOREST:
                        GameObject forest = Instantiate(forestPrefab, pos, Quaternion.identity, mapParent.transform);
                        forest.name = $"Cell_{x}_{y}";

                        MeshRenderer renderer_fo = forest.GetComponent<MeshRenderer>();
                        renderer_fo.material = forestMaterial;
                        break;

                    case MapGeneration.FLOOR:
                    default:
                        GameObject floor = Instantiate(floorPrefab, pos, Quaternion.identity, mapParent.transform);
                        floor.name = $"Cell_{x}_{y}";

                        MeshRenderer renderer_fl = floor.GetComponent<MeshRenderer>();
                        renderer_fl.material = floorMaterial;
                        break;
                }
            }
        }
    }
}