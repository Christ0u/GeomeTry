using UnityEngine;

public class Ship : Character
{
    //Debug : SerializeField pour modifier les valeurs dans l'éditeur
    [SerializeField] private float gravityScale = 2.5f;
    [SerializeField] private float maxVelocityY = 10.0f;

    private bool isFlying = false;

    protected override void Start()
    {
        base.Start();
        rb.gravityScale = gravityScale;
    }

    public override void onClick()
    {
        isFlying = keyPressed;
    }

    protected override void HandleMovement()
    {
        if (!isAlive) return; // Si le vaisseau est pas vivant
        
        if (isFlying)
        {
            rb.gravityScale = -Mathf.Abs(gravityScale);
            ReattachParticleSystem(); // Détache les particules si nécessaire
        }
        else
        {
            // Gravité normale pour descendre
            rb.gravityScale = Mathf.Abs(gravityScale);
            DettachParticleSystem(); // Détache les particules si nécessaire
        }

        // on limite la vitesse en y sinon ça part en cacahuète
        if (rb.linearVelocityY > maxVelocityY)
            rb.linearVelocityY = maxVelocityY;
        else if (rb.linearVelocityY < -maxVelocityY)
            rb.linearVelocityY = -maxVelocityY;

        // on calcul l'angle de rotation du vaisseau pour lui appliquer ensuite au spriteRenderer ainsi qu'au particule system (+180°)
        float angle = Mathf.Atan2(rb.linearVelocityY, 7) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Clamp(angle, -40f, 40f));

        spriteRenderer.transform.rotation = rotation;
        _particleSystem.transform.rotation = Quaternion.Euler(0, 0, rotation.eulerAngles.z + 180);

        // position du particule system
        Vector3 offset = rotation * new Vector3(-0.5f, 0, -1);
        _particleSystem.transform.position = spriteRenderer.transform.position + offset;

    }

    protected override bool CheckGrounded()
    {
        return false;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}