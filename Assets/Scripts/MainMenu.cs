using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Image jungle;
    public Image desert;

    public void PlayGame()
    {
        if (jungle.enabled)
            SceneManager.LoadScene("Game");
        else if (desert.enabled)
            SceneManager.LoadScene("GAME_TESTING");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
