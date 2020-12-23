using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Image jungle;
    public Image desert;

    void Start()
    {
        PlayerPrefs.SetString("CurrentScene", "Game"); //Load the jungle as a default 
        jungle.enabled = true;
        desert.enabled = false;
        PlayerPrefs.SetInt("CurrentDifficulty", 0); //Default is easy
    }

    public void PlayGame()
    {
        string nameOfCurrScene = PlayerPrefs.GetString("CurrentScene");
        SceneManager.LoadScene(nameOfCurrScene);     
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
