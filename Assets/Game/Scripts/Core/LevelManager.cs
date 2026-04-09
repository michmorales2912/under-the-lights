using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [Header("Referencias personajes")]
    public FatherController    father;
    public DaughterController  daughter;
    public LightExposureSystem exposureSystem;

    [Header("UI Game Over")]
    public CanvasGroup gameOverPanel;
    public TMP_Text    gameOverTitle;
    public TMP_Text    gameOverSubtitle;
    public Button      retryButton;

    [Header("Textos")]
    public string capturedTitle    = "TE ENCONTRARON";
    public string capturedSubtitle = "Mantente en las sombras...";

    [Header("Timing")]
    public float gameOverFadeDelay    = 0.8f;
    public float gameOverFadeDuration = 1.2f;

    private bool _gameOverShown = false;

    void Start()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.alpha          = 0f;
            gameOverPanel.interactable   = false;
            gameOverPanel.blocksRaycasts = false;
        }

        if (retryButton != null)
            retryButton.onClick.AddListener(OnRetryPressed);
    }

   void Update()
{    
    if (_gameOverShown) return;

    if (GameManager.Instance != null && GameManager.Instance.isGameOver)
    {
        _gameOverShown = true;
        StartCoroutine(ShowGameOver());
    }
}

    IEnumerator ShowGameOver()
    {
        yield return new WaitForSeconds(gameOverFadeDelay);

        if (gameOverTitle    != null) gameOverTitle.text    = capturedTitle;
        if (gameOverSubtitle != null) gameOverSubtitle.text = capturedSubtitle;

        float elapsed = 0f;
        while (elapsed < gameOverFadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            gameOverPanel.alpha = Mathf.Clamp01(elapsed / gameOverFadeDuration);
            yield return null;
        }

        gameOverPanel.alpha          = 1f;
        gameOverPanel.interactable   = true;
        gameOverPanel.blocksRaycasts = true;
    }

    void OnRetryPressed()
    {
        _gameOverShown = false;
        exposureSystem?.ResetExposure();
        GameManager.Instance?.RestartGame();
    }
}