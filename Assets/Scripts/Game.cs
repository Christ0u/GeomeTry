using UnityEngine;
using System.IO;
using Defective.JSON;
using System;
using JetBrains.Annotations;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine.Tilemaps; // Dépendance externe : https://assetstore.unity.com/packages/tools/input-management/json-object-710#description
using System.Collections.Generic;

public class Game : MonoBehaviour
{
    public GameObject Spike;
    public GameObject Tile;
    public GameObject Character;
    public Tilemap tilemap;

    void Start()
    {
        // Lecture du fichier JSON
        string mapPath = Application.dataPath + "/Scripts/Map/map.json";
        string rawMap = File.ReadAllText(mapPath);

        // Parsing du fichier JSON
        JSONObject map = new JSONObject(rawMap);

        Debug.Log(map["Map"]);

        //Construction de la map
        foreach (JSONObject item in map["map"])
        {
            Debug.Log(item["type"].stringValue + " aux coordonnées (" + item["x"].intValue + "; " + item["y"].intValue + ")");

            Vector3 position = tilemap.GetCellCenterWorld(new Vector3Int(item["x"].intValue, item["y"].intValue, 0));
            // Ajustement de la position pour que l'objet soit centré une case
            position += new Vector3(tilemap.cellSize.x / 2, tilemap.cellSize.y / 2, 0);

            switch ($"{item["type"].stringValue}")
            {
                case "spike":
                    // Instanciation d'un Spike dans la tilemap (tilemap.transform) sans rotation (Quaternion.identity)
                    Instantiate(Spike, position, Quaternion.identity, tilemap.transform);
                    break;
                case "block":
                    // Instanciation d'un Spike dans la tilemap (tilemap.transform) sans rotation (Quaternion.identity)
                    Instantiate(Tile, position, Quaternion.identity, tilemap.transform);
                    break;
            }
        }
    }

    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.P))
    //     {
    //         string exportPath = Application.dataPath + "/Scripts/Map/exported_map.json";
    //         ExportMapElementsToJSON(exportPath);
    //         Debug.Log("Map exported to " + exportPath);
    //     }
    // }

    // public void ExportMapElementsToJSON(string filePath)
    // {
    //     List<JSONObject> elements = new List<JSONObject>();

    //     foreach (Transform child in tilemap.transform)
    //     {
    //         Vector3Int cellPosition = tilemap.WorldToCell(child.position);
    //         string type = child.gameObject.name.Contains("Spike") ? "spike" : "block";

    //         JSONObject element = new JSONObject();
    //         element.AddField("type", type);
    //         element.AddField("x", cellPosition.x);
    //         element.AddField("y", cellPosition.y);

    //         elements.Add(element);
    //     }

    //     JSONObject map = new JSONObject();
    //     JSONObject jsonArray = new JSONObject(JSONObject.Type.Array);
    //     foreach (var element in elements)
    //     {
    //         jsonArray.Add(element);
    //     }
    //     map.AddField("map", jsonArray);

    //     File.WriteAllText(filePath, map.ToString());
    // }
}