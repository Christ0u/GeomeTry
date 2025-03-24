using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{   
    private GameManager _gameManager;
    private DevUtils _devUtils;
    
    [SerializeField] private GameObject backgroundPrefab;
    private Image _backgroundImage;
    private Canvas _canvas;
    
    private void Start()
    {
        _gameManager = GameManager.Instance;

        _devUtils = GetComponent<DevUtils>();
        _canvas = FindFirstObjectByType<Canvas>();

        if(SceneManager.GetActiveScene().name == "Main Menu")
        {
            SetBackground(_canvas, _devUtils.GenerateRandomInt(1,3), _devUtils.GenerateRandomColor());
        }

    }

    #region Manage Buttons Actions
    
    /// <summary>
    /// Permet de quitter le jeu.
    /// </summary>
    public void OnClickQuitGame()
    {
        #if UNITY_EDITOR
        Debug.Log("Appel à la fonction OnClickQuitGame - Ne marche que en build.");
        #else
        Application.Quit();
        #endif
    }
    
    /// <summary>
    /// Permet de charger une scène.
    /// </summary>
    /// <param name="sceneName">Nom de la scène à charger.</param>
    public void OnClickSwitchSceneButton(string sceneName)
    {
        Debug.Log("Chargement de la scène " + sceneName);
        StartCoroutine(_gameManager.LoadScene(sceneName));
        Debug.Log("Fin chargement de la scène " + sceneName);
    }
    
    /// <summary>
    /// Permet de lancer une partie.
    /// </summary>
    public void OnClickLevelButton()
    {
        StartCoroutine(_gameManager.LoadScene("Level"));
        // LaunchLevel(Level);
        _gameManager.PlayMode = true; // -> A déplacer dans le Launch
    }
    #endregion

    #region Manage Backgrounds

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
    private void SetBackgroundSettings(GameObject backgroundObject, int backgroundNumber, Color backgroundColor)
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

        _backgroundImage.color = backgroundColor;
    }

    /// <summary>
    /// Positionne le fond dans le panel.
    /// </summary>
    private void SetBackgroundPosition(GameObject backgroundContainer,GameObject backgroundObject, int xIndex, int yIndex, float bgWidth, float bgHeight)
    {
        RectTransform tileRect = backgroundObject.GetComponent<RectTransform>();

        // Positionner chaque tuile en fonction de sa position dans la grille
        tileRect.anchorMin = new Vector2(0, 0);
        tileRect.anchorMax = new Vector2(0, 0);
        tileRect.pivot = new Vector2(0, 0);

        // Calculer la position de la tuile dans le panel
        float xPos = xIndex * bgWidth;
        float yPos = yIndex * bgHeight;

        tileRect.anchoredPosition = new Vector2(xPos, yPos);
        tileRect.sizeDelta = new Vector2(bgWidth, bgHeight);
        
        backgroundObject.transform.SetParent(backgroundContainer.transform, false);
    }

    /// <summary>
    /// Crée et place des GameObjects de fond dans un panneau du Canvas.
    /// </summary>
    public void SetBackground(Canvas canvas, int backgroundNumber, Color backgroundColor)
    {
        if (backgroundPrefab != null)
        {
            (float canvasWidth, float canvasHeight, float bgWidth, float bgHeight) = GetBackgroundSizes(canvas, backgroundPrefab);

            // Créer un conteneur pour contenir les tuiles du fond
            GameObject backgroundContainer = new GameObject("Background");
            RectTransform backgroundContainerRectTransform = backgroundContainer.AddComponent<RectTransform>();
            
            backgroundContainer.transform.SetParent(canvas.transform, false);
            backgroundContainer.transform.SetSiblingIndex(0); // Le "0" signifie que l'objet sera placé au tout début de la hiérarchie

            // Remplir toute la surface du conteneur
            backgroundContainerRectTransform.anchorMin = new Vector2(0, 0);
            backgroundContainerRectTransform.anchorMax = new Vector2(1, 1);
            backgroundContainerRectTransform.pivot = new Vector2(0.5f, 0.5f);
            backgroundContainerRectTransform.sizeDelta = new Vector2(0, 0); // Valeurs de taille pour remplir l'espace

            // Calculer le nombre de tuiles nécessaires pour couvrir tout le fond
            int horizontalTiles = Mathf.CeilToInt(canvasWidth / bgWidth);
            int verticalTiles = Mathf.CeilToInt(canvasHeight / bgHeight);

            // Application du fond à chaque position dans la grille
            for (int i = 0; i < horizontalTiles; i++)
            {
                for (int j = 0; j < verticalTiles; j++)
                {
                    GameObject backgroundTile = Instantiate(backgroundPrefab);
                    SetBackgroundSettings(backgroundTile, backgroundNumber, backgroundColor);
                    SetBackgroundPosition(backgroundContainer, backgroundTile, i, j, bgWidth, bgHeight);
                }
            }
        }
        else
        {
            Debug.LogError("Erreur de configuration : le prefab de fond n'est pas défini.");
        }
    }
    #endregion
}