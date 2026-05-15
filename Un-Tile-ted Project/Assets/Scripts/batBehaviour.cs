using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class batBehaviour : MonoBehaviour
{
    public MovementHandler player; 
    [SerializeField] public AIMovementHandler movementHandler;
    public EntityMovementHandler entityMovementHandler;
    public float startTime = 0f;
    public float repeatTime = 3f;
    public int[] pos = new int[2];
    private bool isChasing = false;
    public bool IsStunned { get; set; }
    public bool IsChasing 
    { 
        get { return isChasing; } 
        set 
        { 
            isChasing = value;
            // if (isChasing)
            // {
            //     CancelInvoke();
            //     InvokeRepeating("ChasePlayer", startTime, repeatTime);
            //     StartCoroutine(ChasePlayerCoroutine());
            // }
            // else
            // {
            //     CancelInvoke();
            //     InvokeRepeating("MoveEnemy", startTime, repeatTime);
            // }
                
        } 
    }

    void OnEnable()
    {
        // Debug.Log("Im a bat mf");
        
    }
    void Start()
    {
        // InvokeRepeating("MoveEnemy", startTime, repeatTime);
        StartCoroutine(MoveC());
    }

    async Task MoveEnemy()
    {
        int randomDirection = Random.Range(0, 4);
        switch(randomDirection)
        {
            case 0:
                if(movementHandler.VerifyDirection(Vector2.up, pos))
                    await entityMovementHandler.OnMove(Vector2.up);
                break;
            case 1:
                if(movementHandler.VerifyDirection(Vector2.down, pos))
                    await entityMovementHandler.OnMove(Vector2.down);
                break;
            case 2:
                if(movementHandler.VerifyDirection(Vector2.left, pos))
                    await entityMovementHandler.OnMove(Vector2.left);
                break;
            case 3:
                if(movementHandler.VerifyDirection(Vector2.right, pos))
                    await entityMovementHandler.OnMove(Vector2.right);
                break;
        }
    }

    IEnumerator MoveC()
    {
        while (true)
        {
            yield return new WaitForSeconds(repeatTime);
            if (IsStunned)
                continue;
            Task moveTask;
            if (isChasing)
                moveTask = ChasePlayer();
            else
                moveTask = MoveEnemy();

            yield return new WaitUntil(() => moveTask.IsCompleted);
        }
    }

    async Task ChasePlayer()
    {
        Vector2 directionToPlayer = new Vector2 (player.playerPosition[0] - pos[0], player.playerPosition[1] - pos[1]);
        directionToPlayer.Normalize();
        Vector2 direction = Mathf.Abs(directionToPlayer.x) > Mathf.Abs(directionToPlayer.y) ? Vector2.right * Mathf.Sign(directionToPlayer.x) : Vector2.up * Mathf.Sign(directionToPlayer.y);
        if(movementHandler.VerifyDirection(direction, pos))
        {
            // Debug.Log("Moving to" + direction);
            await entityMovementHandler.OnMove(direction);
            // Debug.Log("Bat at " + pos[0] + " ," + pos[1]);
        }
        else
        {
            direction = Mathf.Abs(directionToPlayer.x) < Mathf.Abs(directionToPlayer.y) ? Vector2.right * Mathf.Sign(directionToPlayer.x) : Vector2.up * Mathf.Sign(directionToPlayer.y);
            if (movementHandler.VerifyDirection(direction, pos))
                await entityMovementHandler.OnMove(direction);
        }
    }
}
