using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void GameStart()
    {
        SceneManager.LoadScene("Tutorial_Level");
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
