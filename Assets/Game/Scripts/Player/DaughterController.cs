using UnityEngine;

/// <summary>
/// La hija sigue al padre automáticamente manteniéndose cerca.
/// Implementa seguimiento suave con offset y detección de obstáculos.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class DaughterController : MonoBehaviour
{
    [Header("Referencias")]
    public FatherController father;

    [Header("Seguimiento")]
    public float followSpeed       = 3.0f;
    public float followDistance    = 1.2f;   // distancia ideal al padre
    public float stopDistance      = 0.6f;   // distancia mínima antes de detenerse
    public float catchUpMultiplier = 1.8f;   // se apresura si queda muy atrás

    [Header("Posición relativa")]
    public Vector2 followOffset = new Vector2(-0.8f, 0f); // ligeramente detrás del padre

    [Header("Sprite")]
    public bool flipWithFather = true;

    // Estado público
    public bool IsMoving { get; private set; }

    private Rigidbody2D  _rb;
    private Animator     _anim;
    private SpriteRenderer _sr;

    void Awake()
    {
        _rb  = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _sr  = GetComponent<SpriteRenderer>();

        _rb.gravityScale   = 0f;
        _rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        if (father == null) return;
        if (GameManager.Instance != null && GameManager.Instance.isGameOver)
        {
            _rb.linearVelocity = Vector2.zero;
            return;
        }

        FollowFather();
    }

    void FollowFather()
    {
        // Calcula el offset según hacia dónde mira el padre
        float fatherDir = father.GetComponent<SpriteRenderer>().flipX ? 1f : -1f;
        Vector2 offset  = new Vector2(followOffset.x * fatherDir, followOffset.y);
        Vector2 targetPos = (Vector2)father.transform.position + offset;

        float dist = Vector2.Distance(transform.position, targetPos);
        IsMoving   = dist > stopDistance;

        if (!IsMoving)
        {
            _rb.linearVelocity = Vector2.zero;
        }
        else
        {
            // Acelera si está muy lejos (la hija corre para alcanzar)
            float speedMultiplier = dist > followDistance * 2.5f ? catchUpMultiplier : 1f;
            Vector2 dir = (targetPos - (Vector2)transform.position).normalized;
            _rb.linearVelocity = dir * followSpeed * speedMultiplier;
        }

        // Flip del sprite
        if (flipWithFather && _sr != null)
            _sr.flipX = father.GetComponent<SpriteRenderer>().flipX;

        // Animaciones
        if (_anim != null)
        {
            _anim.SetBool("IsMoving",    IsMoving);
            _anim.SetBool("IsCrouching", father.IsCrouching);
        }
    }

    /// <summary>
    /// Detiene a la hija en el lugar (ej: animación de captura).
    /// </summary>
    public void StopImmediate()
    {
        _rb.linearVelocity = Vector2.zero;
        IsMoving = false;
    }
}