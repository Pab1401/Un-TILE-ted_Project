using UnityEngine;

public class SpriteRotator : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        Transform t = Camera.main.transform;
        gameObject.transform.localRotation = t.localRotation;
    }
}
