using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void GameStart()
    {
        SceneManager.LoadScene("Level_1");
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
