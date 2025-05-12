using UnityEngine;
using UnityEngine.SceneManagement;

public class EndZone : MonoBehaviour
{
    [SerializeField] Character player;
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;
        if (_gameManager == null)
        {
            Debug.LogError("GameManager.Instance est null dans EndZone!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Character character = other.GetComponent<Character>();

        if (character != null)
        {
            if (_gameManager != null)
            {
                _gameManager.PlayMode = false;
            }
            
            Debug.Log("You win!");
            
            Destroy(character.gameObject);
            
            Invoke("LoadLevelSelectionMenu", 0.5f); // 2eme argument = temps avant de charger la scène
        }
    }
    
    private void LoadLevelSelectionMenu()
    {
        if (_gameManager != null)
        {
            StartCoroutine(_gameManager.LoadScene("Level Selection"));
        }
        else
        {
            // Fallback si GameManager n'est pas disponible
            SceneManager.LoadScene("Level Selection");
        }
    }
}