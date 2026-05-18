using System.Collections;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;

public class SnakeStats : MonoBehaviour, ITakeDamage
{
    public SnakeBehaviour behaviour;
    public GameManager manager;
    private float health = 40f;
    public float damage = 14f;
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
        AudioManager.Instance.PlaySnakeHurt();
        // Debug.Log("Snaker took damage, now has: " + health);
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
        isStunned = true;
        // Debug.Log("Taking damage");
        takingDamage = true;
        //behaviour.CancelInvoke();
        yield return new WaitForSeconds(1.5f);
        takingDamage = false;
        isStunned = false;
    }
}
