using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game State")]
    public bool isGameOver = false;
    public bool isPaused   = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        isGameOver = false;
        isPaused   = false;
        SceneManager.LoadScene(sceneName);
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;
        Debug.Log("[GameManager] GAME OVER");
    }

  public void RestartGame()
{
    isGameOver = false;
    isPaused   = false;
    Time.timeScale = 1f;
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}

    public void PauseGame(bool pause)
    {
        isPaused       = pause;
        Time.timeScale = pause ? 0f : 1f;
    }
}