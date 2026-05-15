using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class SnakeBehaviour : MonoBehaviour
{
    [Header("Dependencies")]
    public MovementHandler player; 
    [SerializeField] public AIMovementHandler movementHandler;
    public EntityMovementHandler entityMovementHandler;
    
    [Header("Settings")]
    public float repeatTime = 3f;
    public int[] pos = new int[2];
    
    private int movesTaken = 0;
    private bool isWaiting = false;

    void Start()
    {
        StartCoroutine(StalkerLoop());
    }

    IEnumerator StalkerLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(repeatTime);
            Task currentTask;
            // Execute 3 moves in a row
            for (movesTaken = 0; movesTaken < 3; movesTaken++)
            {
                // 1. Check if we can attack (Player is 1 tile away)
                if (IsPlayerAdjacent())
                    currentTask = AttackPlayer();
                else
                    currentTask = MoveTowardsPlayer();

                yield return new WaitUntil(() => currentTask.IsCompleted); 
            }
        }
    }

    private bool IsPlayerAdjacent()
    {
        int distX = Mathf.Abs(pos[0] - player.playerPosition[0]);
        int distY = Mathf.Abs(pos[1] - player.playerPosition[1]);
        // Manhattan distance of 1 means they are directly next to the enemy
        return (distX + distY == 1);
    }

    private async Task AttackPlayer()
    {
        Vector2 attackDir = new Vector2(player.playerPosition[0] - pos[0], player.playerPosition[1] - pos[1]);
        
        await entityMovementHandler.OnMove(attackDir);
        await entityMovementHandler.OnMove(-attackDir);
        
        // Debug.Log("Stalker Attacked!");
    }

    private async Task MoveTowardsPlayer()
    {
        Vector2 bestDir = Vector2.zero;
        float bestScore = float.MinValue;

        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

        foreach (Vector2 dir in directions)
        {
            // First, check if the tile is even walkable (not a wall/out of bounds)
            if (movementHandler.VerifyDirection(dir, pos))
            {
                int targetX = pos[0] + (int)dir.x;
                int targetY = pos[1] + (int)dir.y;

                // Calculate distance to player after this move
                float distToPlayer = Vector2.Distance(new Vector2(targetX, targetY), 
                                     new Vector2(player.playerPosition[0], player.playerPosition[1]));

                // SCORING LOGIC
                float score = 0;

                // Preference 1: Getting closer is better
                score -= distToPlayer * 10f; 

                // Preference 2: Is it Forest? (MapGeneration.FOREST = 3)
                if (movementHandler.map.Grid[targetX, targetY].block == 3)
                {
                    score += 50f; // High priority for grass/forest
                }

                if (score > bestScore)
                {
                    bestScore = score;
                    bestDir = dir;
                }
            }
        }

        if (bestDir != Vector2.zero)
        {
            await entityMovementHandler.OnMove(bestDir);
        }
    }
}
