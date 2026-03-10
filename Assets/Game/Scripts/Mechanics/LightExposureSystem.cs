using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Sistema de exposición a la luz para la hija.
/// 
/// - Detecta si la hija está dentro de una zona de luz (LightZone)
/// - Llena/vacía la barra de exposición
/// - Al llegar al 100% llama a GameManager.GameOver()
/// 
/// Las zonas de luz son colliders 2D con el tag "LightZone" y el script LightZone.cs
/// </summary>
public class LightExposureSystem : MonoBehaviour
{
    [Header("Exposición")]
    public float maxExposure       = 100f;
    public float exposureFillRate  = 25f;   // unidades por segundo dentro de la luz
    public float exposureDrainRate = 15f;   // unidades por segundo fuera de la luz

    [Header("Umbrales")]
    [Range(0f, 100f)]
    public float warningThreshold  = 50f;   // % para efectos de advertencia
    [Range(0f, 100f)]
    public float criticalThreshold = 80f;   // % para efectos críticos

    [Header("Referencias UI")]
    public ExposureBarUI exposureBarUI; // asignar en Inspector

    [Header("Efectos")]
    public SpriteRenderer daughterRenderer;
    public Color safeColor     = Color.white;
    public Color warningColor  = new Color(1f, 0.7f, 0.3f);
    public Color criticalColor = new Color(1f, 0.2f, 0.2f);

    // Estado
    public float CurrentExposure { get; private set; } = 0f;
    public float ExposurePercent => CurrentExposure / maxExposure;
    public bool  IsExposed       { get; private set; } = false;

    private int _lightZoneCount = 0; // cuántas zonas de luz la cubren actualmente
    private bool _gameOverTriggered = false;

    void Update()
    {
        if (_gameOverTriggered) return;
        if (GameManager.Instance != null && GameManager.Instance.isGameOver) return;

        UpdateExposure();
        UpdateVisuals();
    }

    void UpdateExposure()
    {
        IsExposed = _lightZoneCount > 0;

        if (IsExposed)
        {
            CurrentExposure += exposureFillRate * Time.deltaTime;
            CurrentExposure  = Mathf.Clamp(CurrentExposure, 0f, maxExposure);

            if (CurrentExposure >= maxExposure)
                TriggerCapture();
        }
        else
        {
            CurrentExposure -= exposureDrainRate * Time.deltaTime;
            CurrentExposure  = Mathf.Clamp(CurrentExposure, 0f, maxExposure);
        }

        // Actualizar UI
        if (exposureBarUI != null)
            exposureBarUI.SetExposure(ExposurePercent);
    }

    void UpdateVisuals()
    {
        if (daughterRenderer == null) return;

        if (ExposurePercent >= criticalThreshold / 100f)
            daughterRenderer.color = Color.Lerp(warningColor, criticalColor,
                (ExposurePercent - criticalThreshold / 100f) / (1f - criticalThreshold / 100f));

        else if (ExposurePercent >= warningThreshold / 100f)
            daughterRenderer.color = Color.Lerp(safeColor, warningColor,
                (ExposurePercent - warningThreshold / 100f) / ((criticalThreshold - warningThreshold) / 100f));

        else
            daughterRenderer.color = safeColor;
    }

    void TriggerCapture()
    {
        if (_gameOverTriggered) return;
        _gameOverTriggered = true;

        if (daughterRenderer != null)
            StartCoroutine(CaptureFlash());

        GameManager.Instance?.GameOver();
    }

    IEnumerator CaptureFlash()
    {
        // Flash rápido blanco → rojo para indicar captura
        for (int i = 0; i < 3; i++)
        {
            daughterRenderer.color = Color.white;
            yield return new WaitForSeconds(0.08f);
            daughterRenderer.color = Color.red;
            yield return new WaitForSeconds(0.08f);
        }
    }

    // ─── Llamados por LightZone.cs via trigger ───────────────────────────────

    public void EnterLightZone()
    {
        _lightZoneCount++;
    }

    public void ExitLightZone()
    {
        _lightZoneCount = Mathf.Max(0, _lightZoneCount - 1);
    }

    public void ResetExposure()
    {
        CurrentExposure    = 0f;
        _lightZoneCount    = 0;
        _gameOverTriggered = false;
        if (daughterRenderer != null)
            daughterRenderer.color = safeColor;
    }
}