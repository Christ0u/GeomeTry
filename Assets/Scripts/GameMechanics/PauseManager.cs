using UnityEngine;
using System.Collections.Generic;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;

    private GameManager _gameManager;
    private bool isPaused = false;
    private AudioSource audioSource;
    private Dictionary<AudioSource, float> audioTimePositions = new Dictionary<AudioSource, float>();

    private void Awake()
    {
        _gameManager = GameManager.Instance;
        if (_gameManager == null)
        {
            Debug.LogError("GameManager.Instance est null !");
        }

        // Rechercher le menu pause s'il n'est pas assigné
        if (pauseMenuUI == null)
        {
            pauseMenuUI = GameObject.FindWithTag("PauseMenu");
            if (pauseMenuUI == null)
            {
                Debug.LogError("Pause Menu UI non trouvé ! Assurez-vous qu'il existe dans la scène et qu'il est taggé comme 'PauseMenu'.");
            }
        }

        // Rechercher la source audio
        audioSource = FindFirstObjectByType<AudioSource>();

        // Menu pause est désactivé au démarrage
        isPaused = false;
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
        else
        {
            Debug.LogError("Pause Menu UI non assigné dans l'inspecteur !");
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Debug.Log("Touche Echap pressée - Basculement pause");
            TogglePause();
        }

        // Surveiller l'état du menu
        if (pauseMenuUI != null)
        {
            //Debug.Log($"État du menu pause: {(pauseMenuUI.activeSelf ? "visible" : "caché")}, isPaused: {isPaused}");
        }
    }

    public void TogglePause()
    {
        if (_gameManager == null)
        {
            _gameManager = GameManager.Instance;
        }

        // Inverser l'état de pause
        isPaused = !isPaused;

        if (isPaused)
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
        // Mettre le jeu en pause
        if (_gameManager != null)
        {
            _gameManager.PlayMode = false;
        }

        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        // Mettre en pause toutes les sources audio
        if (audioSource != null && audioSource.isPlaying)
        {
            audioTimePositions[audioSource] = audioSource.time;
            audioSource.Pause();
        }
    }

    public void ResumeGame()
    {
        // Reprendre le jeu
        if (_gameManager != null)
        {
            _gameManager.PlayMode = true;
        }

        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.5f;
        isPaused = false;

        // Vérifier si la source audio existe et si elle a été mise en pause
        if (audioSource != null && audioTimePositions.ContainsKey(audioSource))
        {
            audioSource.time = audioTimePositions[audioSource];
            audioSource.Play();
            audioTimePositions.Clear();
        }
    }
}