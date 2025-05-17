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

    private void Awake()
    {
        _gameManager = GameManager.Instance;

        if (_gameManager == null)
        {
            Debug.LogError("GameManager.Instance est null !");
        }

        _devUtils = GetComponent<DevUtils>();
        _canvas = FindFirstObjectByType<Canvas>();

        // S'abonner à l'événement de changement de scène
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        ApplyMenuBackground();
    }

    private void OnDestroy()
    {
        // Se désabonner de l'événement pour éviter les erreurs
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplyMenuBackground();  // Réappliquer le fond après le chargement de la scène
    }

    #region Manage Buttons Actions

    /// <summary>
    /// Permet de quitter le jeu.
    /// </summary>
    public void OnClickQuitGame()
    {
#if UNITY_EDITOR
#else
        Application.Quit();
#endif
    }

    /// <summary>
    /// Lance une vidéo YouTube.
    /// </summary>
    public void OnClickYouTubeButton()
    {
        Application.OpenURL("https://www.youtube.com/watch?v=SGbQ34Gm_UU"); // Oui c'est un rickroll.
    }

    /// <summary>
    /// Permet de charger une scène.
    /// </summary>
    /// <param name="sceneName">Nom de la scène à charger.</param>
    public void OnClickSwitchSceneButton(string sceneName)
    {
        if (!PlayerPrefs.HasKey("previousScene"))
        {
            PlayerPrefs.SetString("previousScene", SceneManager.GetActiveScene().name);
        }

        // Si _gameManager est null, le récupérer
        if (_gameManager == null)
        {
            _gameManager = GameManager.Instance;

            if (_gameManager == null)
            {
                Debug.LogError("GameManager non disponible. Chargement de scène impossible.");
                // SceneManager directement comme fallback
                SceneManager.LoadScene(sceneName);
                return;
            }
        }

        StartCoroutine(_gameManager.LoadScene(sceneName));
    }

    /// <summary>
    /// Permet de charger la scène précédente.
    /// </summary>
    public void OnClickGoBackButton()
    {
        if (!PlayerPrefs.HasKey("previousScene"))
        {
            Debug.LogError("La variable previousScene est vide ou non initialisée !");
            return;
        }

        SceneManager.LoadScene(PlayerPrefs.GetString("previousScene"));
    }

    /// <summary>
    /// Permet de lancer une partie.
    /// </summary>
    public void OnClickLevelButton(Level level, TextAsset levelFile)
    {
        if (_gameManager == null)
        {
            _gameManager = GameManager.Instance;

            if (_gameManager == null)
            {
                Debug.LogError("GameManager non trouvé ! Impossible de lancer un niveau.");
                return;
            }
        }

        _gameManager.LoadLevel(levelFile);
    }
    #endregion

    #region Manage Backgrounds

    /// <summary>
    /// Applique le background sur le canvas de la scène.
    /// </summary>
    private void ApplyMenuBackground()
    {
        if (_canvas == null)
        {
            _canvas = FindFirstObjectByType<Canvas>();
            if (_canvas == null)
            {
                Debug.LogError("Canvas introuvable dans la scène.");
                return;
            }
        }

        if (_canvas != null && _devUtils != null && !SceneManager.GetActiveScene().name.Equals("Level"))
        {
            SetBackground(_canvas, _devUtils.backgroundIndex, _devUtils.backgroundMenuColor);
        }

        if (SceneManager.GetActiveScene().name.Equals("Level"))
        {
            SetBackground(_canvas, _devUtils.GenerateRandomInt(1, _devUtils.backgroundQuantity), _devUtils.GenerateRandomColor());
        }
    }

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
    private void SetBackgroundPosition(GameObject backgroundContainer, GameObject backgroundObject, int xIndex, int yIndex, float bgWidth, float bgHeight)
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
            // Supprimer les backgrounds existants pour éviter l'accumulation
            GameObject existingBackground = GameObject.Find("Background");
            if (existingBackground != null)
                Destroy(existingBackground);

            (float canvasWidth, float canvasHeight, float bgWidth, float bgHeight) = GetBackgroundSizes(canvas, backgroundPrefab);

            // Créer un conteneur pour contenir les tuiles du fond
            GameObject backgroundContainer = new GameObject("Background");
            RectTransform backgroundContainerRectTransform = backgroundContainer.AddComponent<RectTransform>();

            backgroundContainer.transform.SetParent(canvas.transform, false);

            // Définir explicitement la position Z pour être derrière les autres éléments
            Vector3 position = backgroundContainerRectTransform.localPosition;
            position.z = 100;
            backgroundContainerRectTransform.localPosition = position;

            // Placer en premier dans la hiérarchie
            backgroundContainer.transform.SetAsFirstSibling();

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