using UnityEngine;

public class BatCollisionManager : MonoBehaviour
{
    public BatEnemyStats batStats;

    void OnTriggerEnter(Collider other)
    {
        // Debug.Log("Collision Detected");
        ITakeDamage damageable = other.gameObject.GetComponent<ITakeDamage>();
        if (damageable != null)
            if (other.gameObject.CompareTag("Player"))
            {
                damageable.TakeDamage(batStats.damage);
                Vector2 playerInput = other.gameObject.GetComponent<PlayerInputHandler>().playerInput;
                MovementHandler playerMove = other.gameObject.GetComponentInChildren<MovementHandler>();
                if (playerMove == null)
                    Debug.Log("PlayerMove is null");
                playerMove.VerifyDirection(-playerInput);
                batStats.StartCoroutine(batStats.TookDamage());
            }
    }
    void Start()
    {
        Debug.Log("Collision Manager Started");
    }
}
