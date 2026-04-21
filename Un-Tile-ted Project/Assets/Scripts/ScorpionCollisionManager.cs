using UnityEngine;

public class ScorpionCollisionManager : MonoBehaviour
{
    public ScorpionStats stats;

    void OnTriggerEnter(Collider other)
    {
        ITakeDamage damageable = other.gameObject.GetComponent<ITakeDamage>();
        if (other.GetComponent<ITakeDamage>() == null || other.gameObject.tag != "Player")
            return;
        damageable.TakeDamage(stats.CollisionDamage);
        Vector2 playerInput = other.gameObject.GetComponent<PlayerInputHandler>().playerInput;
                MovementHandler playerMove = other.gameObject.GetComponentInChildren<MovementHandler>();
                if (playerMove == null)
                    Debug.Log("PlayerMove is null");
                playerMove.VerifyDirection(-playerInput);
    }
}
