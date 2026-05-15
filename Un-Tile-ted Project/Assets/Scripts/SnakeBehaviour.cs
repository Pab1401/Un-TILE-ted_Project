using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class SnakeBehaviour : MonoBehaviour
{
    [Header("Dependencies")]
    public MovementHandler player; 
    public AIMovementHandler movementHandler;
    public EntityMovementHandler entityMovementHandler;
    
    [Header("Settings")]
    public float repeatTime = 4f; // Downtime length after completing actions
    public int[] pos = new int[2];
    
    private bool isStunned = false; // Match the bat state architecture if needed

    void Start()
    {
        StartCoroutine(ActionLoop());
    }

    IEnumerator ActionLoop()
    {
        while (true)
        {
            // 1. Trigger the waiting period (downtime)
            yield return new WaitForSeconds(repeatTime);

            if (isStunned)
                continue;

            // 2. Queue and execute a burst of 3 individual, tile-by-tile moves
            for (int i = 0; i < 3; i++)
            {
                // Prior to moving each tile, check if player is close enough to strike
                if (IsPlayerAdjacent())
                {
                    Task attackTask = AttackPlayer();
                    yield return new WaitUntil(() => attackTask.IsCompleted);
                    
                    // Instantly trigger the waiting period by breaking out of the 3-move loop
                    break; 
                }

                // If not attacking, process exactly one smart tile step
                Task moveTask = TakeSmartStep();
                
                // We yield until the single tile movement is completely finished
                yield return new WaitUntil(() => moveTask.IsCompleted);

                // Optional: Very slight delay between tiles (e.g. 0.05s) if you want them 
                // to pause for a split second between grid steps, otherwise they fluidly roll into the next tile.
                yield return new WaitForSeconds(0.05f);
            }
        }
    }

    private bool IsPlayerAdjacent()
    {
        int distX = Mathf.Abs(pos[0] - player.playerPosition[0]);
        int distY = Mathf.Abs(pos[1] - player.playerPosition[1]);
        // Adjacent on a grid means a combined distance of exactly 1 tile
        return (distX + distY == 1);
    }

    private async Task AttackPlayer()
    {
        // Direction vector pushing towards player
        Vector2 attackDir = new Vector2(player.playerPosition[0] - pos[0], player.playerPosition[1] - pos[1]);
        
        // Bump animation: moves 1 tile towards player, then 1 tile back
        await entityMovementHandler.OnMove(attackDir);
        await entityMovementHandler.OnMove(-attackDir);
        
        Debug.Log("Snake Attacked the Player!");
    }

    private async Task TakeSmartStep()
{
    Vector2 bestDir = Vector2.zero;
    float bestScore = float.MinValue;

    // Cardinally available step options
    Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    foreach (Vector2 dir in directions)
    {
        // Use your existing AIMovementHandler boundary/wall validations from current position
        if (movementHandler.VerifyDirection(dir, pos))
        {
            int targetX = pos[0] + (int)dir.x;
            int targetY = pos[1] + (int)dir.y;

            // Calculate distance to player if we step here
            float distToPlayer = Vector2.Distance(
                new Vector2(targetX, targetY), 
                new Vector2(player.playerPosition[0], player.playerPosition[1])
            );

            float score = 0f;

            // Rule 1: Moving closer to the player increases score
            score += distToPlayer * 10f; 

            // Rule 2: Prefer Forest / Grass tiles (MapGeneration.FOREST = 3)
            if (movementHandler.map.Grid[targetX, targetY].block == 3)
            {
                score += 45f; // Weighted highly to stay hidden/stick to grass
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
        // --- FIX HERE: Instantly update the grid tracker positions ---
        // This ensures step #2 evaluates options relative to where step #1 just moved!
        pos[0] += (int)bestDir.x;
        pos[1] += (int)bestDir.y;

        // Perform the physical movement interpolation one tile forward
        await entityMovementHandler.OnMove(bestDir);
    }
}
}
