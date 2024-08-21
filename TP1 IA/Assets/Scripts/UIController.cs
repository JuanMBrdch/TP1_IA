using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [Header("Scenes")]
    public string newGameScene;
    public string mainMenuScene;

    [Header("Pause")]
    public GameObject pausePanel;
    public GameObject pauseButton;

    public void Pause()
    {
        pausePanel.SetActive(true);
        pauseButton.SetActive(false);
        Time.timeScale = 0;
    }

    public void UnPause()
    {
        pausePanel.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1;
    }

    public void NewGame()
    {
        SceneManager.LoadScene(newGameScene);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
