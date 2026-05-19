using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void GameStart()
    {
        SceneManager.LoadScene("TutorialScreen");
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
