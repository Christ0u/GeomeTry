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
    [SerializeField] Transform groundCheckObject; // On va laisser mon groundCheck on m'a dit que c'est bien vouala. (Source sûre => étudiant de l'Enjmin)
    [SerializeField] private LayerMask layerMask;
    
    [SerializeField] private ParticleSystem _particleSystem;

    void Start()
    {
        currentSpeed = DefaultSpeed;
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
        }
    }
}
