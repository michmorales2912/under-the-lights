using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Controla el estado general del Level_01:
/// inicio, game over, UI de retry.
/// </summary>
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

    void Start()
    {
        // Panel de game over oculto al inicio
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
        // Detectar game over desde GameManager
        if (GameManager.Instance != null &&
            GameManager.Instance.isGameOver &&
            gameOverPanel != null &&
            gameOverPanel.alpha == 0f)
        {
            StartCoroutine(ShowGameOver());
        }
    }

    IEnumerator ShowGameOver()
    {
        yield return new WaitForSeconds(gameOverFadeDelay);

        // Textos
        if (gameOverTitle    != null) gameOverTitle.text    = capturedTitle;
        if (gameOverSubtitle != null) gameOverSubtitle.text = capturedSubtitle;

        // Fade in del panel
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
        exposureSystem?.ResetExposure();
        GameManager.Instance?.RestartGame();
    }
}