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
        if (pausePanel) pausePanel.SetActive(true);
        if (pauseButton) pauseButton.SetActive(false);
        Time.timeScale = 0;
    }

    public void UnPause()
    {
        if (pausePanel) pausePanel.SetActive(false);
        if(pauseButton) pauseButton.SetActive(true);
        Time.timeScale = 1;
    }

    public void NewGame()
    {
        UnPause();
        SceneManager.LoadScene(newGameScene);
    }

    public void GoToMainMenu()
    {
        UnPause();
        SceneManager.LoadScene(mainMenuScene);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
