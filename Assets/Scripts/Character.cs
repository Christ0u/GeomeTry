using UnityEngine;

public class Character : MonoBehaviour
{
    public float currentSpeed;
    
    private const float DefaultSpeed = 8.0f;
    private const float JumpForce = 13.0f;
    
    private bool _isGrounded;
    private bool _isAlive;
    
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform groundCheckObject;
    [SerializeField] private LayerMask layerMask;

    void Start()
    {
        _isAlive = true;
        currentSpeed = DefaultSpeed;
    }
    
    void Update()
    {
        if (!GameManager.playMode) return; // Si le jeu est pas lancé

        _isGrounded = Physics2D.OverlapCircle(groundCheckObject.position, 0.1f, layerMask);

        transform.Translate(new Vector2(currentSpeed * Time.deltaTime, 0));
        Jump();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            _isAlive = false;
            Debug.Log("Le joueur est mort lol");
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, JumpForce);
        }
    }


}
