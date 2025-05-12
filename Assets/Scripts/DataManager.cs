using UnityEngine;
using Defective.JSON; // Dépendance externe : https://assetstore.unity.com/packages/tools/input-management/json-object-710#description

public class DataManager : MonoBehaviour
{
    // Singleton
    public static DataManager Instance { get; private set; }

    // Propriétés
    public TextAsset dataFile { get; private set; }
    public JSONObject data { get; private set; }

    private void Awake()
    {
        // Initialisation du singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Conserve l'objet entre les scènes
        }
        else
        {
            Destroy(gameObject); // Empêche les doublons
        }

        // Chargement des données
        LoadData();
    }

    private void LoadData()
    {
        // Charger le fichier JSON
        dataFile = Resources.Load<TextAsset>("Data/data");

        if (dataFile != null)
        {
            string rawData = dataFile.text;
            data = new JSONObject(rawData);
        }
        else
        {
            Debug.LogError("Fichier de données introuvable !");
        }
    }

    // Méthodes
    public void UpdateTotalJumps(int jumpCount)
    {
        if (data != null)
        {
            // Mettre à jour le nombre total de sauts
            if (data.HasField("totalJumps"))
            {
                data["totalJumps"].intValue += jumpCount;
            }
            else
            {
                data.AddField("totalJumps", jumpCount);
            }

            Debug.Log($"Total des sauts mis à jour : {data["totalJumps"].intValue}");
        }
        else
        {
            Debug.LogError("Les données n'ont pas été chargées !");
        }
    }

    public void Save()
    {
        if (data != null)
        {
            // Enregistrer les données dans un fichier JSON
            System.IO.File.WriteAllText("Assets/Resources/Data/data.json", data.ToString());
            Debug.Log("Données sauvegardées avec succès !");
        }
        else
        {
            Debug.LogError("Impossible de sauvegarder, les données sont nulles !");
        }
    }
}