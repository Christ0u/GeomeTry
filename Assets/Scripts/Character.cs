using UnityEngine;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour
{
    public float currentSpeed;
    
    private const float DefaultSpeed = 8.0f;
    private const float JumpForce = 13.0f;
    
    private bool _isGrounded;
    
    [SerializeField] GameManager gameManager;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private ParticleSystem _particleSystem;

    private Vector3 startPosition;

    void Start()
    {
        _isGrounded = true; 
        startPosition = transform.position;
        currentSpeed = DefaultSpeed;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }
    
    void Update()
    {
        if (!gameManager.playMode) return; // Si le jeu est pas lancé

        transform.Translate(new Vector2(currentSpeed * Time.deltaTime, 0));
        CheckGrounded();
        
        // si on veut implementer la modification de gravitée c'est ici.

        RotateSprite();
    }

    private void CheckGrounded()
    {
        Vector2 frontRayOrigin = rb.position + new Vector2(0.5f, 0);
        Vector2 backRayOrigin = rb.position - new Vector2(0.47f, 0);

        RaycastHit2D frontHit = Physics2D.Raycast(frontRayOrigin, Vector2.down, 0.55f, groundLayer);
        RaycastHit2D backHit = Physics2D.Raycast(backRayOrigin, Vector2.down, 0.55f, groundLayer);

        _isGrounded = frontHit.collider != null || backHit.collider != null;
    }

    private void Jump()
    {
        if (_isGrounded) {
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, JumpForce);
            _isGrounded = false;  
        }
    }

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
        rb.linearVelocity = Vector2.zero;
        Invoke(nameof(Respawn), 0f);
    }

    private void Respawn()
    {
        transform.position = startPosition;
        rb.linearVelocity = Vector2.zero;
    }

    // Debug stuff
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(rb.position + new Vector2(0.5f,0), rb.position  + new Vector2(0.5f,0) + Vector2.down * 0.55f);
        Gizmos.DrawLine(rb.position -  new Vector2(0.47f,0), rb.position -  new Vector2(0.47f,0) + Vector2.down * 0.55f);

    }
}