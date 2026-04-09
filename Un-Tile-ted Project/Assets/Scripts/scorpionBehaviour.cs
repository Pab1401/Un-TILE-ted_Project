using UnityEngine;

public class scorpionBehaviour : MonoBehaviour
{
    public ShootLogic shootLogic;
    public GameObject projectilePrefab;
    public GameObject player;
    
    public float startTime = 2f;
    public float repeatTime = 8f;

    private float damage = 15f;

    void OnEnable()
    {
        Debug.Log("Im a scorpion mf");
    }

    void Start()
    {
        InvokeRepeating("Shoot", startTime, repeatTime);
    }

    void Shoot()
    {
        
        shootLogic.Shoot(transform.position, player.transform.position, damage, projectilePrefab);
    }

}
