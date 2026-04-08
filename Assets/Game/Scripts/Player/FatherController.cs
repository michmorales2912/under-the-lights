using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FatherController : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 3.5f;
    public float crouchSpeedMultiplier = 0.5f;

    [Header("Salto")]
    public float jumpForce = 10f;

    [Header("Referencias")]
    public VirtualJoystick joystick;

    [Header("Sprite")]
    public bool flipOnMove = true;

    // Estado público
    public bool IsCrouching { get; private set; } = false;
    public bool IsMoving    { get; private set; } = false;
    public bool IsGrounded  { get; private set; } = false;
    public Vector2 MoveDirection { get; private set; }

    private Rigidbody2D    _rb;
    private SpriteRenderer _sr;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _rb.freezeRotation = true;
        _rb.gravityScale   = 3f;
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

  void HandleMovement()
{
    if (GameManager.Instance != null && GameManager.Instance.isGameOver)
    {
        _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
        return;
    }

    // Joystick o teclado (lo que tenga input)
    Vector2 joystickInput = joystick != null ? joystick.InputDirection : Vector2.zero;
    float keyInput = Input.GetAxis("Horizontal");
    
    float horizontal = Mathf.Abs(joystickInput.x) > 0.1f ? joystickInput.x : keyInput;

    IsMoving = Mathf.Abs(horizontal) > 0.1f;
    float speed = moveSpeed * (IsCrouching ? crouchSpeedMultiplier : 1f);
    _rb.linearVelocity = new Vector2(horizontal * speed, _rb.linearVelocity.y);

    if (flipOnMove && _sr != null && Mathf.Abs(horizontal) > 0.05f)
        _sr.flipX = horizontal < 0;
}

    // Detectar suelo por colisión directa
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
            IsGrounded = true;
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
            IsGrounded = false;
    }

    public void Jump()
    {
        if (!IsGrounded) return;
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
    }

    public void SetCrouch(bool value) => IsCrouching = value;
    public void ToggleCrouch()        => IsCrouching = !IsCrouching;
    void Update()
{
    if (Input.GetKeyDown(KeyCode.Space))
        Jump();
    
}}