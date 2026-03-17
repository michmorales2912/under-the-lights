using UnityEngine;
using TMPro;

/// <summary>
/// Checkpoint — guarda la posición del jugador.
/// Si hay Game Over, reaparece aquí en lugar del inicio.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Checkpoint : MonoBehaviour
{
    [Header("Configuración")]
    public int checkpointID = 1;

    [Header("Visual")]
    public SpriteRenderer indicatorSprite;
    public Color inactiveColor = new Color(1f, 1f, 1f, 0.3f);
    public Color activeColor   = new Color(0f, 1f, 0.5f, 0.8f);

    private bool _activated = false;

    // Posición de respawn guardada globalmente
    public static Vector3 LastCheckpointPos;
    public static bool    HasCheckpoint = false;

    void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
        if (indicatorSprite != null)
            indicatorSprite.color = inactiveColor;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (_activated) return;
        if (!other.CompareTag("Player")) return;

        _activated = true;
        LastCheckpointPos = transform.position;
        HasCheckpoint     = true;

        if (indicatorSprite != null)
            indicatorSprite.color = activeColor;

        Debug.Log($"[Checkpoint {checkpointID}] Activado en {transform.position}");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = _activated ? Color.green : Color.white;
        Gizmos.DrawWireCube(transform.position, new Vector3(1f, 2f, 0));
    }
}