using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

[System.Serializable]
public struct MusicTrack
{
    public AudioClip intro;
    public AudioClip loop;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Reproductores de Música")]
    [SerializeField] private AudioSource introSource;
    [SerializeField] private AudioSource loopSource;
    [SerializeField] private AudioSource pauseSource;

    [Header("Reproductor de SFX (Efectos)")]
    [SerializeField] private AudioSource sfxSource;

    [Header("Las 6 Canciones")]
    [SerializeField] private MusicTrack menuMusic;
    [SerializeField] private MusicTrack tutorialMusic;
    [SerializeField] private List<MusicTrack> levelMusics;

    [Header("Efectos de Sonido (Jugador)")]
    [SerializeField] private AudioClip playerShootSFX;
    [SerializeField] private AudioClip playerHurtSFX;
    [SerializeField] private AudioClip playerDeathSFX;

    [Header("Efectos de Sonido (Enemigos)")]
    [SerializeField] private AudioClip bossHurtSFX;
    [SerializeField] private AudioClip snakeHurtSFX;
    [SerializeField] private AudioClip scorpionShotSFX;
    [SerializeField] private AudioClip batHurtSFX;

    // --- NUEVO: Sonidos de Sistema ---
    [Header("Efectos de Sistema (Victoria / Derrota)")]
    [SerializeField] private AudioClip levelCompleteSFX;
    [SerializeField] private AudioClip gameOverSFX;
    // ---------------------------------

    // --- VARIABLES DE COOLDOWN (NUEVO) ---
    private readonly float damageSoundDelay = 2f; // Tiempo de espera en segundos

    private float nextPlayerHurtTime = 0f;
    private float nextBossHurtTime = 0f;
    private float nextSnakeHurtTime = 0f;
    private float nextScorpionHurtTime = 0f;
    private float nextBatHurtTime = 0f;
    // -------------------------------------

    private bool isIntroPlaying;
    private bool isGamePaused;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (introSource != null) introSource.playOnAwake = false;
            if (loopSource != null) loopSource.playOnAwake = false;
            if (pauseSource != null) pauseSource.playOnAwake = false;
            if (sfxSource != null) sfxSource.playOnAwake = false;

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateMusicForScene(scene.name);
    }

    // ==========================================
    // MÉTODOS PARA REPRODUCIR EFECTOS (ACTUALIZADOS)
    // ==========================================

    // --- NUEVO: Métodos de Victoria y Derrota ---
    public void PlayLevelComplete()
    {
        StopAllMusic(); // Apagamos la música de fondo
        PlaySFX(levelCompleteSFX);
    }

    public void PlayGameOver()
    {
        StopAllMusic(); // Apagamos la música de fondo
        PlaySFX(gameOverSFX);
    }
    // ---------------------------------------------

    // El disparo y la muerte NO tienen delay, siempre suenan
    public void PlayPlayerShoot() { PlaySFX(playerShootSFX); }
    public void PlayPlayerDeath() { PlaySFX(playerDeathSFX); }

    // Efectos CON delay de 2 segundos
    public void PlayPlayerHurt()
    {
        if (Time.time >= nextPlayerHurtTime)
        {
            PlaySFX(playerHurtSFX);
            nextPlayerHurtTime = Time.time + damageSoundDelay;
        }
    }

    public void PlayBossHurt()
    {
        if (Time.time >= nextBossHurtTime)
        {
            PlaySFX(bossHurtSFX);
            nextBossHurtTime = Time.time + damageSoundDelay;
        }
    }

    public void PlaySnakeHurt()
    {
        if (Time.time >= nextSnakeHurtTime)
        {
            PlaySFX(snakeHurtSFX);
            nextSnakeHurtTime = Time.time + damageSoundDelay;
        }
    }

    public void PlayScorpionShot()
    {
        if (Time.time >= nextScorpionHurtTime)
        {
            PlaySFX(scorpionShotSFX);
            nextScorpionHurtTime = Time.time + damageSoundDelay;
        }
    }

    public void PlayBatHurt()
    {
        if (Time.time >= nextBatHurtTime)
        {
            PlaySFX(batHurtSFX);
            nextBatHurtTime = Time.time + damageSoundDelay;
        }
    }

    // El motor central de los efectos
    private void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    // ==========================================
    // LÓGICA DE MÚSICA (Sin Cambios)
    // ==========================================

    private void PlayMusicTrack(MusicTrack track)
    {
        introSource.Stop(); loopSource.Stop(); pauseSource.Stop();
        isIntroPlaying = false; isGamePaused = false;

        if (track.intro != null)
        {
            introSource.clip = track.intro; introSource.loop = false; introSource.Play();
            if (track.loop != null)
            {
                loopSource.clip = track.loop; loopSource.loop = true;
                isIntroPlaying = true;
            }
        }
        else if (track.loop != null)
        {
            loopSource.clip = track.loop; loopSource.loop = true; loopSource.Play();
        }
    }

    void Update()
    {
        if (isIntroPlaying && !isGamePaused && !introSource.isPlaying)
        {
            isIntroPlaying = false;
            loopSource.Play();
        }
    }

    private void UpdateMusicForScene(string sceneName)
    {
        MusicTrack trackToPlay = menuMusic;
        if (sceneName == "MainMenu") trackToPlay = menuMusic;
        else if (sceneName == "TutorialScreen") trackToPlay = tutorialMusic;
        else
        {
            if (sceneName.Contains("1") && levelMusics.Count > 0) trackToPlay = levelMusics[0];
            else if (sceneName.Contains("2") && levelMusics.Count > 1) trackToPlay = levelMusics[1];
            else if (sceneName.Contains("3") && levelMusics.Count > 2) trackToPlay = levelMusics[2];
            else if (sceneName.Contains("4") && levelMusics.Count > 3) trackToPlay = levelMusics[3];
        }
        PlayMusicTrack(trackToPlay);
    }

    public void TogglePauseMusic(bool isPaused)
    {
        isGamePaused = isPaused;
        if (isPaused)
        {
            introSource.Pause(); loopSource.Pause();
            if (tutorialMusic.loop != null)
            {
                pauseSource.clip = tutorialMusic.loop; pauseSource.loop = true; pauseSource.Play();
            }
        }
        else
        {
            pauseSource.Stop();
            if (isIntroPlaying) introSource.UnPause();
            else loopSource.UnPause();
        }
    }

    public void StopAllMusic()
    {
        isIntroPlaying = false;
        introSource.Stop(); loopSource.Stop(); pauseSource.Stop();
    }
}