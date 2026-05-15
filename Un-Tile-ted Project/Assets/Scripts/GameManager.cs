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

    public string nextScene;
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
        // CAMBIO RESPECTO AL ORIGINAL: Se agregaron condicionales "if" antes de desactivar los menús.
        // Esto evita que Unity arroje un error (NullReferenceException) si usas este script en
        // una escena que no tenga menú de pausa o de victoria (como en un menú principal).
        if (levelFailedMenu) levelFailedMenu.gameObject.SetActive(false);
        if (levelClearedMenu) levelClearedMenu.gameObject.SetActive(false);
        if (pauseMenu) pauseMenu.gameObject.SetActive(false);

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

        // CAMBIO RESPECTO AL ORIGINAL: Aquí nos comunicamos con el nuevo sistema de audio.
        // Le avisamos al AudioManager (si es que existe en la escena) que el juego se pausó
        // o se reanudó, para que él se encargue de cambiar la música sin ensuciar este script.
        if (AudioManager.Instance != null) AudioManager.Instance.TogglePauseMusic(isPaused);

        if (isPaused)
        {
            playerInput.moveAction.Disable();
            cursor.cursorAction.Disable();
            cursor.cursorClickAction.Disable();

            // CAMBIO RESPECTO AL ORIGINAL: Validación de seguridad con "if"
            if (pauseMenu) pauseMenu.gameObject.SetActive(true);
        }
        else
        {
            playerInput.moveAction.Enable();
            cursor.cursorAction.Enable();
            cursor.cursorClickAction.Enable();

            // CAMBIO RESPECTO AL ORIGINAL: Validación de seguridad con "if"
            if (pauseMenu) pauseMenu.gameObject.SetActive(false);
        }
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        Pause();
    }

    public void GameEnd()
    {
        // CAMBIO RESPECTO AL ORIGINAL: Al morir, le decimos al AudioManager que detenga 
        // todas las canciones (Intro, Loop y Pausa) para que haya silencio en la pantalla de Game Over.
        if (AudioManager.Instance != null) AudioManager.Instance.StopAllMusic();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        playerInput.moveAction.Disable();
        cursor.cursorAction.Disable();
        cursor.cursorClickAction.Disable();

        // CAMBIO RESPECTO AL ORIGINAL: Validación de seguridad con "if"
        if (levelFailedMenu) levelFailedMenu.gameObject.SetActive(true);
    }

    public void GameWin()
    {
        // CAMBIO RESPECTO AL ORIGINAL: Al ganar, igual que al morir, detenemos la música
        // para dar paso a la pantalla de victoria y evitar que el audio siga sonando de fondo.
        if (AudioManager.Instance != null) AudioManager.Instance.StopAllMusic();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        playerInput.moveAction.Disable();
        cursor.cursorAction.Disable();
        cursor.cursorClickAction.Disable();

        // CAMBIO RESPECTO AL ORIGINAL: Validación de seguridad con "if"
        if (levelClearedMenu) levelClearedMenu.gameObject.SetActive(true);
    }

    public void NextLevel()
    {
        // ESTA SECCIÓN SE MANTIENE EXACTAMENTE IGUAL AL ORIGINAL
        PlayerStatus playerStats = FindFirstObjectByType<PlayerStatus>();
        Vault.Instance.SaveStats(playerStats.Health, playerStats.MaxHealth, playerStats.PlayerDamage, playerStats.bulletBounce, playerStats.maxBullets, playerStats.reloadTime);
        SceneManager.LoadScene(nextScene);
    }

    public void LevelRestart()
    {
        // ESTA SECCIÓN SE MANTIENE EXACTAMENTE IGUAL AL ORIGINAL
        SceneManager.LoadScene("MainMenu");
    }
}