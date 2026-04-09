using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game State")]
    public bool isGameOver = false;
    public bool isPaused = false;

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
        SceneManager.LoadScene(sceneName);
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;
        Debug.Log("[GameManager] GAME OVER — La niña fue detectada");
        // Aquí irá la animación de captura más adelante
    }
public void RestartGame()
{
    isGameOver = false;
    isPaused   = false;

    if (Checkpoint.HasCheckpoint)
    {
        // Recargar escena y reposicionar en checkpoint
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // El reposicionamiento se hace en LevelManager.Start()
    }
    else
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

    public void PauseGame(bool pause)
    {
        isPaused = pause;
        Time.timeScale = pause ? 0f : 1f;
    }
}