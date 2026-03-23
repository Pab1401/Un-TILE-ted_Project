using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovementHandler : MonoBehaviour
{
    [SerializeField] private GameObject movementTarget;
    [SerializeField] private MapRender renderer;
    public void OnMove(Vector2 direction)
    {
        Debug.Log("Direction: " + direction);
        movementTarget.transform.Translate(direction.x*renderer.spacing, 0, direction.y*renderer.spacing);
    }
}
