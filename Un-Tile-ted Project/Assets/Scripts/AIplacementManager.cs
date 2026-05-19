using System.Collections.Generic;
//using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Experimental.GlobalIllumination;

public class AIplacementManager : MonoBehaviour
{
    [SerializeField] private MapGeneration dataGenerator;
    [SerializeField] private MapRender mapRender;
    [SerializeField] private GameObject batPrefab;
    [SerializeField] public List<Vector2> enemySpawnPoints;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject scorpionPrefab;
    [SerializeField] private ShootLogic shootLogic;

    [SerializeField] private GameObject snakePrefab;
    [SerializeField] private GameObject scorpionProjectilePrefab;
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private GameManager GameManager;

    [SerializeField] private int enemyCount;
    //[SerializeField] private int batCount;
    [SerializeField] private int scorpionCount;
    [SerializeField] private int snakeCount;
    private bool Boss;
    public int middle;

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
        if (dataGenerator.Boss)
        {
            Boss = dataGenerator.Boss;
            GameManager.Boss = dataGenerator.Boss;
        }
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
            if (snakeCount > 0)
            {
                GameObject enemy = Instantiate(snakePrefab, EnSp.SpawnPosition, Quaternion.identity);
                
                SnakeBehaviour behaviour = enemy.GetComponent<SnakeBehaviour>();
                behaviour.pos = new int[] {(int)EnSp.SpawnPoint.x, (int)EnSp.SpawnPoint.y};

                SnakeStats stats = enemy.AddComponent<SnakeStats>();
                stats.behaviour = behaviour;
                stats.manager = GameManager;

                SnakeCollisionManager collider = enemy.AddComponent<SnakeCollisionManager>();
                collider.stats = stats;
                
                behaviour.player = player.GetComponentInChildren<MovementHandler>();
                
                behaviour.movementHandler = enemy.AddComponent<AIMovementHandler>();
                behaviour.movementHandler.map = dataGenerator;
                
                behaviour.entityMovementHandler = enemy.AddComponent<EntityMovementHandler>();
                behaviour.entityMovementHandler.movementTarget = enemy;
                behaviour.entityMovementHandler.r = mapRender;

                snakeCount--;                

            }
            else if (scorpionCount > 0)
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
                GameObject enemy = Instantiate(batPrefab, EnSp.SpawnPosition, Quaternion.identity);
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
        if (Boss)
        {
            GameManager.Boss = true;
            middle = dataGenerator.Grid.GetLength(0) / 2;
            bossPrefab.GetComponent<BossStats>().manager = FindFirstObjectByType<GameManager>();
            bossPrefab.GetComponent<BossBehaviour>().player = player;
            Instantiate(bossPrefab, new Vector3(mapRender.blockPositions[middle, (middle*2)].x, mapRender.blockPositions[middle, (middle*2)].y + 0.75f, mapRender.blockPositions[middle, (middle*2)].z + 3f), Quaternion.identity);
            BoxCollider bossCollider = bossPrefab.GetComponent<BoxCollider>();
            bossCollider.size = new Vector3(middle*2 * 1.2f, 2f, 0.5f);
            GameManager.EnemyCount++; // Increment enemy count for the boss
            // if (bossPrefab.GetComponent<ShootLogic>() == null)
            // {
            //     ShootLogic shooter = bossPrefab.AddComponent<ShootLogic>();
            // }
            
            // mapRender.blockPositions
            
            
        }

        #region OldCode
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
        #endregion

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
