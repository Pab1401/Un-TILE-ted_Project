using Unity.VectorGraphics;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerInputHandler playerInput;
    [SerializeField] private cursorHandler cursor;
    [SerializeField] UnityEngine.UI.Image pauseMenu;
    [SerializeField] UnityEngine.UI.Image levelClearedMenu;
    [SerializeField] UnityEngine.UI.Image levelFailedMenu;
    [SerializeField] SceneAsset nextScene;
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
        levelFailedMenu.gameObject.SetActive(false);
        levelClearedMenu.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(false);
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
            pauseMenu.gameObject.SetActive(true);
        }
        else
        {
            playerInput.moveAction.Enable();
            cursor.cursorAction.Enable();
            cursor.cursorClickAction.Enable();
            pauseMenu.gameObject.SetActive(false);
        }
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        Pause();
    }
    public void GameEnd()
    {
        Cursor.lockState = CursorLockMode.None;
        playerInput.moveAction.Disable();
        cursor.cursorAction.Disable();
        cursor.cursorClickAction.Disable();
        levelFailedMenu.gameObject.SetActive(true);
    }

    public void GameWin()
    {
        Cursor.lockState= CursorLockMode.None;
        playerInput.moveAction.Disable();
        cursor.cursorAction.Disable();
        cursor.cursorClickAction.Disable();
        levelClearedMenu.gameObject.SetActive(true);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(nextScene.name);
    }

    public void LevelRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
