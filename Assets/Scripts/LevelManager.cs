using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private DevUtils _devUtils;
    private UIManager _uiManager;

    private void Awake()
    {
        _devUtils = GetComponent<DevUtils>();
        _uiManager = GetComponent<UIManager>();
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Level")
        {
            SetLevel();
        }
    }

    private void SetLevel()
    {
        // Recherche du Canvas dans la scène
        Canvas canvas = FindFirstObjectByType<Canvas>();

        SetBackground(canvas, _devUtils.GenerateRandomInt(1, _devUtils.backgroundQuantity), _devUtils.GenerateRandomColor());
    }

    private void SetBackground(Canvas canvas, int backgroundNumber, Color backgroundColor)
    {
        if (canvas != null)
        {
            _uiManager.SetBackground(canvas, backgroundNumber, backgroundColor);
        }
        else
        {
            Debug.LogError("Erreur de configuration : le Canvas n'est pas défini.");
        }
    }
}