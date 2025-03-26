using System;
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
        
        // Modifier l'appel plus tard
        SetBackground(canvas, _devUtils.GenerateRandomInt(1, _devUtils.backgroundQuantity), _devUtils.GenerateRandomColor());
        // TODO
        // SetGround();
        // SetTiles();
        // SetMusic();
    }

    #region Manage Background
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
    #endregion

    #region Manage Ground
    // TODO
    // Permettre le placement du sol ici.. ?
    // private void SetGround(Canvas canvas);
    #endregion

    #region Manage Tiles
    // TODO
    // Permettre le placement des tuiles, obstacles ici.. ?
    // private void SetTiles(Canvas canvas);
    #endregion

    #region Manage Audio
    // TODO
    // Permettre le placement des sons ici.. ?
    // private void SetMusic();
    #endregion

    #region Level Settings
    // TODO
    // private void SetLevelName();
    // private void SetLevelNumber();
    // private void SetDifficulty();
    // ...
    // // Permettre de sauvegarder le niveau ici.. ?
    // private void SaveLevel();
    #endregion

}