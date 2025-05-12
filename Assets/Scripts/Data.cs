using UnityEngine;
using System.Collections.Generic;
using Defective.JSON;
using Unity.VisualScripting; // Dépendance externe : https://assetstore.unity.com/packages/tools/input-management/json-object-710#description

public class Data : MonoBehaviour
{
    public List<Dictionary<int, DataMapItem>> MapsData { get; private set; }
    public int TotalJumps { get; set; }
    public int TotalAttempts { get; private set; }

    // Constructeur
    public Data(TextAsset dataFile)
    {
        // Lecture du fichier
        string rawData = dataFile.text;

        // Parsing du fichier JSON
        JSONObject jsonData = new JSONObject(rawData);

        // Enregistrement des propriétés

        // MapsData
        MapsData = new List<Dictionary<int, DataMapItem>>();

        foreach (var key in jsonData["maps"].keys)
        {

            Dictionary<int, DataMapItem> mapItem = new Dictionary<int, DataMapItem>();

            // Récupération des données de la carte
            JSONObject item = jsonData["maps"][key];
            int id = int.Parse(key); // La clé est l'ID
            float progress = item["progress"].floatValue;
            int attempts = item["attempts"].intValue;
            int jumps = item["jumps"].intValue;

            // Ajout des données dans le dictionnaire
            mapItem.Add(id, new DataMapItem(progress, attempts, jumps));
            MapsData.Add(mapItem);

        }

        // TotalJumps
        TotalJumps = jsonData["totalJumps"].intValue;

        // TotalAttempts
        TotalAttempts = jsonData["totalAttempts"].intValue;
    }

    // Debug
    public void ShowData()
    {
        foreach (var map in MapsData)
        {
            foreach (var item in map)
            {
                Debug.Log($"Map ID: {item.Key}, Progress: {item.Value.Progress}, Attempts: {item.Value.Attempts}, Jumps: {item.Value.Jumps}");
            }
        }

        Debug.Log($"Total Jumps: {TotalJumps}");
        Debug.Log($"Total Attempts: {TotalAttempts}");
    }
}
