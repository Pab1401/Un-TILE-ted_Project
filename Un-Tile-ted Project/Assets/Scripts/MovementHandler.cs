using System;
using UnityEngine;

public class MovementHandler : MonoBehaviour
{
    [SerializeField] private MapGeneration map;
    [SerializeField] private EntityMovementHandler playerMovementHandler;
    public int[] playerPosition = new int[2];

    public void SetPlayerPosition()
    {
        playerPosition = new int[] { map.spawnPos.x, map.spawnPos.y };
    }
    public void VerifyDirection(Vector2 input)
    {
        int x = (int)input.x;
        int y = (int)input.y;
        try
        {
            // if (map.Grid[playerPosition[0] + x, playerPosition[1] + y].block != 2 && playerPosition[0] + x <= map.Grid.GetLength(1) && playerPosition[1] + y <= map.Grid.GetLength(0) && map.Grid[playerPosition[0] + x, playerPosition[1] + y].taken == false)
            if (map.Grid[playerPosition[0] + x, playerPosition[1] + y].block != 2 && playerPosition[0] + x <= map.Grid.GetLength(1) && playerPosition[1] + y <= map.Grid.GetLength(0))
            {
                // map.Grid[playerPosition[0], playerPosition[1]].taken = false;
                playerPosition[0] += x;
                playerPosition[1] += y;
                // map.Grid[playerPosition[0], playerPosition[1]].taken = true;
                //Debug.Log("Player position: " + playerPosition[0] + ", " + playerPosition[1]);
                playerMovementHandler.OnMove(input);
            }
        }
        catch (IndexOutOfRangeException)
        {
            //Debug.Log(ex.Message);
        }
    }



}
