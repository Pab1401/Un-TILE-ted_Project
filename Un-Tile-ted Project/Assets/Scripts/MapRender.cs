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
    [SerializeField] private int wallBorderThickness = 16; 
    public bool isBossRoom; // Check this to disable the outside walls

    [Header("Materials")]
    [SerializeField] private Material FloorMaterial;
    [SerializeField] private Material WallMaterial;
    [SerializeField] private Material ForestMaterial;
    [SerializeField] private Material PlaneMaterial; 

    private Vector3 SideCenter;
    private Vector3 TopCenter;
    

    [Header("Gameplay Scripts")]
    [SerializeField] private AIplacementManager aiPlacementManager;

    public Vector3 playerSpawnPosition;

    private GameObject mapParent;

    public Vector3[,] blockPositions;

    
    public void RenderMap()
    {
        dataGenerator.Boss = isBossRoom;
        // 1. Clean up old map if it exists
        if (mapParent != null) Destroy(mapParent);

        mapParent = new GameObject("GeneratedMap_Visuals");

        blockPositions = new Vector3[dataGenerator.Grid.GetLength(0), dataGenerator.Grid.GetLength(1)];

        MapGeneration.Cells[,] grid = dataGenerator.Grid;
        int size = grid.GetLength(0);
        aiPlacementManager.GenerateEnemySpawnPositions();

        // 2. Loop through the data and spawn visuals
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                // Calculate world position
                Vector3 pos = new Vector3(x * spacing, 0, y * spacing);
                blockPositions[x, y] = pos; 
                    

                if (aiPlacementManager.CheckList(x, y) && grid[x, y].block == MapGeneration.WALL)
                {
                    grid[x, y].block = MapGeneration.FLOOR; // Change wall to floor if it's an enemy spawn point
                }

                // 3. Apply Visuals based on Data
                switch (grid[x, y].block)
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
                {
                    aiPlacementManager.AddSpawnPosition(new Vector2(x, y), pos + Vector3.up * 1.05f); // Adjust height to sit on floor
                    grid[x, y].taken = true;
                }
                if (dataGenerator.spawnPos.x == x && dataGenerator.spawnPos.y == y)
                {
                    playerSpawnPosition = pos + Vector3.up * 1.2f; // Adjust height to sit on floor
                    // grid[x, y].taken = true;
                }

            }
        }

        // 4. Spawn the surrounding Wall Border ONLY if it's NOT a boss room
        if (!isBossRoom)
        {
            GenerateWallBorder(size);
        }

        // 5. Create and place the Plane underneath
        CreateUnderMapPlane(size);
    }

    private void GenerateWallBorder(int size)
    {
        for (int x = -wallBorderThickness; x < size + wallBorderThickness; x++)
        {
            for (int y = -wallBorderThickness; y < size + wallBorderThickness; y++)
            {
                if (x < 0 || x >= size || y < 0 || y >= size)
                {
                    Vector3 pos = new Vector3(x * spacing, 0, y * spacing);
                    GameObject borderWall = Instantiate(WallPrefab, pos, Quaternion.identity, mapParent.transform);
                    borderWall.name = $"Border_Wall_{x}_{y}";

                    MeshRenderer renderer_w = borderWall.GetComponent<MeshRenderer>();
                    if (renderer_w != null)
                    {
                        renderer_w.material = WallMaterial;
                    }
                }
            }
        }
    }

    private void CreateUnderMapPlane(int size)
    {
        // Adjust plane calculations based on whether we have borders or not
        int thickness = isBossRoom ? 0 : wallBorderThickness;
        float totalCells = size + (thickness * 2);
        float totalWorldSize = totalCells * spacing;

        float centerOffset = ((size - 1) * spacing) / 2f;
        Vector3 planePosition = new Vector3(centerOffset, -0.01f, centerOffset); 

        GameObject underPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        underPlane.name = "Map_UnderPlane";
        underPlane.transform.position = planePosition;
        underPlane.transform.SetParent(mapParent.transform);

        float planeScaleX = totalWorldSize / 10f;
        float planeScaleZ = totalWorldSize / 10f;
        underPlane.transform.localScale = new Vector3(planeScaleX, 1f, planeScaleZ);

        if (PlaneMaterial != null)
        {
            MeshRenderer planeRenderer = underPlane.GetComponent<MeshRenderer>();
            planeRenderer.material = PlaneMaterial;
        }
    }
}