using UnityEngine;

public class Cube : Character
{    
    public override void onClick()
    {
        Jump();
    }

    private void Jump()
    {
        if (CheckGrounded())
        {
            DettachParticleSystem();
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);
            _isGrounded = false;
        }
    }

    protected override void HandleMovement()
    {
        RotateSprite();
    }

    private void RotateSprite()
    {
        if (!CheckGrounded())
        {
            // * Time.deltaTime pour que la rotation soit indépendante du framerate
            float rotationAmount = rotationSpeed * Time.deltaTime;
            spriteRenderer.transform.Rotate(0, 0, -rotationAmount);
        }
        else // il faudrait un petit timer qu'on reset si on detect un saut pour pas snap tout de suite.
        {
            float currentRotation = spriteRenderer.transform.eulerAngles.z;
            float snappedRotation = Mathf.Round(currentRotation / 90) * 90;
            float smoothedRotation = Mathf.LerpAngle(currentRotation, snappedRotation, 0.25f);

            spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, smoothedRotation);
        }
    }
}