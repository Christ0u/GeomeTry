using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditorManager : MonoBehaviour
{
    [SerializeField] private GameObject backgroundPrefab;
    private Image _backgroundImage;

    private void Start()
    {
        CreateBackground();
    }

    #region CreateMapComponents
    private void CreateBackground()
    {
        if (backgroundPrefab != null)
        {
            GameObject backgroundObject = Instantiate(backgroundPrefab);

            // Recherche du Canvas dans la scène
            Canvas canvas = GameObject.FindFirstObjectByType<Canvas>();
            if (canvas != null)
            {
                backgroundObject.transform.SetParent(canvas.transform, false);

                // Placer le background derrière les autres éléments
                backgroundObject.transform.SetSiblingIndex(0);
                
                SetBackground(backgroundObject, GenerateRandomInt(1,2), GenerateRandomColor());
                AdjustBackgroundSize(backgroundObject, canvas);
            }
            else
            {
                Debug.LogError("Aucun Canvas trouvé !");
            }
        }
        else
        {
            Debug.LogError("Prefab de fond non assigné !");
        }
    }
    #endregion

    #region EditMapComponents
    public void SetBackground(GameObject backgroundObject, int backgroundNumber, Color color)
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
    
    private void AdjustBackgroundSize(GameObject backgroundObject, Canvas canvas)
    {
        RectTransform canvasRectTransform = canvas.GetComponent<RectTransform>();
        RectTransform backgroundRectTransform = backgroundObject.GetComponent<RectTransform>();

        // Centrer le background en fonction du Canvas horizontalement et verticalement
        backgroundRectTransform.anchorMin = new Vector2(0.5f, 0.5f);  
        backgroundRectTransform.anchorMax = new Vector2(0.5f, 0.5f); 
        backgroundRectTransform.anchoredPosition = Vector2.zero;  // Position au centre du Canvas

        // Taille du background = taille du Canvas
        backgroundRectTransform.sizeDelta = new Vector2(canvasRectTransform.rect.width, canvasRectTransform.rect.height);
    }
    #endregion
    
    // A supprimer plus tard
    #region DevUtils
    public int GenerateRandomInt(int min, int max)
    {
        return UnityEngine.Random.Range(min, max + 1);  // max + 1 pour inclure la valeur max
    }
    public Color GenerateRandomColor()
    {
        float red = UnityEngine.Random.Range(0f, 1f);
        float green = UnityEngine.Random.Range(0f, 1f);
        float blue = UnityEngine.Random.Range(0f, 1f);
        
        return new Color(red, green, blue);
    }
    #endregion
}