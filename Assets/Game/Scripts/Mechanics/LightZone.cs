using UnityEngine;

/// <summary>
/// Coloca este script en cada zona de luz peligrosa del nivel.
/// Necesita un Collider2D en modo Trigger.
/// 
/// La hija debe tener el tag "Daughter" y el componente LightExposureSystem.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class LightZone : MonoBehaviour
{
    [Header("Configuración")]
    public float intensityMultiplier = 1f; // zonas más brillantes llenan más rápido

    [Header("Debug")]
    public bool showGizmo = true;
    public Color gizmoColor = new Color(1f, 1f, 0f, 0.25f);

    void Awake()
    {
        // Asegura que el collider sea trigger
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Daughter")) return;

        var exposure = other.GetComponent<LightExposureSystem>();
        if (exposure != null)
            exposure.EnterLightZone();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Daughter")) return;

        var exposure = other.GetComponent<LightExposureSystem>();
        if (exposure != null)
            exposure.ExitLightZone();
    }

    void OnDrawGizmos()
    {
        if (!showGizmo) return;

        Gizmos.color = gizmoColor;
        var col = GetComponent<Collider2D>();

        if (col is BoxCollider2D box)
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(box.offset, box.size);
        }
        else if (col is CircleCollider2D circle)
        {
            Gizmos.DrawSphere((Vector2)transform.position + circle.offset,
                              circle.radius * transform.lossyScale.x);
        }
    }
}