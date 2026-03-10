using UnityEngine;

/// <summary>
/// Hace que un reflector (SearchLight) se mueva de un lado a otro.
/// El collider hijo LightZone se mueve junto con él automáticamente.
/// </summary>
public class SearchLightMover : MonoBehaviour
{
    [Header("Movimiento")]
    public float leftLimit   = -2f;   // límite izquierdo en X local
    public float rightLimit  =  2f;   // límite derecho en X local
    public float moveSpeed   = 1.5f;

    [Header("Pausa en extremos")]
    public float pauseDuration = 0.8f; // segundos que espera antes de volver

    private float   _startX;
    private int     _direction = 1;    // 1 = derecha, -1 = izquierda
    private float   _pauseTimer = 0f;
    private bool    _isPaused = false;

    void Start()
    {
        _startX = transform.position.x;
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameOver) return;

        if (_isPaused)
        {
            _pauseTimer -= Time.deltaTime;
            if (_pauseTimer <= 0f)
                _isPaused = false;
            return;
        }

        // Mover
        transform.Translate(Vector3.right * _direction * moveSpeed * Time.deltaTime);

        float localX = transform.position.x - _startX;

        // Rebotar en límites
        if (localX >= rightLimit && _direction == 1)
        {
            _direction  = -1;
            _isPaused   = true;
            _pauseTimer = pauseDuration;
        }
        else if (localX <= leftLimit && _direction == -1)
        {
            _direction  = 1;
            _isPaused   = true;
            _pauseTimer = pauseDuration;
        }
    }

    // Gizmo para ver los límites en el editor
    void OnDrawGizmosSelected()
    {
        float originX = Application.isPlaying ? _startX : transform.position.x;
        Vector3 left  = new Vector3(originX + leftLimit,  transform.position.y, 0);
        Vector3 right = new Vector3(originX + rightLimit, transform.position.y, 0);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(left, right);
        Gizmos.DrawWireSphere(left,  0.1f);
        Gizmos.DrawWireSphere(right, 0.1f);
    }
}