using System.Collections;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;

public class BatStatsForBoss : MonoBehaviour, ITakeDamage
{
    public batBehaviour behaviour;
    public BossBehaviour manager;
    private float health = 50f;
    public float damage = 8f;
    public float timerDecrease = 0.1f;
    private bool takingDamage = false;

    public bool isStunned
    {
        get { return behaviour.IsStunned; }
        set { behaviour.IsStunned = value; }
    }
    
    public void TakeDamage(float damage)
    {
        if (isStunned)
            return;
        if (takingDamage)
            return;
        health -= damage;
        AudioManager.Instance.PlayBatHurt();
        // Debug.Log("Bat took damage, now has: " + health);
        StartCoroutine(TookDamage());
        behaviour.repeatTime -= timerDecrease;
        if (health <= 0)
        {
            manager.enemyCount--;
            Destroy(gameObject);
        }
    }

    public IEnumerator TookDamage()
    {
        isStunned = true;
        // Debug.Log("Taking damage");
        takingDamage = true;
        //behaviour.CancelInvoke();
        yield return new WaitForSeconds(1.5f);
        takingDamage = false;
        isStunned = false;
    }
}
