using System;
using UnityEngine;
using System.Threading.Tasks;

public class MovementHandler : MonoBehaviour
{
    [SerializeField] private MapGeneration map;
    [SerializeField] private EntityMovementHandler playerMovementHandler;
    public int[] playerPosition = new int[2];

    private Task movementQueue = Task.CompletedTask;

    public void SetPlayerPosition()
    {
        playerPosition = new int[] { map.spawnPos.x, map.spawnPos.y };
    }

    public async Task AddMoveToQueue(Vector2 Input)
    {
        Task previousMove = movementQueue;
    
        Task currentMove = ProcessQueueMove(Input, previousMove);
        movementQueue = currentMove;
    
        await currentMove;
    }

    private async Task ProcessQueueMove(Vector2 Input, Task previousMove)
    {
    // Wait for the move that was ahead of us in line to finish
    await previousMove;

    int x = (int)Input.x;
    int y = (int)Input.y;
    
    try
    {
        // Boundary and block checks
        if (playerPosition[0] + x >= 0 && playerPosition[0] + x < map.Grid.GetLength(0) &&
            playerPosition[1] + y >= 0 && playerPosition[1] + y < map.Grid.GetLength(1))
        {
            if (map.Grid[playerPosition[0] + x, playerPosition[1] + y].block != 2)
            {
                playerPosition[0] += x;
                playerPosition[1] += y;
                await playerMovementHandler.OnMove(Input);
            }
        }
    }
    catch (IndexOutOfRangeException) { }
    }
    public async Task VerifyDirection(Vector2 input)
    {
        await AddMoveToQueue(input);
    }



}
