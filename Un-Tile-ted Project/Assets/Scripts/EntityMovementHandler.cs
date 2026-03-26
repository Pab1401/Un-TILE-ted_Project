using Unity.VisualScripting;
using UnityEngine;

public class EntityMovementHandler : MonoBehaviour
{
    [SerializeField] public GameObject movementTarget;
    [SerializeField] public MapRender r;
    public void OnMove(Vector2 direction)
    {
        Debug.Log("Direction: " + direction);
        movementTarget.transform.Translate(direction.x*r.spacing, 0, direction.y*r.spacing);
    }
}
