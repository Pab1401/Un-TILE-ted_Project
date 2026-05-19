using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    public static event Action<float> OnFireBeams;

    public GameObject player;
    public GameObject bulletPrefab;
    public BossStats Stats;
    public int phase = 0;
    public float actionDelay = 1f;
    public int enemyCount;
    [SerializeField] private int maxEnemies;
    [SerializeField] private int repetitions = 3;
    private int GridMiddle;
    private BossEnemyInstancer enemyMaker;
    private BossShooter shooter; 
    
    public int delay = 100;
    private bool shouldPhaseChange = false;
    void OnEnable()
    {
        BossStats.OnPhaseChange += () => shouldPhaseChange = true;
    }
    void OnDisable()
    {
        BossStats.OnPhaseChange -= () => shouldPhaseChange = false;
    }
    void Start()
    {
        enemyMaker = GetComponentInChildren<BossEnemyInstancer>();
        shooter = GetComponentInChildren<BossShooter>();
        GridMiddle = FindFirstObjectByType<AIplacementManager>().middle;
        StartCoroutine(Behaviour());
    }

    async Task ShootPlayer()
    {
        for (int i = 0; i < repetitions;i++)
        {
            await shooter.Shoot(bulletPrefab, GridMiddle, player.transform.position);
            await Task.Delay(delay);
        }
    }

    async Task SpawnMinions()
    {
        await enemyMaker.MakeEnemies(player);
    }

    async Task ShootBeams()
    {
        for (int i = 0; i < repetitions;i++)
        {
            await shooter.Beams(GridMiddle);
            await Task.Delay(delay);
        }
        OnFireBeams?.Invoke(Stats.beamDamage);
    }

    IEnumerator Behaviour()
    {
        while (true)
        {
            if (shouldPhaseChange)
            {
                phase++;
                shouldPhaseChange = false;
            }
            yield return new WaitForSeconds(actionDelay);
            Task actionTask;
            switch(phase)
            {
                case 0:
                    actionTask = ShootPlayer();
                    yield return new WaitUntil(() => actionTask.IsCompleted);
                    break;
                case 1:
                    actionTask = ShootBeams();
                    yield return new WaitUntil(() => actionTask.IsCompleted);
                    break;
                case 2:
                    actionTask = ShootPlayer();
                    yield return new WaitUntil(() => actionTask.IsCompleted);
                    actionTask = ShootBeams();
                    yield return new WaitUntil(() => actionTask.IsCompleted);
                    break;
            }
            if (enemyCount < maxEnemies)
            {
                actionTask = SpawnMinions();
                yield return new WaitUntil(() => actionTask.IsCompleted);
                enemyCount++;
            }
            
        }
    }

}