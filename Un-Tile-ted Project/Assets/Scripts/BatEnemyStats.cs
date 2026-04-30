using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class BatEnemyStats : MonoBehaviour, ITakeDamage
{
    public batBehaviour behaviour;
    public GameManager manager;
    private float health = 50f;
    public float damage = 8f;
    public float timerDecrease = 0.1f;
    private bool takingDamage = false;
    
    public void TakeDamage(float damage)
    {
        if (takingDamage)
            return;
        health -= damage;
        // Debug.Log("Bat took damage, now has: " + health);
        StartCoroutine(TookDamage());
        behaviour.repeatTime -= timerDecrease;
        if (health <= 0)
        {
            manager.EnemyCount--;
            Destroy(gameObject);
        }
    }

    public IEnumerator TookDamage()
    {
        // Debug.Log("Taking damage");
        takingDamage = true;
        behaviour.CancelInvoke();
        yield return new WaitForSeconds(1.5f);
        if (behaviour.IsChasing)
            behaviour.InvokeRepeating("ChasePlayer", behaviour.startTime, behaviour.repeatTime);
        else
            behaviour.InvokeRepeating("MoveEnemy", behaviour.startTime, behaviour.repeatTime);
        takingDamage = false;
    }
}
