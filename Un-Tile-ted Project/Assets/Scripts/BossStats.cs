using UnityEngine;

public class BossStats : MonoBehaviour, ITakeDamage
{
    public GameManager manager;
    public static event System.Action<float> OnBossHealthChanged;
    public static event System.Action OnPhaseChange;
    private float health = 500;
    public float bulletDamage = 20f;
    public float beamDamage = 10f;

    public void TakeDamage(float damage)
    {
        health -= damage;
        OnBossHealthChanged?.Invoke(health);
        if (health <350 && health > 300)
        {
            OnPhaseChange?.Invoke();
        }
        else if (health <= 250)
        {
            OnPhaseChange?.Invoke();
        }
        if (health <= 0)
        {
            manager.IsBossAlive = false;
        }
    }
}
