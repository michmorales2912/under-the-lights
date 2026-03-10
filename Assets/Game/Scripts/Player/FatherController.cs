using UnityEngine;

/// <summary>
/// Controla el movimiento del padre usando el joystick virtual.
/// Requiere Rigidbody2D en el mismo GameObject.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class FatherController : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 3.5f;
    public float crouchSpeedMultiplier = 0.5f;

    [Header("Referencias")]
    public VirtualJoystick joystick;

    [Header("Sprite")]
    public bool flipOnMove = true;

    // Estado
    public bool IsCrouching { get; private set; } = false;
    public bool IsMoving    { get; private set; } = false;
    public Vector2 MoveDirection { get; private set; }

    private Rigidbody2D _rb;
    private Animator    _anim;
    private SpriteRenderer _sr;

    void Awake()
    {
        _rb   = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _sr   = GetComponent<SpriteRenderer>();

        _rb.gravityScale = 0f; // Juego 2D side-scroll sin gravedad (plataformer se ajusta después)
        _rb.freezeRotation = true;
    }

    void Update()
    {
        HandleCrouch();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameOver)
        {
            _rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 input = joystick != null ? joystick.InputDirection : Vector2.zero;
        MoveDirection = input;
        IsMoving = input.magnitude > 0.1f;

        float speed = moveSpeed * (IsCrouching ? crouchSpeedMultiplier : 1f);
        _rb.linearVelocity = input * speed;

        // Flip del sprite según dirección horizontal
        if (flipOnMove && _sr != null && Mathf.Abs(input.x) > 0.05f)
            _sr.flipX = input.x < 0;

        // Animaciones
        if (_anim != null)
        {
            _anim.SetBool("IsMoving",   IsMoving);
            _anim.SetBool("IsCrouching", IsCrouching);
        }
    }

    void HandleCrouch()
    {
        // El agacharse se activa desde el botón de crouch (ver CrouchButton.cs)
        // También se puede activar cuando el joystick apunta fuerte hacia abajo
        if (joystick != null && joystick.InputDirection.y < -0.8f)
            SetCrouch(true);
        else if (joystick != null && joystick.InputDirection.y >= -0.8f && IsCrouching)
            SetCrouch(false);
    }

    public void SetCrouch(bool value)
    {
        IsCrouching = value;
    }

    // Llamado externamente por botón dedicado de crouch
    public void ToggleCrouch()
    {
        IsCrouching = !IsCrouching;
    }
}