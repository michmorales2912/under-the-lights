using UnityEngine;

/// <summary>
/// Scroll automático con loop infinito para fondos.
/// </summary>
public class ParallaxLayer : MonoBehaviour
{
    [Header("Scroll")]
    public float scrollSpeed    = 0.05f;
    [Range(0f, 1f)]
    public float parallaxFactor = 0.5f;
    public bool  autoScroll     = true;

    [Header("Loop")]
    public float spriteWidth = 20f; // ancho del sprite en unidades

    private float _startX;

    void Start()
    {
        _startX = transform.position.x;

        // Detectar ancho automáticamente si tiene SpriteRenderer
        var sr = GetComponent<SpriteRenderer>();
        if (sr != null && sr.sprite != null)
            spriteWidth = sr.bounds.size.x;
    }

    void Update()
    {
        if (!autoScroll) return;

        transform.position += Vector3.left * scrollSpeed * parallaxFactor * Time.deltaTime;

        // Loop infinito cuando sale de pantalla
        if (transform.position.x <= _startX - spriteWidth)
            transform.position = new Vector3(_startX, transform.position.y, transform.position.z);
    }
}