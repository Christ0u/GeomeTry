using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditorManager : MonoBehaviour
{
    private DevUtils _devUtils;

    [SerializeField] private GameObject backgroundPrefab;
    private Image _backgroundImage;

    private void Start()
    {
        _devUtils = GetComponent<DevUtils>();
        SetLevel();
    }

    #region CreateMapComponents
    private void SetLevel()
    {
        // Recherche du Canvas dans la scène
        Canvas canvas = GameObject.FindFirstObjectByType<Canvas>();

        if (backgroundPrefab != null)
        {
            if (canvas != null)
            {
                SetBackground(canvas, _devUtils.GenerateRandomInt(1, 3), _devUtils.GenerateRandomColor());
            }
            else
            {
                Debug.LogError("Erreur de configuration : le Canvas n'est pas défini.");
            }
        }
        else
        {
            Debug.LogError("Erreur de configuration : le prefab de fond n'est pas défini.");
        }
    }
    #endregion

    // Gestion des fonds
    #region Manage Background

    /// <summary>
    /// Réccupère les données nécessaires pour le placement et la taille du fond.
    /// </summary>
    private (float canvasWidth, float canvasHeight, float bgWidth, float bgHeight) GetBackgroundSizes(Canvas canvas, GameObject backgroundObject, int scale = 5)
    {
        RectTransform backgroundRectTransform = backgroundObject.GetComponent<RectTransform>();
        RectTransform canvasRectTransform = canvas.GetComponent<RectTransform>();

        // Dimensions du Canvas
        float canvasWidth = canvasRectTransform.rect.width;
        float canvasHeight = canvasRectTransform.rect.height;

        // Taille du fond
        float bgWidth = backgroundRectTransform.rect.width * scale;
        float bgHeight = backgroundRectTransform.rect.height * scale;

        return (canvasWidth, canvasHeight, bgWidth, bgHeight);
    }

    /// <summary>
    /// Définit les motifs et la couleur du fond.
    /// </summary>
    private void SetBackgroundSettings(GameObject backgroundObject, int backgroundNumber, Color color)
    {
        _backgroundImage = backgroundObject.GetComponent<Image>();

        if (_backgroundImage == null)
        {
            Debug.LogWarning("Pas de fond trouvé !");
            return;
        }

        string imagePath = "UI/Backgrounds/Background_" + backgroundNumber;
        Sprite newSprite = Resources.Load<Sprite>(imagePath);
        if (newSprite != null)
        {
            _backgroundImage.sprite = newSprite;
            Debug.Log($"Image {imagePath} chargée avec succès.");
        }
        else
        {
            Debug.LogWarning($"'{imagePath}' n'a pas été trouvé. Couleur par défaut appliquée.");
            _backgroundImage.color = Color.white;
        }

        _backgroundImage.color = color;
    }

    /// <summary>
    /// Positionne le fond sur le Canvas.
    /// </summary>
    private void SetBackgroundPosition(Canvas canvas, GameObject backgroundObject, int xIndex, int yIndex, float bgWidth, float bgHeight)
    {
        // Remplissage du canvas avec le fond
        backgroundObject.transform.SetParent(canvas.transform, false);
        // Placer le fond en arrière-plan
        backgroundObject.transform.SetSiblingIndex(0);

        RectTransform tileRect = backgroundObject.GetComponent<RectTransform>();

        // Positionner chaque tuile en fonction de sa position dans la grille
        tileRect.anchorMin = new Vector2(0, 0);
        tileRect.anchorMax = new Vector2(0, 0);
        tileRect.pivot = new Vector2(0, 0);

        // Calculer la position de la tuile
        float xPos = xIndex * bgWidth;
        float yPos = yIndex * bgHeight;

        tileRect.anchoredPosition = new Vector2(xPos, yPos);
        tileRect.sizeDelta = new Vector2(bgWidth, bgHeight);
    }

    /// <summary>
    /// Crée et place des GameObjects de fond sur le Canvas.
    /// </summary>
    private void SetBackground(Canvas canvas, int backgroundNumber, Color color)
    {
        (float canvasWidth, float canvasHeight, float bgWidth, float bgHeight) = GetBackgroundSizes(canvas, backgroundPrefab);

        // Danaé : Quentin -> on peux peut-être se servir de ce qui suit pour 
        // remplir des zones de la map vide avec des tuiles comme tu m'avais demandé ?

        // Calcul pour recouvrir tout le fond
        int horizontalTiles = Mathf.CeilToInt(canvasWidth / bgWidth);
        int verticalTiles = Mathf.CeilToInt(canvasHeight / bgHeight);

        // Application du fond
        for (int i = 0; i < horizontalTiles; i++)
        {
            for (int j = 0; j < verticalTiles; j++)
            {
                GameObject backgroundTile = Instantiate(backgroundPrefab);
                SetBackgroundSettings(backgroundTile, backgroundNumber, color);
                SetBackgroundPosition(canvas, backgroundTile, i, j, bgWidth, bgHeight);
            }
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