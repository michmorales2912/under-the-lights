using UnityEngine;

/// <summary>
/// Zona de luz peligrosa. Necesita Collider2D en modo Trigger.
/// La hija debe tener tag "Daughter" y componente LightExposureSystem.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class LightZone : MonoBehaviour
{
    [Header("Debug")]
    public bool  showGizmo  = true;
    public Color gizmoColor = new Color(1f, 1f, 0f, 0.25f);

    void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Daughter")) return;
        other.GetComponent<LightExposureSystem>()?.EnterLightZone();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        // OnTriggerStay asegura que si la hija ya estaba dentro al activarse, se detecte
        if (!other.CompareTag("Daughter")) return;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Daughter")) return;
        other.GetComponent<LightExposureSystem>()?.ExitLightZone();
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
            Gizmos.DrawSphere(
                (Vector2)transform.position + circle.offset,
                circle.radius * transform.lossyScale.x
            );
        }
    }
}