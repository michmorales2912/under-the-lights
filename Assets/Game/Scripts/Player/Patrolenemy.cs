using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PatrolEnemy : MonoBehaviour
{
    [Header("Patrulla")]
    public float leftLimit  = -3f;
    public float rightLimit =  3f;
    public float moveSpeed  = 1.8f;
    public float pauseAtEnd = 1f;

    [Header("Detección")]
    public float detectionRadius = 1.2f;
    public bool  detectFather    = false;

    private Rigidbody2D    _rb;
    private SpriteRenderer _sr;
    private float _startX;
    private int   _direction  =  1;
    private float _pauseTimer = 0f;
    private bool  _isPaused   = false;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _rb.gravityScale   = 1f;
        _rb.freezeRotation = true;
    }

    void Start()
    {
        _startX = transform.position.x;
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameOver) return;
        HandlePatrol();
        CheckDetection();
    }

    void HandlePatrol()
    {
        if (_isPaused)
        {
            _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
            _pauseTimer -= Time.deltaTime;
            if (_pauseTimer <= 0f) _isPaused = false;
            return;
        }

        _rb.linearVelocity = new Vector2(_direction * moveSpeed, _rb.linearVelocity.y);

        if (_sr != null) _sr.flipX = _direction < 0;

        float localX = transform.position.x - _startX;
        if (localX >= rightLimit && _direction == 1)
        {
            _direction = -1; _isPaused = true; _pauseTimer = pauseAtEnd;
        }
        else if (localX <= leftLimit && _direction == -1)
        {
            _direction =  1; _isPaused = true; _pauseTimer = pauseAtEnd;
        }
    }

    void CheckDetection()
    {
        GameObject daughter = GameObject.FindWithTag("Daughter");
        if (daughter != null)
        {
            if (Vector2.Distance(transform.position, daughter.transform.position) <= detectionRadius)
            {
                GameManager.Instance?.GameOver();
                return;
            }
        }

        if (detectFather)
        {
            GameObject father = GameObject.FindWithTag("Player");
            if (father != null)
            {
                if (Vector2.Distance(transform.position, father.transform.position) <= detectionRadius)
                    GameManager.Instance?.GameOver();
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        float originX = Application.isPlaying ? _startX : transform.position.x;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(
            new Vector3(originX + leftLimit,  transform.position.y, 0),
            new Vector3(originX + rightLimit, transform.position.y, 0)
        );
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}