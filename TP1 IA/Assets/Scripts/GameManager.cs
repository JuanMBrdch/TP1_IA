using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private string victorySceneName = "VictoryScene"; // Nombre de la escena de victoria
    [SerializeField] private string gameOverSceneName = "GameOverScene"; // Nombre de la escena de GameOver

    private void Awake()
    {
        // Verificar si ya existe una instancia
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destruir el duplicado
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Hacer que persista entre escenas
    }

    private void OnEnable()
    {
        RemyModel.DieAction += OnPlayerDeath;
    }

    private void OnDisable()
    {
        RemyModel.DieAction -= OnPlayerDeath;
    }

    private void OnPlayerDeath()
    {
        // Cargar escena de GameOver
        SceneManager.LoadScene(gameOverSceneName);
    }

    public void TriggerVictory()
    {
        SceneManager.LoadScene(victorySceneName);
    }

    public void TriggerGameOver()
    {
        SceneManager.LoadScene(gameOverSceneName);
    }
}


