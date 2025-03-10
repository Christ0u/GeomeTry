using UnityEngine;

public class Ship : Character
{
    //Debug : SerializeField pour modifier les valeurs dans l'éditeur
    [SerializeField] private float flyForce = 35.0f;  
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

    // première essai pour le vol, remplacé par onClick, je le laisse pour l'instant ça peut être utile
    // protected override void Update()
    // {
    //     base.Update();
    //     isFlying = Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0);
    // }

    protected override void HandleMovement()
    {
        if (isFlying)
        {
            // on gère les particules ici j'ai pas reussi a faire mieux ... pas optimal car dans HandleMovement...
            rb.AddForce(Vector2.up * flyForce, ForceMode2D.Force);
            ReattachParticleSystem();
            rb.linearVelocity = new Vector2(rb.linearVelocityX, Mathf.Min(rb.linearVelocityY + 0.5f, maxVelocityY));
        } else {
            DettachParticleSystem();
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

            
    }

    protected override bool CheckGrounded()
    {
        return false;
    }
    
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        
        if (!isFlying && rb.linearVelocityY < 0)
        {
            rb.AddForce(Vector2.down * 5f, ForceMode2D.Force);
        }
    }
}