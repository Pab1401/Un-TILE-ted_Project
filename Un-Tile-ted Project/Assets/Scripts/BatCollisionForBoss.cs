using UnityEngine;

public class BatCollisionForBoss : MonoBehaviour
{
    public BatStatsForBoss batStats;

    async void OnTriggerEnter(Collider other)
    {
        // Debug.Log("Collision Detected");
        ITakeDamage damageable = other.gameObject.GetComponent<ITakeDamage>();
        if (damageable != null && other.gameObject.CompareTag("Player"))
        {
            damageable.TakeDamage(batStats.damage);

            PlayerInputHandler inputHandler = other.gameObject.GetComponent<PlayerInputHandler>();
            MovementHandler playerMove = other.gameObject.GetComponentInChildren<MovementHandler>();

            if (inputHandler != null && playerMove != null)
            {
                inputHandler.isMoving = true;
            
                await playerMove.VerifyDirection(-inputHandler.playerInput);
            
                inputHandler.isMoving = false;
            }
        
            batStats.StartCoroutine(batStats.TookDamage());
        }
    }
}
