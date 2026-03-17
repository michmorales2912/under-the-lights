using UnityEngine;

/// <summary>
/// Enemigo que patrulla entre dos puntos.
/// Si la hija lo toca, Game Over.
/// Usa un sprite simple (rectángulo rojo) por ahora.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PatrolEnemy : MonoBehaviour
{
    [Header("Patrulla")]
    public float leftLimit   = -3f;
    public float rightLimit  =  3f;
    public float moveSpeed   = 1.8f;
    public float pauseAtEnd  = 1f;

    [Header("Detección")]
    public float detectionRadius = 1.2f; // distancia a la que detecta al padre/hija
    public bool  detectFather    = false; // true = también detecta al padre

    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private float _startX;
    private int   _direction  = 1;
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

        // Flip sprite
        if (_sr != null)
            _sr.flipX = _direction < 0;

        float localX = transform.position.x - _startX;
        if (localX >= rightLimit && _direction == 1)
        {
            _direction  = -1;
            _isPaused   = true;
            _pauseTimer = pauseAtEnd;
        }
        else if (localX <= leftLimit && _direction == -1)
        {
            _direction  = 1;
            _isPaused   = true;
            _pauseTimer = pauseAtEnd;
        }
    }

    void CheckDetection()
    {
        // Detectar hija
        GameObject daughter = GameObject.FindWithTag("Daughter");
        if (daughter != null)
        {
            float dist = Vector2.Distance(transform.position, daughter.transform.position);
            if (dist <= detectionRadius)
            {
                GameManager.Instance?.GameOver();
                return;
            }
        }

        // Detectar padre (opcional)
        if (detectFather)
        {
            GameObject father = GameObject.FindWithTag("Player");
            if (father != null)
            {
                float dist = Vector2.Distance(transform.position, father.transform.position);
                if (dist <= detectionRadius)
                    GameManager.Instance?.GameOver();
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Límites de patrulla
        float originX = Application.isPlaying ? _startX : transform.position.x;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(
            new Vector3(originX + leftLimit,  transform.position.y, 0),
            new Vector3(originX + rightLimit, transform.position.y, 0)
        );

        // Radio de detección
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}