using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerInputHandler playerInput;
    [SerializeField] private cursorHandler cursor;
    [SerializeField] Canvas canvas;
    private int enemyCount;
    public int EnemyCount
    {
        get { return enemyCount; }
        set
        {
            enemyCount = value;
            if (enemyCount == 0)
                GameWin();
        }
    }
    private InputAction pauseAction;
    bool isPaused;

    void OnEnable()
    {
        canvas.enabled = false;
        isPaused = false;
        pauseAction = InputSystem.actions.FindAction("Pause");
        pauseAction.performed += OnPause;
    }

    public void Pause()
    {
        isPaused = !isPaused;
        UnityEngine.Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        UnityEngine.Cursor.visible = isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        if (isPaused)
        {
            playerInput.moveAction.Disable();
            cursor.cursorAction.Disable();
            cursor.cursorClickAction.Disable();
            canvas.enabled = true;
        }
        else
        {
            playerInput.moveAction.Enable();
            cursor.cursorAction.Enable();
            cursor.cursorClickAction.Enable();
            canvas.enabled = false;
        }
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        Pause();
    }
    public void GameEnd()
    {
        Debug.Log("Game Over");
    }

    public void GameWin()
    {
        Debug.Log("You win");
    }
}
