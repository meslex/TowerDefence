using Camera_and_UI;
using MoneyHealth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Working,
    Paused
}

/// <summary>
/// Pauses and resumes game
/// </summary>
public class PauseContoller : BaseUIPanel
{
    //[SerializeField] private GameObject panel = default;

    public GameState CurrentGameState { get; private set; }
    private bool canPause;

    private void OnEnable()
    {
        canPause = true;
    }

    private void Start()
    {
        Hide();
        CurrentGameState = GameState.Working;
        HealthController.Instance.GameOver += BlockPause;
    }

    private void OnDisable()
    {
        HealthController hp = HealthController.Instance;
        hp.GameOver -= BlockPause;
    }

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape) && canPause)
        {
            if (CurrentGameState == GameState.Paused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        CurrentGameState = GameState.Paused;
        Show();
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        CurrentGameState = GameState.Working;
        Hide();
        Time.timeScale = 1;
    }

    //In case if player pressed esc key when game over screen is enabled
    private void BlockPause()
    {
        canPause = false;
    }

    public void RestartLevel()
    {
        if (SceneController.Instance != null)
        {
            ResumeGame();
            string sceneName = SceneManager.GetActiveScene().name;
            SceneController.Instance.FadeAndLoadScene(sceneName);
        }
        else
        {
            Debug.LogError("[PauseController] can't find SceneController");
        }

    }
}
