using UnityEngine;
using System.IO;
using Defective.JSON;
using System;
using JetBrains.Annotations;
using System.Runtime.CompilerServices;
using UnityEditor; // Dépendance externe : https://assetstore.unity.com/packages/tools/input-management/json-object-710#description

public class Game : MonoBehaviour
{
    public GameObject Spike;
    public GameObject Tile;
    public GameObject Character;
    private float scale = 1.2f; // Echelle des sprites

    void Start()
    {
        // Lecture du fichier JSON
        string mapPath = Application.dataPath + "/Scripts/Map/map.json";
        string rawMap = File.ReadAllText(mapPath);

        // Parsing du fichier JSON
        JSONObject map = new JSONObject(rawMap);

        Debug.Log(map["Map"]);

        // Construction de la map
        foreach (JSONObject item in map["map"])
        {
            Debug.Log(item["type"].stringValue + " aux coordonnées (" + item["x"].intValue + "; " + item["y"].intValue + ")");

            // Position du GameObject
            float x = GameObject.Find("Character").transform.position.x + (item["x"].intValue * scale);
            float y = GameObject.Find("Character").transform.position.y + (item["y"].intValue * scale);
            Vector2 position = new Vector2(x, y);

            // Création d'un élément de la map à partir du prefab correspondant
            switch ($"{item["type"].stringValue}")
            {
                case "spike":
                    Instantiate(Spike, position, Quaternion.identity); // Quaternion.identity = pas de rotation
                    break;
                case "block":
                    Instantiate(Tile, position, Quaternion.identity); // Quaternion.identity = pas de rotation
                    break;
            }
        }
    }
}