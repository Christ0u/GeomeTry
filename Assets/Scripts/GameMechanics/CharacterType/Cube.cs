using UnityEngine;

public class Cube : Character
{
    protected override void Update()
    {
        // Gestion du système de particule
        if (!CheckGrounded())
        {
            DettachParticleSystem();
        }
        else
        {
            ReattachParticleSystem();
        }

        base.Update();
    }

    public override void onClick()
    {
        if (keyPressed)
        {
            Jump();
        }
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
        else
        {
            float currentRotation = spriteRenderer.transform.eulerAngles.z;
            float snappedRotation = Mathf.Round(currentRotation / 90) * 90;
            float smoothedRotation = Mathf.LerpAngle(currentRotation, snappedRotation, 0.25f);

            spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, smoothedRotation);
        }
    }
}