using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public InputAction moveAction;
    [SerializeField] private MapGeneration map;
    [SerializeField] private MovementHandler moveHandler;
    public Vector2 playerInput;
    public bool isMoving = false;

    private void Moving()
    {
        isMoving = true;
    }

    private void OnEnable()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        moveAction.Enable();
        moveAction.started += OnPlayerInput;
        if (moveHandler == null)
            Debug.Log("movementHandler is null");
    }

    private void OnDisable()
    {
        moveAction.Disable();
    }

    private void OnDestroy()
    {
        moveAction.started -= OnPlayerInput;
    }

    private async Task PlayerMove(Vector2 playerInput)
    {
        isMoving = true;
        await moveHandler.AddMoveToQueue(playerInput);
        isMoving = false;
    }

    private void OnPlayerInput(InputAction.CallbackContext context)
    {
        if (isMoving)
            return;
        playerInput = context.ReadValue<Vector2>();

        if (playerInput.x != 0 && playerInput.y != 0)
            playerInput.Set(playerInput.x, 0);
        PlayerMove(playerInput);
    }
}
