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
    private bool boss;
    public bool Boss
    {
        get {return boss; }
        set 
        {
            boss = value;
            if (boss)
            {
                isBossAlive = true;
            }
        }
    }

    private bool isBossAlive;
    public bool IsBossAlive
    {
        get {return isBossAlive;}
        set
        {
            isBossAlive = value;
            if (!isBossAlive && boss)
            {
                GameWin();
            }
        }
    }

    public string nextScene;
    private int enemyCount;

    public int EnemyCount
    {
        get { return enemyCount; }
        set
        {
            enemyCount = value;
            if (enemyCount <= 0 && !Boss)
                GameWin();
        }
    }

    private InputAction pauseAction;
    private bool isPaused;

    void OnEnable()
    {
        // Inicializaci�n de Men�s
        if (levelFailedMenu) levelFailedMenu.gameObject.SetActive(false);
        if (levelClearedMenu) levelClearedMenu.gameObject.SetActive(false);
        if (pauseMenu) pauseMenu.gameObject.SetActive(false);

        isPaused = false;

        // Configuraci�n del Input de Pausa
        pauseAction = InputSystem.actions.FindAction("Pause");
        if (pauseAction != null) pauseAction.performed += OnPause;
    }

    void OnDisable()
    {
        if (pauseAction != null) pauseAction.performed -= OnPause;
    }

    public void Pause()
    {
        isPaused = !isPaused;

        // Control del tiempo y cursor
        Time.timeScale = isPaused ? 0f : 1f;
        UnityEngine.Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        UnityEngine.Cursor.visible = isPaused;

        // --- COMUNICACI�N CON EL AUDIO MANAGER ---
        // Aqu� es donde le avisamos al AudioManager que cambie a la m�sica de pausa
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.TogglePauseMusic(isPaused);
        }

        // Control de Inputs y Men� Visual
        if (isPaused)
        {
            playerInput.moveAction.Disable();
            cursor.cursorAction.Disable();
            cursor.cursorClickAction.Disable();
            if (pauseMenu) pauseMenu.gameObject.SetActive(true);
        }
        else
        {
            playerInput.moveAction.Enable();
            cursor.cursorAction.Enable();
            cursor.cursorClickAction.Enable();
            if (pauseMenu) pauseMenu.gameObject.SetActive(false);
        }
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        Pause();
    }

    public void GameEnd()
    {
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        playerInput.moveAction.Disable();
        cursor.cursorAction.Disable();
        cursor.cursorClickAction.Disable();
        if (levelFailedMenu) levelFailedMenu.gameObject.SetActive(true);
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayGameOver();
        }
    }

    public void GameWin()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        playerInput.moveAction.Disable();
        cursor.cursorAction.Disable();
        cursor.cursorClickAction.Disable();
        if (levelClearedMenu) levelClearedMenu.gameObject.SetActive(true);
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayLevelComplete();
        }
    }

    public void NextLevel()
    {
        // Guardar estad�sticas antes de cambiar de escena
        PlayerStatus playerStats = FindFirstObjectByType<PlayerStatus>();
        if (playerStats != null && Vault.Instance != null)
        {
            Vault.Instance.SaveStats(playerStats.Health, playerStats.MaxHealth, playerStats.PlayerDamage,
                                   playerStats.bulletBounce, playerStats.maxBullets, playerStats.reloadTime);
        }

        Time.timeScale = 1f; // Aseguramos que el tiempo corra en la nueva escena
        SceneManager.LoadScene(nextScene);
    }

    public void LevelRestart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}