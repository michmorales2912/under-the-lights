using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Zona de sombra segura donde la exposición drena más rápido.
/// Agrega un Collider2D Trigger + este script.
/// Opcional: cambia el color del área para indicar que es segura.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class SafeZone : MonoBehaviour
{
    [Header("Configuración")]
    public float drainMultiplier = 3f; // la barra drena X veces más rápido aquí

    [Header("Visual")]
    public Color safeColor = new Color(0f, 0.5f, 1f, 0.15f); // azul muy tenue

    void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Daughter")) return;
        var exposure = other.GetComponent<LightExposureSystem>();
        if (exposure != null)
            exposure.exposureDrainRate *= drainMultiplier;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Daughter")) return;
        var exposure = other.GetComponent<LightExposureSystem>();
        if (exposure != null)
            exposure.exposureDrainRate /= drainMultiplier;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = safeColor;
        var col = GetComponent<Collider2D>();
        if (col is BoxCollider2D box)
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(box.offset, box.size);
        }
    }
}