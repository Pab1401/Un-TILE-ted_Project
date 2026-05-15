using UnityEngine;

public class ScorpionCollisionManager : MonoBehaviour
{
    public ScorpionStats stats;

    async void OnTriggerEnter(Collider other)
    {
        ITakeDamage damageable = other.gameObject.GetComponent<ITakeDamage>();
        if (damageable == null || !other.gameObject.CompareTag("Player"))
            return;

        damageable.TakeDamage(stats.CollisionDamage);
    
        PlayerInputHandler inputHandler = other.gameObject.GetComponent<PlayerInputHandler>();
        MovementHandler playerMove = other.gameObject.GetComponentInChildren<MovementHandler>();

        if (inputHandler != null && playerMove != null)
        {
            inputHandler.isMoving = true;
            await playerMove.VerifyDirection(-inputHandler.playerInput);
            inputHandler.isMoving = false;
        }
    }
}
