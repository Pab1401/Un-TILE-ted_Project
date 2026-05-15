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

    [Header("Reproductores")]
    [SerializeField] private AudioSource introSource;
    [SerializeField] private AudioSource loopSource;
    [SerializeField] private AudioSource pauseSource;

    [Header("Las 6 Canciones")]
    [SerializeField] private MusicTrack menuMusic;
    [SerializeField] private MusicTrack tutorialMusic;
    [SerializeField] private List<MusicTrack> levelMusic;

    private bool wasIntroPlaying;
    private int savedTimeSamples;

    void Awake()
    {
        // Singleton perfecto: Solo sobrevive el original
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Nos suscribimos al evento de cambio de escena de Unity
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // Limpieza de eventos por seguridad
        if (Instance == this) SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Esta función se dispara SOLA cada vez que carga una escena
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateMusicForScene(scene.name);
    }

    private void PlayMusicTrack(MusicTrack track)
    {
        introSource.Stop(); loopSource.Stop(); pauseSource.Stop();

        if (track.intro != null)
        {
            introSource.clip = track.intro;
            introSource.loop = false;
            introSource.Play();

            if (track.loop != null)
            {
                loopSource.clip = track.loop;
                loopSource.loop = true;
                double introDuration = (double)track.intro.samples / track.intro.frequency;
                loopSource.PlayScheduled(AudioSettings.dspTime + introDuration);
            }
        }
        else if (track.loop != null)
        {
            loopSource.clip = track.loop;
            loopSource.loop = true;
            loopSource.Play();
        }
    }

    private void UpdateMusicForScene(string sceneName)
    {
        MusicTrack trackToPlay = menuMusic;

        if (sceneName == "MainMenu") trackToPlay = menuMusic;
        else if (sceneName == "Tutorial") trackToPlay = tutorialMusic;
        else
        {
            if (sceneName.Contains("1") && levelMusic.Count > 0) trackToPlay = levelMusic[0];
            else if (sceneName.Contains("2") && levelMusic.Count > 1) trackToPlay = levelMusic[1];
            else if (sceneName.Contains("3") && levelMusic.Count > 2) trackToPlay = levelMusic[2];
            else if (sceneName.Contains("4") && levelMusic.Count > 3) trackToPlay = levelMusic[3];
        }

        PlayMusicTrack(trackToPlay);
    }

    // El GameManager llamará a esto al pausar/despausar
    public void TogglePauseMusic(bool isPaused)
    {
        if (isPaused)
        {
            wasIntroPlaying = introSource.isPlaying;
            savedTimeSamples = wasIntroPlaying ? introSource.timeSamples : loopSource.timeSamples;

            introSource.Stop();
            loopSource.Stop();

            if (tutorialMusic.loop != null)
            {
                pauseSource.clip = tutorialMusic.loop;
                pauseSource.loop = true;
                pauseSource.Play();
            }
        }
        else
        {
            pauseSource.Stop();

            if (wasIntroPlaying)
            {
                introSource.timeSamples = savedTimeSamples;
                introSource.Play();
                double remainingTime = (double)(introSource.clip.samples - savedTimeSamples) / introSource.clip.frequency;
                loopSource.PlayScheduled(AudioSettings.dspTime + remainingTime);
            }
            else
            {
                loopSource.timeSamples = savedTimeSamples;
                loopSource.Play();
            }
        }
    }

    // El GameManager llamará a esto al ganar/perder
    public void StopAllMusic()
    {
        introSource.Stop();
        loopSource.Stop();
        pauseSource.Stop();
    }
}