using UnityEngine;
using System.Collections.Generic;

public class JumpOrb : MonoBehaviour
{
    private static Dictionary<string, float> _jumpOrbForces = new Dictionary<string, float>
    {
        { "PinkJumpOrb",    12.5f },
        { "YellowJumpOrb",  20.0f },
        { "RedJumpOrb",     27.0f }
    };

    private float getJumpForce()
    {
        // Récupération du JumpPad actuel
        string currentJumpOrb = transform.parent.name.Replace("(Clone)", "").Trim();

        if (_jumpOrbForces.TryGetValue(currentJumpOrb, out float force))
        {
            return force;
        }
        return 0f;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
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
}