using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Character : MonoBehaviour
{
    protected GameManager _gameManager;
    
    public float currentSpeed;
    
    protected const float DefaultSpeed = 6.5f;
    protected float jumpForce = 13.5f;
    protected float rotationSpeed = 280.0f;
    
    protected bool _isGrounded;
    
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected ParticleSystem _particleSystem;

    protected Vector3 startPosition;
    protected Transform _particleSystemParent;

    protected virtual void Start()
    {
        Time.timeScale = 1.3f; // Game speed
        _isGrounded = true; 
        _gameManager = GameManager.Instance;
        startPosition = transform.position;
        currentSpeed = DefaultSpeed;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        _particleSystemParent = new GameObject("ParticleSystemParent").transform;
    }
    
    protected virtual void FixedUpdate()
    {
        if (!_gameManager.PlayMode) return; // Si le jeu est pas lancé

        transform.Translate(new Vector2(currentSpeed * Time.deltaTime, 0));
        CheckGrounded();
        
        HandleMovement();
    }

    protected virtual void Update()
    {
        // Gestion du système de particule
        if (!CheckGrounded()) {
            DettachParticleSystem();
        } else {
            ReattachParticleSystem();
        }

        if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)) {
            onClick();
        }
    }

    public abstract void onClick();

    protected virtual bool CheckGrounded()
    {
        Vector2 frontRayOrigin = rb.position + new Vector2(0.5f, 0);
        Vector2 backRayOrigin = rb.position - new Vector2(0.47f, 0);

        RaycastHit2D frontHit = Physics2D.Raycast(frontRayOrigin, Vector2.down, 0.55f, groundLayer);
        RaycastHit2D backHit = Physics2D.Raycast(backRayOrigin, Vector2.down, 0.55f, groundLayer);

        return frontHit.collider != null || backHit.collider != null;
    }

    protected virtual void HandleMovement() 
    {
        // Cube jump
        // Ship burst
        // ...
        // wave
    }

    public virtual void Die()
    {
        rb.linearVelocity = Vector2.zero;
        Invoke(nameof(Respawn), 0f);
    }

    protected virtual void Respawn()
    {
        transform.position = startPosition;
        rb.linearVelocity = Vector2.zero;
    }

    protected void DettachParticleSystem()
    {
        _particleSystem.Stop();
        _particleSystem.transform.SetParent(_particleSystemParent, true);
    }

    protected void ReattachParticleSystem()
    {
        _particleSystem.Play();
        _particleSystem.transform.SetParent(transform, true);
        _particleSystem.transform.localPosition = new Vector3(0, -0.5f, 0); 
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(rb.position + new Vector2(0.5f,0), rb.position + new Vector2(0.5f,0) + Vector2.down * 0.55f);
        Gizmos.DrawLine(rb.position - new Vector2(0.47f,0), rb.position - new Vector2(0.47f,0) + Vector2.down * 0.55f);
    }
}