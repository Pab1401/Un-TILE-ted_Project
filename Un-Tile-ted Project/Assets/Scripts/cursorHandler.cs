using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class cursorHandler : MonoBehaviour
{
    public float cursorDistance = 1f;
    [SerializeField] private Transform cursorObject;
    [SerializeField] private ShootLogic shootLogic;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject bulletPrefab;
    private InputAction cursorAction;
    private InputAction cursorClickAction;
    private float smoothSpeed = 2f;
    void OnEnable()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        cursorAction = InputSystem.actions.FindAction("CursorMove");
        cursorAction.performed += OnCursorMove;
        cursorClickAction = InputSystem.actions.FindAction("CursorClick");
        cursorClickAction.started += OnCursorClick;
    }

    private void OnCursorMove(InputAction.CallbackContext context)
    {
        Vector2 cursorInput = context.ReadValue<Vector2>();
        cursorInput = cursorInput.normalized * cursorDistance;
        cursorObject.localPosition = Vector3.Lerp(cursorObject.localPosition, new Vector3(cursorInput.x, cursorInput.y, 0), smoothSpeed);
    }

    private void OnCursorClick(InputAction.CallbackContext context)
    {
        Vector3 targetPosition = new Vector3(cursorObject.transform.localPosition.x, 0, cursorObject.transform.localPosition.y);
        shootLogic.Shoot(player.transform.position, player.transform.position + targetPosition, 10f, bulletPrefab, player);
    }


}
