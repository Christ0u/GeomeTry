using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    private GameManager _gameManager;
    private bool isPaused = false;

    private void Awake()
    {
        _gameManager = GameManager.Instance;
        if (_gameManager == null)
        {
            Debug.LogError("GameManager.Instance est null !");
        }
        
        // Désactiver le menu pause au démarrage
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
        else
        {
            Debug.LogError("Pause Menu UI non assigné dans l'inspecteur !");
        }
    }

    private void Start()
    {
        // Essayer à nouveau d'obtenir GameManager s'il était null dans Awake
        if (_gameManager == null)
        {
            _gameManager = GameManager.Instance;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (_gameManager == null)
        {
            _gameManager = GameManager.Instance;
            if (_gameManager == null)
            {
                Debug.LogError("GameManager toujours inaccessible lors de la pause!");
                // Comportement de secours quand GameManager n'est pas disponible
                isPaused = !isPaused;
                pauseMenuUI.SetActive(isPaused);
                Time.timeScale = isPaused ? 0f : 1f;
                return;
            }
        }
        
        // Comportement normal quand GameManager est disponible
        if (_gameManager.PlayMode)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    public void PauseGame()
    {
        if (_gameManager != null)
        {
            _gameManager.PlayMode = false;
        }
        
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        if (_gameManager != null)
        {
            _gameManager.PlayMode = true;
        }
        
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
}