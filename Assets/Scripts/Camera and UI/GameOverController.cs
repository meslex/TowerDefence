using Camera_and_UI;
using MoneyHealth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Shows/Hides gameOverPanel
/// </summary>
public class GameOverController : BaseUIPanel
{
    public event Action OnRestart;

    [SerializeField] private Text scoreText = default;
    [SerializeField] private Text enemiesKilled = default;
    // Start is called before the first frame update
    void Start()
    {
        Hide();
        HealthController.Instance.GameOver += ShowGameOverWindow;
    }

    public void ShowGameOverWindow()
    {
        Show();
        Time.timeScale = 0;
        scoreText.text = $"SCORE: {MoneyContoller.Instance.Score}";
        enemiesKilled.text = $"ENEMIES KILLED: {MoneyContoller.Instance.EnemiesKilled}";
    }

    public void Restart()
    {
        OnRestart?.Invoke();
    }
}
