using System.Numerics;
using UnityEngine;

public class CreateBeam : MonoBehaviour
{
    void Start()
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>() ? gameObject.GetComponent<Rigidbody>() : gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.AddTorque(UnityEngine.Vector3.right, ForceMode.Force);
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
        lr.SetPosition(0, gameObject.transform.position + (UnityEngine.Vector3.back * 1));
        lr.SetPosition(1, gameObject.transform.position + (UnityEngine.Vector3.back * 0.5f) * 80);
        if (Physics.Raycast(gameObject.transform.position + (UnityEngine.Vector3.back * 1), gameObject.transform.position + (UnityEngine.Vector3.back * 0.5f), out hit, 80f))
        {
            Debug.Log("Object hit: " + hit.collider.name);
            Debug.DrawLine(gameObject.transform.position, hit.point, Color.red);
            if (hit.collider.gameObject.tag == "Player")
            {
                Debug.Log("Object hit: " + hit.collider.name);
                hit.collider.gameObject.GetComponent<PlayerStatus>().TakeDamage(damage);
            }
        }
        else
        {
             Debug.DrawLine(gameObject.transform.position, gameObject.transform.position + (UnityEngine.Vector3.back * 0.5f) * 80, Color.green);
        }
        GameObject.Destroy(gameObject, 0.5f);
        
    }

}