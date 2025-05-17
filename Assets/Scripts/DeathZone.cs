using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField] Character player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Character character = other.GetComponent<Character>();

        if (character != null)

        {
            character.Die();
        }
    }
}
