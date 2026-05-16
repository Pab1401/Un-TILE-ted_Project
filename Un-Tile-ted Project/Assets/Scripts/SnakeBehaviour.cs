using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBehaviour : MonoBehaviour
{
    [Header("Dependencies")]
    public MovementHandler player; 
    [SerializeField] public AIMovementHandler movementHandler;
    public EntityMovementHandler entityMovementHandler;
    
    [Header("Settings")]
    public float repeatTime = 4f; 
    public int[] pos = new int[2];
    
    public bool IsStunned = false; 

    void Start()
    {
        StartCoroutine(ActionLoop());
    }

    IEnumerator ActionLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(repeatTime);

            if (IsStunned)
                continue;

            // --- NEW: Turn Memory ---
            // Tracks where we moved during THIS 3-move burst to prevent pacing back and forth
            List<Vector2Int> visitedThisBurst = new List<Vector2Int>();
            // Add our initial starting tile to the burst memory
            visitedThisBurst.Add(new Vector2Int(pos[0], pos[1]));

            for (int i = 0; i < 3; i++)
            {
                if (IsPlayerAdjacent())
                {
                    Task attackTask = AttackPlayer();
                    yield return new WaitUntil(() => attackTask.IsCompleted);
                    break; 
                }

                // Pass the memory list into the movement processor
                Task moveTask = TakeSmartStep(visitedThisBurst);
                yield return new WaitUntil(() => moveTask.IsCompleted);

                yield return new WaitForSeconds(0.05f);
            }
        }
    }

    private bool IsPlayerAdjacent()
    {
        int distX = Mathf.Abs(pos[0] - player.playerPosition[0]);
        int distY = Mathf.Abs(pos[1] - player.playerPosition[1]);
        return (distX + distY == 1);
    }

    private async Task AttackPlayer()
    {
        Vector2 attackDir = new Vector2(player.playerPosition[0] - pos[0], player.playerPosition[1] - pos[1]);
        await entityMovementHandler.OnMove(attackDir);
        await entityMovementHandler.OnMove(-attackDir);
    }

    private async Task TakeSmartStep(List<Vector2Int> visitedThisBurst)
    {
        Vector2 bestDir = Vector2.zero;
        float bestScore = float.MinValue;

        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        int size = movementHandler.map.Grid.GetLength(0);

        // Calculate current distance to player BEFORE moving
        float currentDistToPlayer = Vector2.Distance(
            new Vector2(pos[0], pos[1]), 
            new Vector2(player.playerPosition[0], player.playerPosition[1])
        );

        foreach (Vector2 dir in directions)
        {
            int targetX = pos[0] + (int)dir.x;
            int targetY = pos[1] + (int)dir.y;

            if (targetX >= 0 && targetX < size && targetY >= 0 && targetY < size)
            {
                if (movementHandler.map.Grid[targetX, targetY].block != 2)
                {
                    float distAfterMove = Vector2.Distance(
                        new Vector2(targetX, targetY), 
                        new Vector2(player.playerPosition[0], player.playerPosition[1])
                    );

                    float score = 0f;

                    // Core Rule: Closer to player is always fundamentally better
                    score -= distAfterMove * 10f; 

                    // FIX 1: Directional Filtering 
                    // Only grant the Grass Bonus (+35) if this tile keeps us closer OR equal distance.
                    // This blocks the snake from moving BACKWARD away from you just to stay on grass.
                    if (distAfterMove <= currentDistToPlayer && movementHandler.map.Grid[targetX, targetY].block == 3)
                    {
                        score += 35f; 
                    }

                    // FIX 2: Anti-Backtracking
                    // If the tile was already stepped on during this 3-move turn, tank its score
                    if (visitedThisBurst.Contains(new Vector2Int(targetX, targetY)))
                    {
                        score -= 150f; 
                    }

                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestDir = dir;
                    }
                }
            }
        }

        if (bestDir != Vector2.zero)
        {
            if (movementHandler.VerifyDirection(bestDir, pos))
            {
                // Record this newly targeted position into our short-term turn memory
                visitedThisBurst.Add(new Vector2Int(pos[0], pos[1]));
                
                await entityMovementHandler.OnMove(bestDir);
            }
        }
    }
}
