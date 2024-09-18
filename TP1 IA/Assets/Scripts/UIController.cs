using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Scenes")]
    public string newGameScene;
    public string testGameScene;
    public string mainMenuScene;

    [Header("Pause")]
    public GameObject pausePanel;
    public GameObject pauseButton;

    bool isPaused;

    [Header("Hearts Control")]
    public int heartsAmount;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    [Header("Energy Control")]
    public int drinksAmount;
    public Image[] drinks;
    public Sprite fullDrink;
    public Sprite emptyDrink;

    public RemyModel remyModel;

    private void Start()
    {
        isPaused = false;
    }

    public void Pause()
    {
        if (pausePanel) pausePanel.SetActive(true);
        if (pauseButton) pauseButton.SetActive(false);
        isPaused = true;
        Time.timeScale = 0;
    }

    public void UnPause()
    {
        if (pausePanel) pausePanel.SetActive(false);
        if (pauseButton) pauseButton.SetActive(true);
        isPaused = false;
        Time.timeScale = 1;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                UnPause();
            } 
            else
            {
                Pause();
            }
        }

        if (remyModel.Life > heartsAmount)
        {
            remyModel.Life = heartsAmount;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < remyModel.Life)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if (i < heartsAmount)
            {
                hearts[i].enabled = true;
            }
            else
                hearts[i].enabled = true;
        }

        for (int i = 0; i < drinks.Length; i++)
        {
            if (i < remyModel.Energy)
            {
                drinks[i].sprite = fullDrink;
            }
            else
            {
                drinks[i].sprite = emptyDrink;
            }

            if (i < heartsAmount)
            {
                drinks[i].enabled = true;
            }
            else
                drinks[i].enabled = true;
        }
    }

    public void NewGame()
    {
        UnPause();
        SceneManager.LoadScene(newGameScene);
    }

    public void NewTestGame()
    {
        UnPause();
        SceneManager.LoadScene(testGameScene);
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
