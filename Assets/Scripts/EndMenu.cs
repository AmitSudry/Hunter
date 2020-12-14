using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndMenu : MonoBehaviour
{
    public void PlayGame()
    {
        //Resart level method
        //get the prefernce indicating the previous scene loaded name
        string nameOfCurrScene = PlayerPrefs.GetString("CurrentScene");
        SceneManager.LoadScene(nameOfCurrScene);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
