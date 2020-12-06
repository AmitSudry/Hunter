using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool paused = false;

    public GameObject menuUI;
    public GameObject playerUI;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            if(paused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerUI.SetActive(true);
        menuUI.SetActive(false);
        Time.timeScale = 1.0f;
        paused = false;
    }

    void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        playerUI.SetActive(false);
        menuUI.SetActive(true);
        Time.timeScale = 0.0f;
        paused = true;
    }

    public void LoadMenu()
    {
        Resume();
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}