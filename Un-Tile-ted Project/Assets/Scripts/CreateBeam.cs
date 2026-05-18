using System.Numerics;
using UnityEngine;

public class CreateBeam : MonoBehaviour
{
    void Start()
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>() ? gameObject.GetComponent<Rigidbody>() : gameObject.AddComponent<Rigidbody>();
        rb.AddTorque(UnityEngine.Vector3.forward, ForceMode.Force);
    }
    private void OnEnable()
    {
        // Subscribe to the event
        BossBehaviour.OnFireBeams += FireBeam;
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks or errors when the object is destroyed
        BossBehaviour.OnFireBeams -= FireBeam;
    }

    void FireBeam(float damage)
    {
        RaycastHit hit;
        LineRenderer lr = gameObject.GetComponent<LineRenderer>() ? gameObject.GetComponent<LineRenderer>() : gameObject.AddComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.SetPosition(0, gameObject.transform.position + (UnityEngine.Vector3.back * 0.5f));
        lr.SetPosition(1, gameObject.transform.position + (UnityEngine.Vector3.forward * 0.5f));
        if (Physics.Raycast(gameObject.transform.position + (UnityEngine.Vector3.back * 0.5f), gameObject.transform.position + (UnityEngine.Vector3.forward * 0.5f), out hit, 80f))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                hit.collider.gameObject.GetComponent<PlayerStatus>().TakeDamage(damage);
            }
        }
        GameObject.Destroy(gameObject, 0.5f);
        
    }

}
