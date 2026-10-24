

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // singleton for GameManager
    public static GameManager Instance { get; private set; }

    // different states for the game
    public enum GameState
    {
        Playing,
        Paused,
        GameOver
    }

    public GameState CurrentState { get; private set; }

    [Header("Game Data")]
    public int score = 0;
    public int lives = 3;

    // Events that other scripts can subscribe to
    public event Action<int> ScoreChanged;
    public event Action<int> LivesChanged;

    public GameObject gameOverPanel;
    public GameObject pausePanel;

    private void Awake()
    {
        // keeps only one GameManager
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            // destroys an extra copy of the manager
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // starts the game in the playing state
        ChangeState(GameState.Playing);
    }

    private void Update()
    {
        // presses escape to pause or resume the game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (CurrentState == GameState.Playing)
            {
                ChangeState(GameState.Paused);
            }
            else if (CurrentState == GameState.Paused)
            {
                ChangeState(GameState.Playing);
            }
        }
    }

    public void ChangeState(GameState newState)
    {
        // changes the current game state
        CurrentState = newState;

        // checks what state the game is in
        switch (CurrentState)
        {
            case GameState.Playing:
                // unpauses the game and hides the panels
                Time.timeScale = 1f;
                pausePanel.SetActive(false);
                gameOverPanel.SetActive(false);
                break;

            case GameState.Paused:
                // pauses the game and shows the pause panel
                Time.timeScale = 0f;
                pausePanel.SetActive(true);
                gameOverPanel.SetActive(false);
                break;

            case GameState.GameOver:
                // shows game over and pauses the game
                Time.timeScale = 0f;
                pausePanel.SetActive(false);
                gameOverPanel.SetActive(true);
                break;
        }
    }

    public void CoinPickedUp(int points)
    {
        // prevents collecting coins when not playing
        if (CurrentState != GameState.Playing) return;

        // adds points when a coin is collected
        score += points;

        // shows the score in the console
        Debug.Log("Score: " + score);

        // notifys listeners of the new score
        ScoreChanged?.Invoke(score);
    }

    public void LoseLife()
    {
        // prevents lives from going below zero
        if (lives <= 0 || CurrentState != GameState.Playing) return;

        // takes away one life
        lives--;

        // sends signal to listeners of the lives left
        LivesChanged?.Invoke(lives);

        if (lives == 0)
        {
            // shows game over and pauses the game
            ChangeState(GameState.GameOver);
        }
    }

    public void RestartGame()
    {
        // unpauses the game
        Time.timeScale = 1f;

        // reloads the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
