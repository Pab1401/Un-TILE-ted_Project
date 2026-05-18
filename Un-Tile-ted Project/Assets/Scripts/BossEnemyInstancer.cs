using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class BossEnemyInstancer : MonoBehaviour
{
    private MapRender mapRender;
    private MapGeneration dataGenerator;
    private BossBehaviour manager;
    private List<EnemySpawns> spawnPositions = new List<EnemySpawns>();
    [SerializeField] private GameObject enemyPrefab;
    struct EnemySpawns
    {
        public Vector3 SpawnPoint;
        public bool canSpawn;
    }
    void Start()
    {
        mapRender = FindFirstObjectByType<MapRender>();
        dataGenerator = FindFirstObjectByType<MapGeneration>();
        manager = FindFirstObjectByType<BossBehaviour>();
        int size = mapRender.blockPositions.GetLength(0);
        for (int i = 0; i < size; i++)
        {
            spawnPositions.Add(new EnemySpawns { SpawnPoint = mapRender.blockPositions[size - 1, i] + Vector3.up, canSpawn = dataGenerator.Grid[size - 1, i].block == MapGeneration.WALL ? false : true });
        }
    }
    public async Task MakeEnemies()
    {
        int index = Random.Range(0, spawnPositions.Count);
        if (!spawnPositions[index].canSpawn)
            return;
        GameObject enemy = Instantiate(enemyPrefab, spawnPositions[index].SpawnPoint, Quaternion.identity);
        batBehaviour batScript = enemy.GetComponent<batBehaviour>();
        batScript.movementHandler = enemy.AddComponent<AIMovementHandler>();
        batScript.entityMovementHandler = enemy.AddComponent<EntityMovementHandler>();
        batScript.movementHandler.map = dataGenerator;
        batScript.entityMovementHandler.movementTarget = enemy;
        batScript.entityMovementHandler.r = mapRender;
        batScript.pos = new int[] {dataGenerator.Grid.GetLength(0), index};
        BatStatsForBoss enemyStats = enemy.AddComponent<BatStatsForBoss>();
        BatCollisionForBoss colManager = enemy.AddComponent<BatCollisionForBoss>();
        colManager.batStats = enemyStats;
        enemyStats.behaviour = batScript;
        enemyStats.manager = manager;
    }
}
