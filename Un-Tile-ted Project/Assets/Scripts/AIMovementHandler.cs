using System;
using UnityEngine;

public class AIMovementHandler : MonoBehaviour
{
    [SerializeField] public MapGeneration map;
    public bool VerifyDirection(Vector2 direction, int[] enemyPosition)
    {
        int x = (int)direction.x;
        int y = (int)direction.y;
        try
        {
            if (map.Grid[enemyPosition[0] + x, enemyPosition[1] + y] != 2 && (enemyPosition[0] + x <= map.Grid.GetLength(1) - 1 || enemyPosition[1] + y <= map.Grid.GetLength(0) - 1))
            {
                enemyPosition[0] += x;
                enemyPosition[1] += y;
                //Debug.Log("Enemy position: " + enemyPosition[0] + ", " + enemyPosition[1]);
                // Call the movement handler for the enemy
                return true;
            }
            return false;
        }
        catch(IndexOutOfRangeException ex)
        {
            return false;
        }
        
    }
}
