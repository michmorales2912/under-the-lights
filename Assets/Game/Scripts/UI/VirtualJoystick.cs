using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Joystick virtual para Android.
/// Coloca este script en el GameObject del fondo del joystick (el círculo exterior).
/// El "handle" es el círculo interior que se mueve.
/// 
/// Jerarquía sugerida en Canvas:
///   JoystickArea (este script + Image círculo exterior)
///     └── JoystickHandle (Image círculo interior, asignar en handle)
/// </summary>
public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("Referencias")]
    public RectTransform handle;        // El círculo interior
    public RectTransform joystickArea;  // Este mismo RectTransform (se asigna solo)

    [Header("Configuración")]
    public float maxRadius = 60f;       // Radio máximo de movimiento del handle en píxeles UI
    [Range(0f, 1f)]
    public float deadzone  = 0.1f;      // Zona muerta (ignorar inputs muy pequeños)

    // Input normalizado [-1, 1] en X e Y
    public Vector2 InputDirection { get; private set; } = Vector2.zero;

    private RectTransform _rectTransform;
    private Vector2 _centerPos;
    private bool _isDragging = false;

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        if (joystickArea == null)
            joystickArea = _rectTransform;
    }

    void Start()
    {
        ResetHandle();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isDragging = true;
        _centerPos  = _rectTransform.position;
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_isDragging) return;

        Vector2 delta = eventData.position - _centerPos;
        float   dist  = delta.magnitude;

        // Clampear al radio máximo
        Vector2 clamped = dist > maxRadius
            ? delta.normalized * maxRadius
            : delta;

        // Mover el handle visualmente
        if (handle != null)
            handle.anchoredPosition = clamped / _rectTransform.lossyScale.x;

        // Calcular input normalizado
        Vector2 raw = clamped / maxRadius;
        InputDirection = raw.magnitude < deadzone ? Vector2.zero : raw;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isDragging    = false;
        InputDirection = Vector2.zero;
        ResetHandle();
    }

    void ResetHandle()
    {
        if (handle != null)
            handle.anchoredPosition = Vector2.zero;
    }

    // Dibuja el radio en el editor para facilitar ajuste
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, maxRadius * 0.01f);
    }
}