using System.Collections;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    private batBehaviour bat;
    void Start()
    {
        bat = GetComponentInParent<batBehaviour>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if(bat.IsChasing)
                return;
            if (bat.player == null)
                bat.player = other.gameObject.GetComponentInChildren<MovementHandler>();
            StopCoroutine("PlayerLost");
            bat.IsChasing = true;
            // Debug.Log("Player detected");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (bat.IsChasing)
        {
            // Debug.Log("Player left detection");
            StartCoroutine("PlayerLost");               
        }
    }

    IEnumerator PlayerLost()
    {
        yield return new WaitForSeconds(6f);
        bat.IsChasing = false;
        // Debug.Log("Player lost");
    }

}
