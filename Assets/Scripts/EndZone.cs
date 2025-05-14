using UnityEngine;
using UnityEngine.SceneManagement;

public class EndZone : MonoBehaviour
{
    [SerializeField] private Character player;
    private GameObject completedLevelCanvas; // Référence au Canvas
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;
        if (_gameManager == null)
        {
            Debug.LogError("GameManager.Instance est null dans EndZone!");
        }

        // Recherche du Canvas dans la scène par son nom ou sous MainCamera
        completedLevelCanvas = GameObject.Find("CanvasCompletedLevel");
        if (completedLevelCanvas == null)
        {
            Camera mainCamera = GameObject.FindGameObjectWithTag("MainCamera")?.GetComponent<Camera>();
            Transform mainCameraTransform = mainCamera?.transform;
            if (mainCameraTransform != null)
            {
                completedLevelCanvas = mainCameraTransform.Find("CanvasCompletedLevel")?.gameObject;
            }
        }

        if (completedLevelCanvas == null)
        {
            Debug.LogError("Le Canvas 'CanvasCompletedLevel' est introuvable dans la scène ou sous MainCamera.");
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

            //Debug.Log("You win!");

            // Activer le Canvas
            if (completedLevelCanvas != null)
            {
                completedLevelCanvas.SetActive(true);
            }
            else
            {
                Debug.LogError("Le Canvas 'CompletedLevelCanvas' est null !");
            }

            Destroy(character.gameObject);

            Invoke("LoadLevelSelectionMenu", 5f); // 2eme argument = temps avant de charger la scène
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