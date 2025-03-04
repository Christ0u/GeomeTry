using UnityEngine;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour
{
    public float currentSpeed;
    
    private const float DefaultSpeed = 8.0f;
    private const float JumpForce = 13.0f;
    
    private bool _isGrounded;
    private bool _isDead;
    
    [SerializeField] GameManager gameManager;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private ParticleSystem _particleSystem;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
        currentSpeed = DefaultSpeed;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }
    
    void Update()
    {
        if (!gameManager.playMode) return; // Si le jeu est pas lancé

        _isGrounded = Physics2D.OverlapCircle(groundCheckObject.position, 0.1f, layerMask);

        transform.Translate(new Vector2(currentSpeed * Time.deltaTime, 0));
        Jump();
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, JumpForce);

    private void RotateSprite()
    {
        if (!_isGrounded)
        {
            // * Time.deltaTime pour que la rotation soit indépendante du framerate
            float rotationAmount = RotationSpeed * Time.deltaTime; 
            spriteRenderer.transform.Rotate(0, 0, -rotationAmount);

        } else { // il faudrait un petit timer qu'on reset si on detect un saut pour pas snap tout de suite.
        
            float currentRotation = spriteRenderer.transform.eulerAngles.z;
            float snappedRotation = Mathf.Round(currentRotation / 90) * 90;
            float smoothedRotation = Mathf.LerpAngle(currentRotation, snappedRotation, 0.25f);

            spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, smoothedRotation);
        }
    }

    public void Die()
    {
        _isDead = true;
        rb.linearVelocity = Vector2.zero;
        Invoke(nameof(Respawn), 0f);
    }

    void Respawn()
    {
        transform.position = startPosition;
        rb.linearVelocity = Vector2.zero;
        _isDead = false;
    }
}
