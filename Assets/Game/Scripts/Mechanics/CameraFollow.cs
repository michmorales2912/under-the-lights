using UnityEngine;

/// <summary>
/// Cámara que sigue al padre suavemente.
/// Agrega este script a la Main Camera.
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [Header("Referencias")]
    public Transform target; // arrastra Father aquí

    [Header("Seguimiento")]
    public float smoothSpeed   = 5f;
    public Vector2 offset      = new Vector2(1.5f, 1f); // adelantada y un poco arriba

    [Header("Límites del nivel")]
    public float minX = 0f;
    public float maxX = 50f;
    public float minY = -5f;
    public float maxY = 5f;

    [Header("Solo seguir en X")]
    public bool lockY = true; // true = cámara no sube/baja

    private Vector3 _velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null) return;

        float targetX = target.position.x + offset.x;
        float targetY = lockY ? transform.position.y : target.position.y + offset.y;

        // Clampear dentro de los límites del nivel
        targetX = Mathf.Clamp(targetX, minX, maxX);
        targetY = Mathf.Clamp(targetY, minY, maxY);

        Vector3 targetPos = new Vector3(targetX, targetY, transform.position.z);

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPos,
            ref _velocity,
            1f / smoothSpeed
        );
    }

    // Dibuja los límites en el editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 bottomLeft  = new Vector3(minX, minY, 0);
        Vector3 bottomRight = new Vector3(maxX, minY, 0);
        Vector3 topLeft     = new Vector3(minX, maxY, 0);
        Vector3 topRight    = new Vector3(maxX, maxY, 0);

        Gizmos.DrawLine(bottomLeft,  bottomRight);
        Gizmos.DrawLine(bottomRight, topRight);
        Gizmos.DrawLine(topRight,    topLeft);
        Gizmos.DrawLine(topLeft,     bottomLeft);
    }
}