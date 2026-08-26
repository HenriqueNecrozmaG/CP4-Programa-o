using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Playing,
    Paused,
    GameOver,
    Victory
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public GameState CurrentState { get; private set; }

    public Action<int> OnScoreChanged;
    public Action<int> OnLivesChanged;
    public Action<GameState> OnStateChanged;

    public static int score;
    public static int coin;
    public static int lives;
    public static int time;

    public TextMeshProUGUI textScore;
    public TextMeshProUGUI textCoin;
    public TextMeshProUGUI textLives;
    public TextMeshProUGUI textTime;
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        StartGame();
    }

    void StartGame()
    {
        score = 0;
        coin = 0;
        time = 200;

        textScore.text = "Score: " + score.ToString();
        textCoin.text = "Coin: " + coin.ToString();
        textLives.text = "Lives: " + lives.ToString();
        textTime.text = "Time: " + time.ToString();

        ChangeState(GameState.Playing);

        StartCoroutine(DecreaseTime());
    }

    public void UpdateScore(int scoreAddition)
    {
        if (CurrentState != GameState.Playing) return;

        score += scoreAddition;
        textScore.text = "Score: " + score.ToString();
    }

    public void UpdateCoins(int coinsAddition)
    {
        if (CurrentState != GameState.Playing) return;

        coin += coinsAddition;
    }

    public void UpdateLives(int livesAddition)
    {
        if (CurrentState != GameState.Playing) return;

        lives += livesAddition;

        if (lives <= 0)
        {
            ChangeState(GameState.GameOver);
            SceneManager.LoadScene("StartScene");
        }
    }

    public void Death()
    {
        if (lives > 0)
        {
            StartCoroutine(DeathWait());
        }
        else return;
    }

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;
        Time.timeScale = newState == GameState.Paused ? 0 : 1;
        OnStateChanged?.Invoke(newState);
    }
    
    public void TogglePause()
    {
        if (CurrentState == GameState.Playing)
            ChangeState(GameState.Paused);
        else if (CurrentState == GameState.Paused)
            ChangeState(GameState.Playing);
    }

    public void Victory()
    {
        ChangeState(GameState.Victory);
        StartCoroutine(WaitVictory());
    }

    IEnumerator WaitVictory()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("StartScene");
    }

    IEnumerator DeathWait()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("GameScene");
    }

    IEnumerator DecreaseTime()
    {
        yield return new WaitForSeconds(1);

        time--;
        textTime.text = "Time: " + time.ToString();

        if (time <= 0)
        {
            ChangeState(GameState.GameOver);
            SceneManager.LoadScene("StartScene");
        }
        else
        {
            StartCoroutine(DecreaseTime());
        }
    }
}