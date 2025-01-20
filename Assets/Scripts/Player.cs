using UnityEngine;

public class Player : MonoBehaviour
{
    private const float speed = 8.0f;
    private const float jumpForce = 13.0f;
    private const float jumpDuration = 0f; // A modifier plus tard
    [SerializeField] Rigidbody2D rb;

    private bool _isGrounded;
    [SerializeField] Transform groundCheckObject;
    [SerializeField] private LayerMask layerMask;

    void Update()
    {
        if (!Global.playMode) return;

        _isGrounded = Physics2D.OverlapCircle(groundCheckObject.position, 0.1f, layerMask);

        transform.Translate(new Vector2(speed * Time.deltaTime, 0));
        Jump();
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);
            // transform.Rotate(new Vector3(0, 0, -360) * jumpDuration * Time.deltaTime);
        }
    }


}
