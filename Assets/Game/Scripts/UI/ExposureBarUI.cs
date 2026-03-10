using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controla la barra visual de exposición a la luz.
/// Usa un Slider de Unity UI o una Image con Fill.
/// 
/// Jerarquía sugerida en Canvas:
///   ExposureBar (este script)
///     ├── BarBackground  (Image, color oscuro)
///     ├── BarFill        (Image, Fill Method: Horizontal) ← asignar en fillImage
///     └── IconLight      (Image, ícono de luz/ojo)
/// </summary>
public class ExposureBarUI : MonoBehaviour
{
    [Header("Referencias")]
    public Image fillImage;         // La imagen que se llena (Fill Amount)
    public Image barBackground;
    public CanvasGroup canvasGroup; // Para ocultar cuando está en 0

    [Header("Colores según peligro")]
    public Color safeColor     = new Color(0.2f, 0.8f, 0.3f);   // verde
    public Color warningColor  = new Color(1f,   0.7f, 0.1f);   // amarillo
    public Color criticalColor = new Color(1f,   0.15f, 0.1f);  // rojo

    [Header("Umbrales")]
    [Range(0f, 1f)] public float warningAt  = 0.5f;
    [Range(0f, 1f)] public float criticalAt = 0.8f;

    [Header("Visibilidad")]
    public bool hideWhenEmpty = true;
    public float fadeDuration = 0.3f;

    private float _targetAlpha = 0f;
    private float _currentAlpha = 0f;

    void Awake()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    /// <summary>
    /// Actualiza la barra. Llamado por LightExposureSystem cada frame.
    /// </summary>
    /// <param name="percent">Valor entre 0 y 1</param>
    public void SetExposure(float percent)
    {
        percent = Mathf.Clamp01(percent);

        // Fill
        if (fillImage != null)
            fillImage.fillAmount = percent;

        // Color
        if (fillImage != null)
        {
            if (percent >= criticalAt)
                fillImage.color = Color.Lerp(warningColor, criticalColor,
                    (percent - criticalAt) / (1f - criticalAt));

            else if (percent >= warningAt)
                fillImage.color = Color.Lerp(safeColor, warningColor,
                    (percent - warningAt) / (criticalAt - warningAt));

            else
                fillImage.color = safeColor;
        }

        // Visibilidad
        _targetAlpha = (hideWhenEmpty && percent <= 0.01f) ? 0f : 1f;
    }

    void Update()
    {
        // Fade suave de la barra
        if (canvasGroup != null && !Mathf.Approximately(_currentAlpha, _targetAlpha))
        {
            _currentAlpha = Mathf.MoveTowards(_currentAlpha, _targetAlpha,
                Time.deltaTime / fadeDuration);
            canvasGroup.alpha = _currentAlpha;
        }
    }
}