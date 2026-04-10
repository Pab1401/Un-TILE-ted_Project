using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private InputAction moveAction;
    [SerializeField] private MapGeneration map;
    [SerializeField] private MovementHandler moveHandler;
    public Vector2 playerInput;

    private void OnEnable()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        moveAction.started += OnPlayerInput;
        if (moveHandler == null)
            Debug.Log("movementHandler is null");
    }


    private void OnPlayerInput(InputAction.CallbackContext context)
    {
        playerInput = context.ReadValue<Vector2>();

        if (playerInput.x != 0 && playerInput.y != 0)
            playerInput.Set(playerInput.x, 0);
        moveHandler.VerifyDirection(playerInput);
    }
}
