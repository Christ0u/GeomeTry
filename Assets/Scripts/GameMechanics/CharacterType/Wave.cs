using UnityEngine;

public class Wave : Character 
{
    private float _modifier = 1; // 1 ou -1

    protected override void Start()
    {
        base.Start();
        rb.gravityScale = 0f;
    }

    protected override void FixedUpdate()
    {

        Vector2 mouvement = currentSpeed * Time.deltaTime * new Vector2(1, 1);
        mouvement.y *= _modifier;
        transform.Translate(mouvement);

        spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, -90 + 45 * Mathf.Sin(_modifier * -5f)); // -90 = correction du sprite
    }

    protected override void Update()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)) {
            keyPressed = true;
        } else {
            keyPressed = false;
        }
        onClick();
    }

    public override void onClick()
    {
        if (keyPressed)
        {
            Debug.Log("Switching direction");
            _modifier = 1;
        } else {
            _modifier = -1;
        }
    }
    
}
