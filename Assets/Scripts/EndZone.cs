using UnityEngine;

public class EndZone : MonoBehaviour
{
    [SerializeField] Character player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Character character = other.GetComponent<Character>();

        if (character != null)

        {
            Destroy(character.gameObject);
            Debug.Log("You win!");
        }
    }
}