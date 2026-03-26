using UnityEngine;

public class MovementHandler : MonoBehaviour
{
    [SerializeField] private MapGeneration map;
    [SerializeField] private EntityMovementHandler playerMovementHandler;
    private int[] playerPosition = new int[2];

    public void SetPlayerPosition()
    {
        playerPosition = new int[] { map.spawnPos.x, map.spawnPos.y };
    }
    public void VerifyDirection(Vector2 input)
    {
        int x = (int)input.x;
        int y = (int)input.y;

        if (map.Grid[playerPosition[0] + x, playerPosition[1] + y] != 2 && map.Grid[playerPosition[0] + x, playerPosition[1] + y] != null)
        {
            playerPosition[0] += x;
            playerPosition[1] += y;
            Debug.Log("Player position: " + playerPosition[0] + ", " + playerPosition[1]);
            playerMovementHandler.OnMove(input);
        }
    }



}
