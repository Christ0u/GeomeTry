using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelSelectionManager : MonoBehaviour
{
    private UIManager _uiManager;
    public GameObject buttonPrefab;
    public Transform levelContainer;

    private List<TextAsset> levelFiles = new List<TextAsset>();

    void Awake()
    {
        _uiManager = FindFirstObjectByType<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("UIManager non trouvé !");
        }
    }

    void Start()
    {
        LoadLevelFiles();
        GenerateLevelButtons();
    }

    void LoadLevelFiles()
    {
        // Resources/Maps/BASE
        TextAsset[] files = Resources.LoadAll<TextAsset>("Maps/BASE");
        levelFiles.AddRange(files);
        
        Debug.Log($"{levelFiles.Count} niveaux trouvés dans Maps/BASE");
    }

    void GenerateLevelButtons()
    {
        foreach (TextAsset levelFile in levelFiles)
        {
            Level level = new Level(levelFile);
            
            // Création du bouton
            GameObject buttonObj = Instantiate(buttonPrefab, levelContainer);
            Button button = buttonObj.GetComponent<Button>();
            
            // Modifier le texte du bouton
            TMPro.TextMeshProUGUI tmpText = buttonObj.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (tmpText != null)
            {
                tmpText.text = level.Name;
            }
            
            // Stockage de la référence du niveau pour éviter les problèmes de fermeture de portée
            Level levelRef = level;
            
            // Ajout de l'écouteur d'événement pour le clic sur le bouton
            button.onClick.AddListener(() => _uiManager.OnClickLevelButton(levelRef, levelFile));
        }
    }
    
}