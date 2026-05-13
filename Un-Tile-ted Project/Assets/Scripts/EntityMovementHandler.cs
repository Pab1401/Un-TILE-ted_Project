using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using System.Threading.Tasks;

public class EntityMovementHandler : MonoBehaviour
{
    [SerializeField] public GameObject movementTarget;
    [SerializeField] public MapRender r;
    public float speed = 5f;
    public async Task OnMove(Vector2 direction)
    {
        Vector3 finalPosition = movementTarget.transform.position + new Vector3(direction.x * r.spacing, 0, direction.y * r.spacing);

        // We "await" the movement task here
        await MoveAsync(finalPosition);

        // Once the line above is done, we return true
        return;
    }

    async Task MoveAsync(Vector3 pos)
    {
        while (movementTarget.transform.position != pos)
        {
            float step = speed * Time.deltaTime;
            movementTarget.transform.position = Vector3.MoveTowards(movementTarget.transform.position, pos, step);

            await Task.Yield();
        }
        return;
    }
}
