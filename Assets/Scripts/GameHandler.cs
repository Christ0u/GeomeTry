using UnityEngine;
using System.IO;
using Defective.JSON; // Dépendance externe : https://assetstore.unity.com/packages/tools/input-management/json-object-710#description

public class GameHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string mapPath = Application.dataPath + "/Scripts/Map/map.json";
        //Debug.Log("Chemin vers le fichier map : " + mapPath);
        
        string rawMap = File.ReadAllText(mapPath);
        //Debug.Log("Contenu du fichier map : " + rawMap);

        JSONObject map = new JSONObject(rawMap);
        //Debug.Log("Objet JSON : " + map["music"]["path"]);

        foreach (JSONObject item in map["map"])
        {
            Debug.Log(item["type"] + "aux coordonnées (" + item["x"] + "; " + item["y"] + ")");
        }
    }
}