using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Image jungle;
    public Image desert;
    public Image winter;
    public Image arena;
    public Image boss;

    void Start()
    {
        PlayerPrefs.SetString("CurrentScene", "Game1"); //Load the jungle as a default 
        jungle.enabled = true;
        desert.enabled = false;
        winter.enabled = false;
        arena.enabled = false;
        boss.enabled = false;
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
