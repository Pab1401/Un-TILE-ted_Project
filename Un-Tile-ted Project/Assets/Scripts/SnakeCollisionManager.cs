using UnityEngine;

public class SnakeCollisionManager : MonoBehaviour
{
    public SnakeStats stats;

    async void OnTriggerEnter(Collider other)
    {
        // Debug.Log("Collision Detected");
        ITakeDamage damageable = other.gameObject.GetComponent<ITakeDamage>();
        if (damageable != null && other.gameObject.CompareTag("Player"))
        {
            damageable.TakeDamage(stats.damage);

            PlayerInputHandler inputHandler = other.gameObject.GetComponent<PlayerInputHandler>();
            MovementHandler playerMove = other.gameObject.GetComponentInChildren<MovementHandler>();

            if (inputHandler != null && playerMove != null)
            {
                inputHandler.isMoving = true;
            
                await playerMove.VerifyDirection(-inputHandler.playerInput);
            
                inputHandler.isMoving = false;
            }
        
            stats.StartCoroutine(stats.TookDamage());
    }
    }
    void Start()
    {
        // Debug.Log("Collision Manager Started");
    }
}
