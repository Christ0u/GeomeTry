using System.IO;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class DevUtils : MonoBehaviour
{
    public Color backgroundMenuColor;
    public int backgroundIndex;

    // FINALEMENT PAS UTILISER MAIS JE LAISSE ICI, L'IDEE PEUT ETRE INTERESSANTE

    // // Liste des scènes pour lesquelles un fond spécifique doit être appliqué
    // List<string> menuScenes = new List<string>
    // {
    //     "Main Menu",
    //     "Customisation",
    //     "Settings",
    //     "Credits",
    //     "Stats",
    //     "Success",
    //     "Profile",
    //     "Level Selection",
    //     "Level Editor"
    // };

    // Count
    public int backgroundQuantity;

    private void Awake()
    {
        CountBackgroundFiles();

        // Modifier ici le fond de menu
        backgroundMenuColor = GenerateRandomColor();
        backgroundIndex = GenerateRandomInt(1, backgroundQuantity);
    }

    #region Count data
    void CountBackgroundFiles()
    {
        string folderPath = "Assets/Resources/UI/Backgrounds";
        
        if (Directory.Exists(folderPath))
        {
            // Filtre les fichiers pour exclure les fichiers .meta
            string[] files = Directory.GetFiles(folderPath)
                .Where(file => !file.EndsWith(".meta")) 
                .ToArray();
            
            backgroundQuantity = files.Length;
        }
        else
        {
            Debug.LogWarning($"Le dossier {folderPath} n'existe pas.");
        }
    }
    #endregion
    
    #region Generate random data
    public int GenerateRandomInt(int min, int max)
    {
        return Random.Range(min, max + 1);  // max + 1 pour inclure la valeur max
    }

    public Color GenerateRandomColor()
    {
        float red = Random.Range(0f, 1f);
        float green = Random.Range(0f, 1f);
        float blue = Random.Range(0f, 1f);

        return new Color(red, green, blue);
    }
    #endregion
}
