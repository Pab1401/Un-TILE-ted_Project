using Unity.VisualScripting;
using UnityEngine;

public class batBehaviour : MonoBehaviour
{
    [SerializeField] public AIMovementHandler movementHandler;
    public EntityMovementHandler entityMovementHandler;
    public float startTime = 0f;
    public float repeatTime = 2f;
    public int[] pos = new int[2];
    void OnEnable()
    {
        Debug.Log("Im a bat mf");
        
    }
    void Start()
    {
        InvokeRepeating("MoveEnemy", startTime, repeatTime);
    }

    void MoveEnemy()
    {
        int randomDirection = Random.Range(0, 4);
        switch(randomDirection)
        {
            case 0:
                if(movementHandler.VerifyDirection(Vector2.up, pos))
                    entityMovementHandler.OnMove(Vector2.up);
                break;
            case 1:
                if(movementHandler.VerifyDirection(Vector2.down, pos))
                    entityMovementHandler.OnMove(Vector2.down);
                break;
            case 2:
                if(movementHandler.VerifyDirection(Vector2.left, pos))
                    entityMovementHandler.OnMove(Vector2.left);
                break;
            case 3:
                if(movementHandler.VerifyDirection(Vector2.right, pos))
                    entityMovementHandler.OnMove(Vector2.right);
                break;
        }
    }
}
