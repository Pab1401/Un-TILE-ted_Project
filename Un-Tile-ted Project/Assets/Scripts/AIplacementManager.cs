using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AIplacementManager : MonoBehaviour
{
    [SerializeField] private MapGeneration dataGenerator;
    [SerializeField] private MapRender mapRender;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] public List<Vector2> enemySpawnPoints;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject scorpionPrefab;
    [SerializeField] private ShootLogic shootLogic;
    [SerializeField] private GameObject scorpionProjectilePrefab;
    [SerializeField] private GameManager GameManager;

    [SerializeField] private int enemyCount;
    //[SerializeField] private int batCount;
    [SerializeField] private int scorpionCount;

    private struct EnemySpawns
    {
        public Vector3 SpawnPosition;
        public Vector2 SpawnPoint;
    }

    // [Range(0, 1)]
    // public int enemyDecision = 1;
    // private List<Vector3> enemySpawnPositions = new List<Vector3>();
    private List<EnemySpawns> enemyList = new List<EnemySpawns>();
    
    public void AddSpawnPosition(Vector2 point ,Vector3 position)  
    {
        EnemySpawns temp;
        temp.SpawnPoint = point;
        temp.SpawnPosition = position;
        enemyList.Add(temp);

        // enemySpawnPositions.Add(position);
    }

    

    public void GenerateEnemySpawnPositions()
    {
        // Debug.Log(enemyList.Count);
        for (int i = 0; i < enemyCount; i++)
        {
            Vector2 temp = new Vector2(Random.Range(0, dataGenerator.Grid.GetLength(0) - 3), Random.Range(0, dataGenerator.Grid.GetLength(1) - 3));

            if (enemySpawnPoints.Contains(temp))
            {
                i--;
            }
            else
            {
                enemySpawnPoints.Add(temp);
            }
            // Debug.Log("Enemy added to: " + temp);
        }
        // Debug.Log(enemySpawnPoints.Count);
        // enemySpawnPoints.Sort();
    }

    public void SpawnEnemies()
    {
        foreach (EnemySpawns EnSp in enemyList)
        {
            if (scorpionCount > 0)
            {
                GameObject enemy = Instantiate(scorpionPrefab, EnSp.SpawnPosition, Quaternion.identity);
                scorpionBehaviour ScorpionScript = enemy.AddComponent<scorpionBehaviour>();
                ScorpionStats enemyStats = enemy.AddComponent<ScorpionStats>();
                Rigidbody rb = enemy.AddComponent<Rigidbody>();
                ScorpionCollisionManager cm = enemy.AddComponent<ScorpionCollisionManager>();

                cm.stats = enemyStats;
                ScorpionScript.stats = enemyStats;
                ScorpionScript.projectilePrefab = scorpionProjectilePrefab;
                ScorpionScript.shootLogic = shootLogic;
                ScorpionScript.player = player;
                enemyStats.manager = GameManager;
                rb.useGravity = false;
                scorpionCount--;
            }
            else
            {
                GameObject enemy = Instantiate(enemyPrefab, EnSp.SpawnPosition, Quaternion.identity);
                batBehaviour batScript = enemy.GetComponent<batBehaviour>();
                batScript.movementHandler = enemy.AddComponent<AIMovementHandler>();
                batScript.entityMovementHandler = enemy.AddComponent<EntityMovementHandler>();
                batScript.movementHandler.map = dataGenerator;
                batScript.entityMovementHandler.movementTarget = enemy;
                batScript.entityMovementHandler.r = mapRender;
                batScript.pos = new int[] {(int)EnSp.SpawnPoint.x, (int)EnSp.SpawnPoint.y};
                BatEnemyStats enemyStats = enemy.AddComponent<BatEnemyStats>();
                BatCollisionManager colManager = enemy.AddComponent<BatCollisionManager>();
                colManager.batStats = enemyStats;
                enemyStats.behaviour = batScript;
                enemyStats.manager = GameManager;
            }
        }


        // if (enemyDecision == 0)
        // {
        //     foreach (EnemySpawns EnSp in enemyList)
        //     {
        //         // Debug.Log("Spawning Bat");
        //         GameObject enemy = Instantiate(enemyPrefab, EnSp.SpawnPosition, Quaternion.identity);
        //         batBehaviour batScript = enemy.GetComponent<batBehaviour>();
        //         batScript.movementHandler = enemy.AddComponent<AIMovementHandler>();
        //         batScript.entityMovementHandler = enemy.AddComponent<EntityMovementHandler>();
        //         batScript.movementHandler.map = dataGenerator;
        //         batScript.entityMovementHandler.movementTarget = enemy;
        //         batScript.entityMovementHandler.r = mapRender;
        //         batScript.pos = new int[] {(int)EnSp.SpawnPoint.x, (int)EnSp.SpawnPoint.y};
        //         BatEnemyStats enemyStats = enemy.AddComponent<BatEnemyStats>();
        //         BatCollisionManager colManager = enemy.AddComponent<BatCollisionManager>();
        //         colManager.batStats = enemyStats;
        //         enemyStats.manager = GameManager;
        //     }
        // }
        // else if (enemyDecision == 1)
        // {
        //     // Debug.Log("Spawning Scorpion");
        //     for (int i = 0; i < enemySpawnPoints.Count; i++)
        //     {
        //         GameObject enemy = Instantiate(scorpionPrefab, enemySpawnPositions[i], Quaternion.identity);
        //         scorpionBehaviour ScorpionScript = enemy.AddComponent<scorpionBehaviour>();
        //         ScorpionStats enemyStats = enemy.AddComponent<ScorpionStats>();
        //         ScorpionScript.projectilePrefab = scorpionProjectilePrefab;
        //         ScorpionScript.shootLogic = shootLogic;
        //         ScorpionScript.player = player;
        //         enemyStats.manager = GameManager;
        //         Rigidbody rb = enemy.AddComponent<Rigidbody>();
        //         rb.useGravity = false;
        //         //ScorpionCollisionManager colManager = enemy.AddComponent<ScorpionCollisionManager>();
        //         //colManager.scorpionStats = enemyStats;
        //         //colManager.shootLogic = shootLogic;
        //         //colManager.player = player;
        //         //colManager.projectilePrefab = scorpionProjectilePrefab;
        //     }
        // }

        GameManager.EnemyCount = enemySpawnPoints.Count;
        //GameManager.EnemyCount = enemyList.GetRange();
    }

    public bool CheckList(int x, int y)
    {
        Vector2 pos = new Vector2(x, y);
        if (enemySpawnPoints.Contains(pos))
        {
            return true;
        }
        return false;
    }

}
