using UnityEngine;

/// <summary>
/// Asigna a cada capa del fondo. Scroll automático con loop para la escena de intro.
/// </summary>
public class ParallaxLayer : MonoBehaviour
{
    [Header("Scroll")]
    public float scrollSpeed = 0.05f;
    [Range(0f, 1f)]
    public float parallaxFactor = 0.5f;

    private float spriteWidth;
    private float startX;

    void Start()
    {
        var sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            spriteWidth = sr.bounds.size.x;

        startX = transform.position.x;
    }

    void Update()
    {
        transform.position += Vector3.left * scrollSpeed * parallaxFactor * Time.deltaTime;

        // Loop infinito
        if (transform.position.x <= startX - spriteWidth)
        {
            transform.position = new Vector3(startX, transform.position.y, transform.position.z);
        }
    }
}