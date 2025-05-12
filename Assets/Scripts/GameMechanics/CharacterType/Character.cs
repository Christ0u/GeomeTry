using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Character : MonoBehaviour
{
    protected GameManager _gameManager;
    public float currentSpeed;
    protected const float DefaultSpeed = 6.5f;
    protected const float DefaultGravityScale = 4.0f;
    protected float jumpForce = 13.5f;
    protected float rotationSpeed = 280.0f;
    protected bool _isGrounded;
    public const float InitialPosition = -5f;
    public bool isAlive;
    public int jumpCount { get; protected set; } = 0;

    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected ParticleSystem _particleSystem;
    [SerializeField] protected Animator animator;
    [SerializeField] protected AudioSource deathSfx;

    protected Vector3 respawnPosition;
    protected bool keyPressed = false;

    protected virtual void Start()
    {
        isAlive = true;

        Time.timeScale = 1.5f; // Game speed
        _isGrounded = true;
        _gameManager = GameManager.Instance;
        respawnPosition = new Vector3(InitialPosition, 0, 0);
        currentSpeed = DefaultSpeed;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
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
        if (!isAlive) return; // Si le personnage est pas vivant

        if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
        {
            keyPressed = true;
        }
        else
        {
            keyPressed = false;
        }
        onClick();
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
        isAlive = false;

        deathSfx.Play();

        currentSpeed = 0.0f;
        rb.linearVelocity = Vector2.zero;

        // Animation de mort du personnage
        animator.Play("DeathAnimation");
        // Désactivation de la gravité, du sprite et du système de particule
        rb.gravityScale = 0.0f;
        spriteRenderer.enabled = false;
        if (_particleSystem != null)
        {
            _particleSystem.gameObject.SetActive(false);
        }

        // Mise à jour des données de saut
        DataManager.Instance.UpdateTotalJumps(jumpCount);


        // Réinitialisation du compteur de sauts
        jumpCount = 0;

        // Relance le jeu après 1 seconde
        Invoke(nameof(Respawn), 1.0f);
    }

    protected virtual void Respawn()
    {
        isAlive = true;

        transform.position = respawnPosition;
        currentSpeed = DefaultSpeed;
        rb.linearVelocity = Vector2.zero;

        // Réactivation de la gravité, du sprite et du système de particule
        rb.gravityScale = DefaultGravityScale;
        spriteRenderer.enabled = true;
        if (_particleSystem != null)
        {
            _particleSystem.gameObject.SetActive(true);
        }

    }

    protected void DettachParticleSystem()
    {
        _particleSystem.Stop(true);
    }

    protected void ReattachParticleSystem()
    {
        _particleSystem.Play();
        _particleSystem.transform.localPosition = new Vector3(0, -0.5f, 0);
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(rb.position + new Vector2(0.5f, 0), rb.position + new Vector2(0.5f, 0) + Vector2.down * 0.55f);
        Gizmos.DrawLine(rb.position - new Vector2(0.47f, 0), rb.position - new Vector2(0.47f, 0) + Vector2.down * 0.55f);
    }
}