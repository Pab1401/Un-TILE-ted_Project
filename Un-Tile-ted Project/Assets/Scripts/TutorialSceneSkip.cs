using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialSceneSkip : MonoBehaviour
{
    void Start()
    {
        // Start the timer as soon as the scene begins
        StartCoroutine(LoadSceneAfterDelay(7f));
    }

    private IEnumerator LoadSceneAfterDelay(float delay)
    {
        // Wait for 2 seconds
        yield return new WaitForSeconds(delay);

        // Load the new scene
        SceneManager.LoadScene("Level_1");
    }
}
