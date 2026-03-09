using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IntroSceneController : MonoBehaviour
{
    [Header("UI Groups")]
    public CanvasGroup titleGroup;
    public CanvasGroup subtitleGroup;
    public CanvasGroup buttonGroup;

    [Header("Timing")]
    public float initialDelay     = 1.0f;
    public float titleFadeDuration    = 2.5f;
    public float subtitleDelay    = 1.5f;
    public float subtitleFadeDuration = 1.5f;
    public float buttonDelay      = 2.0f;
    public float buttonFadeDuration   = 1.0f;

    [Header("Scene")]
    public string nextSceneName = "Level_01";

    void Start()
    {
        SetAlpha(titleGroup,    0f, false);
        SetAlpha(subtitleGroup, 0f, false);
        SetAlpha(buttonGroup,   0f, false);
        StartCoroutine(IntroSequence());
    }

    IEnumerator IntroSequence()
    {
        yield return new WaitForSeconds(initialDelay);
        yield return FadeTo(titleGroup, 1f, titleFadeDuration);
        yield return new WaitForSeconds(subtitleDelay);
        yield return FadeTo(subtitleGroup, 1f, subtitleFadeDuration);
        yield return new WaitForSeconds(buttonDelay);
        yield return FadeTo(buttonGroup, 1f, buttonFadeDuration);
        SetAlpha(buttonGroup, 1f, true); // activa interacción
    }

    void SetAlpha(CanvasGroup g, float alpha, bool interactable)
    {
        g.alpha = alpha;
        g.interactable = interactable;
        g.blocksRaycasts = interactable;
    }

    IEnumerator FadeTo(CanvasGroup g, float target, float duration)
    {
        float start = g.alpha;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            g.alpha = Mathf.Lerp(start, target, elapsed / duration);
            yield return null;
        }
        g.alpha = target;
    }

    // Asignar este método al botón "COMENZAR" en el Inspector
    public void OnStartPressed()
    {
        StartCoroutine(FadeOutAndLoad());
    }

    IEnumerator FadeOutAndLoad()
    {
        SetAlpha(buttonGroup, 1f, false);
        float duration = 1.5f;

        // Fade out simultáneo de todo
        float elapsed = 0f;
        float aTitle = titleGroup.alpha;
        float aSub   = subtitleGroup.alpha;
        float aBtn   = buttonGroup.alpha;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            titleGroup.alpha    = Mathf.Lerp(aTitle, 0f, t);
            subtitleGroup.alpha = Mathf.Lerp(aSub,   0f, t);
            buttonGroup.alpha   = Mathf.Lerp(aBtn,   0f, t);
            yield return null;
        }

        GameManager.Instance.LoadScene(nextSceneName);
    }
}