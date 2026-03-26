using UnityEngine;

public class AIMovementHandler : MonoBehaviour
{
    [SerializeField] public MapGeneration map;
    public bool VerifyDirection(Vector2 direction, int[] enemyPosition)
    {
        int x = (int)direction.x;
        int y = (int)direction.y;

        if (map.Grid[enemyPosition[0] + x, enemyPosition[1] + y] != 2 && map.Grid[enemyPosition[0] + x, enemyPosition[1] + y] != null)
        {
            enemyPosition[0] += x;
            enemyPosition[1] += y;
            Debug.Log("Enemy position: " + enemyPosition[0] + ", " + enemyPosition[1]);
            // Call the movement handler for the enemy
            return true;
        }
        return false;
    }
}
