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
    [Range(0, 1)]
    public int enemyDecision = 1;
    private List<Vector3> enemySpawnPositions = new List<Vector3>();
    
    public void AddSpawnPosition(Vector3 position)  
    {
        enemySpawnPositions.Add(position);
    }

    public void SpawnEnemies()
    {
        if (enemyDecision == 0)
        {
            for (int i = 0; i < enemySpawnPoints.Count; i++)
            {
                Debug.Log("Spawning Bat");
                GameObject enemy = Instantiate(enemyPrefab, enemySpawnPositions[i], Quaternion.identity);
                batBehaviour batScript = enemy.GetComponent<batBehaviour>();
                batScript.movementHandler = enemy.AddComponent<AIMovementHandler>();
                batScript.entityMovementHandler = enemy.AddComponent<EntityMovementHandler>();
                batScript.movementHandler.map = dataGenerator;
                batScript.entityMovementHandler.movementTarget = enemy;
                batScript.entityMovementHandler.r = mapRender;
                batScript.pos = new int[] { (int)enemySpawnPoints[i].x, (int)enemySpawnPoints[i].y };
                BatEnemyStats enemyStats = enemy.AddComponent<BatEnemyStats>();
                BatCollisionManager colManager = enemy.AddComponent<BatCollisionManager>();
                colManager.batStats = enemyStats;
                enemyStats.manager = GameManager;
            }
        }
        else if (enemyDecision == 1)
        {
            Debug.Log("Spawning Scorpion");
            for (int i = 0; i < enemySpawnPoints.Count; i++)
            {
                GameObject enemy = Instantiate(scorpionPrefab, enemySpawnPositions[i], Quaternion.identity);
                scorpionBehaviour ScorpionScript = enemy.AddComponent<scorpionBehaviour>();
                ScorpionStats enemyStats = enemy.AddComponent<ScorpionStats>();
                ScorpionScript.projectilePrefab = scorpionProjectilePrefab;
                ScorpionScript.shootLogic = shootLogic;
                ScorpionScript.player = player;
                enemyStats.manager = GameManager;
                Rigidbody rb = enemy.AddComponent<Rigidbody>();
                rb.useGravity = false;
                //ScorpionCollisionManager colManager = enemy.AddComponent<ScorpionCollisionManager>();
                //colManager.scorpionStats = enemyStats;
                //colManager.shootLogic = shootLogic;
                //colManager.player = player;
                //colManager.projectilePrefab = scorpionProjectilePrefab;
            }
        }
        GameManager.EnemyCount = enemySpawnPoints.Count;
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
