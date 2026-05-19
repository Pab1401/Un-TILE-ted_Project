using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BossShooter : MonoBehaviour
{
    public GameObject beamPrefab;
    public Vector3 shootingPoint;
    public ShootLogic shootL;
    private BossStats stats;
    private List<int> positions = new List<int>();
    void Start()
    {
        stats = GetComponentInParent<BossStats>();
        shootingPoint = GetComponentInParent<Transform>().position + (Vector3.back*1.5f);
        Debug.Log(shootingPoint);
    }
    public async Task Beams(int middle)
    {
        // Debug.Log("beams");
        int rand = Random.Range(-middle, middle);
        if (positions.Contains(rand))
            return;
        positions.Add(rand);
        Instantiate(beamPrefab, shootingPoint + (Vector3.right * rand), Quaternion.identity);
        await Task.Delay(1500);
    }

    public async Task Shoot(GameObject bullet, int middle, Vector3 player)
    {
        int rand = Random.Range(-middle, middle);
        // Debug.Log("shootin from " + (shootingPoint + (Vector3.right * rand)) + " to " + player);
        // shoot.Shoot(shootingPoint + (Vector3.right * rand), shootingPoint + (Vector3.right * rand) + Vector3.back, stats.bulletDamage, bullet, gameObject.GetComponentInParent<GameObject>());
        shootL.Shoot(shootingPoint + (Vector3.right * rand), player, stats.bulletDamage, bullet, gameObject);
        await Task.Delay(1000);
    }
}   