using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AIplacementManager : MonoBehaviour
{
    [SerializeField] private MapGeneration dataGenerator;
    [SerializeField] private MapRender mapRender;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] public List<Vector2> enemySpawnPoints;
    private List<Vector3> enemySpawnPositions = new List<Vector3>();
    
    public void AddSpawnPosition(Vector3 position)
    {
        enemySpawnPositions.Add(position);
    }

    public void SpawnEnemies()
    {
        for (int i = 0; i < enemySpawnPoints.Count; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, enemySpawnPositions[i], Quaternion.identity);
            batBehaviour batScript = enemy.GetComponent<batBehaviour>();
            batScript.movementHandler = enemy.AddComponent<AIMovementHandler>();
            batScript.entityMovementHandler = enemy.AddComponent<EntityMovementHandler>();
            batScript.movementHandler.map = dataGenerator;
            batScript.entityMovementHandler.movementTarget = enemy;
            batScript.entityMovementHandler.r = mapRender;
            batScript.pos = new int[] { (int)enemySpawnPoints[i].x, (int)enemySpawnPoints[i].y };
        }
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
