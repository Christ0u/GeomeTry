using UnityEngine;
using System.Collections.Generic;

public class JumpPad : MonoBehaviour
{
    private static Dictionary<string, float> _jumpPadForces = new Dictionary<string, float>
    {
        { "PinkJumpPad",    12.5f },
        { "YellowJumpPad",  20.0f },
        { "RedJumpPad",     27.0f }
    };

    [SerializeField] Character player;

    private float getJumpForce()
    {
        // Récupération du JumpPad actuel
        string currentJumpPad = transform.parent.name.Replace("(Clone)", "").Trim();
        
        if (_jumpPadForces.TryGetValue(currentJumpPad, out float force))
        {
            return force;
        }
        return 0f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Character character = other.GetComponent<Character>();

        if (character != null)
        {
            Rigidbody2D rb = character.GetComponent<Rigidbody2D>();

            // Application de la force de saut
            float jumpForce = getJumpForce();
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }
}