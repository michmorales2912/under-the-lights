using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DaughterController : MonoBehaviour
{
    [Header("Referencias")]
    public FatherController father;

    [Header("Seguimiento")]
    public float followSpeed  = 8.0f;
    public float stopDistance = 0.3f;
    public float offsetX      = -0.8f;
    public float offsetY      = -0.8f;

    private Rigidbody2D    _rb;
    private SpriteRenderer _sr;
    private SpriteRenderer _fatherSr;

    public bool IsMoving { get; private set; }

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _rb.gravityScale   = 0f;
        _rb.freezeRotation = true;
        _rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void Start()
    {
        if (father != null)
            _fatherSr = father.GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (father == null) return;
        if (GameManager.Instance != null && GameManager.Instance.isGameOver)
            return;

        FollowFather();
    }

    void FollowFather()
    {
        float dir = (_fatherSr != null && _fatherSr.flipX) ? 1f : -1f;
        Vector2 targetPos = new Vector2(
            father.transform.position.x + offsetX * dir,
            father.transform.position.y + offsetY
        );

        float dist = Vector2.Distance(_rb.position, targetPos);
        IsMoving   = dist > stopDistance;

        if (IsMoving)
        {
            Vector2 newPos = Vector2.MoveTowards(
                _rb.position,
                targetPos,
                followSpeed * Time.fixedDeltaTime
            );
            _rb.MovePosition(newPos);
        }

        if (_sr != null && _fatherSr != null)
            _sr.flipX = _fatherSr.flipX;
    }

    public void StopImmediate() => IsMoving = false;
}