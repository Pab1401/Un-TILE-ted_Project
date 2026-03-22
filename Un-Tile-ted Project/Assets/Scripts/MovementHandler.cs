using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovementHandler : MonoBehaviour
{
    [SerializeField] private GameObject movementTarget;
    [SerializeField] private MapRender renderer;
    public void OnMove(Vector2 direction)
    {
        movementTarget.transform.Translate(direction*renderer.spacing);
    }
}
