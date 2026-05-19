using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class BossEnemyInstancer : MonoBehaviour
{
    private MapRender mapRender;
    private MapGeneration dataGenerator;
    private BossBehaviour manager;
    private MovementHandler player;
    private List<EnemySpawns> spawnPositions = new List<EnemySpawns>();
    [SerializeField] private GameObject enemyPrefab;

    struct EnemySpawns
    {
        public Vector3 SpawnPoint;
        public int gridX; // Changed to track the horizontal column index (X)
        public bool canSpawn;
    }

    void Start()
    {   
        mapRender = FindFirstObjectByType<MapRender>();
        dataGenerator = FindFirstObjectByType<MapGeneration>();
        manager = FindFirstObjectByType<BossBehaviour>();

        int size = mapRender.blockPositions.GetLength(0);
        int topRowIndex = size - 1;

        // Loop horizontally across the columns (i represents the X coordinate)
        for (int i = 0; i < size; i++)
        {
            // FIX: We vary 'i' (horizontal X) and lock the second index to 'topRowIndex' (depth Y/Z)
            Vector3 worldPos = mapRender.blockPositions[i, topRowIndex];

            // Safely check if the map data grid at this coordinate is a wall
            bool isWalkable = dataGenerator.Grid[i, topRowIndex].block != MapGeneration.WALL;

            spawnPositions.Add(new EnemySpawns 
            { 
                SpawnPoint = worldPos + Vector3.up * 1.05f, 
                gridX = i, // Remember its horizontal grid index
                canSpawn = isWalkable 
            });
        }
    }

    public async Task MakeEnemies(GameObject player)
    {
        // 1. Filter out options that are walls so we don't waste spawn attempts
        var walkableSpawns = spawnPositions.Where(s => s.canSpawn).ToList();
        if (walkableSpawns.Count == 0) return;

        // 2. Pick a random open spot from the available top row points
        int randomIndex = Random.Range(0, walkableSpawns.Count);
        EnemySpawns selectedSpawn = walkableSpawns[randomIndex];

        // 3. Instantiate the bat
        GameObject enemy = Instantiate(enemyPrefab, selectedSpawn.SpawnPoint, Quaternion.identity);
        
        batBehaviour batScript = enemy.GetComponent<batBehaviour>();

        batScript.player = player.GetComponent<MovementHandler>();
        batScript.movementHandler = enemy.AddComponent<AIMovementHandler>();
        batScript.entityMovementHandler = enemy.AddComponent<EntityMovementHandler>();
        
        batScript.movementHandler.map = dataGenerator;
        batScript.entityMovementHandler.movementTarget = enemy;
        batScript.entityMovementHandler.r = mapRender;

        // 4. FIX: Assign the grid coordinates matching the horizontal index first, then the locked top row
        int topRowIndex = dataGenerator.Grid.GetLength(0) - 1;
        batScript.pos = new int[] { selectedSpawn.gridX, topRowIndex };

        // 5. Setup runtime dependencies
        BatStatsForBoss enemyStats = enemy.AddComponent<BatStatsForBoss>();
        BatCollisionForBoss colManager = enemy.AddComponent<BatCollisionForBoss>();
        
        colManager.batStats = enemyStats;
        enemyStats.behaviour = batScript;
        enemyStats.manager = manager;
    }
}