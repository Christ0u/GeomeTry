using UnityEngine;
using System.IO;
using Defective.JSON;
using System;
using JetBrains.Annotations;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine.Tilemaps; // Dépendance externe : https://assetstore.unity.com/packages/tools/input-management/json-object-710#description
using System.Collections.Generic;
using Unity.VisualScripting;

public class Game : MonoBehaviour
{
    public GameObject Spike;
    public GameObject Tile;
    public GameObject Character;
    public GameObject Ground;
    public GameObject ShipPortal;
    public GameObject CubePortal;
    public GameObject WavePortal;
    public Tilemap tilemap;

    void Start()
    {
        #region Gestion de la map
        // Chargement du fichier JSON
        TextAsset jsonFile = Resources.Load<TextAsset>("maps/map");

        // Lecture du fichier
        string rawMap = jsonFile.text;

        // Parsing du fichier JSON
        JSONObject map = new JSONObject(rawMap);

        int lastObjectX = 0;

        //Construction de la map
        foreach (JSONObject item in map["map"])
        {
            // Récupération de l'abscisse du dernier objet a instancier
            if (lastObjectX < item["x"].intValue)
            {
                lastObjectX = item["x"].intValue;
            }

            Vector3 position = tilemap.GetCellCenterWorld(new Vector3Int(item["x"].intValue, item["y"].intValue, 0));

            // Ajustement de la position pour que l'objet soit centré sur une case de la tilemap
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
                case "shipPortal":
                    // Instanciation d'un shipPortal dans la tilemap (tilemap.transform) sans rotation (Quaternion.identity)
                    Instantiate(ShipPortal, position, Quaternion.identity, tilemap.transform);
                    break;
                case "cubePortal":
                    // Instanciation d'un cubePortal dans la tilemap (tilemap.transform) sans rotation (Quaternion.identity)
                    Instantiate(CubePortal, position, Quaternion.identity, tilemap.transform);
                    break;
                case "wavePortal":  
                    // Instanciation d'un wavePortal dans la tilemap (tilemap.transform) sans rotation (Quaternion.identity)
                    Instantiate(WavePortal, position, Quaternion.identity, tilemap.transform);
                    break;
            }

        }
        // Instanciation du personnage et de la caméra
        Instantiate(Character, tilemap.GetCellCenterWorld(new Vector3Int(-10, 1, 0)) + new Vector3(tilemap.cellSize.x / 2, tilemap.cellSize.y / 2, 0), Quaternion.identity, tilemap.transform);

        Camera cameraInstance = FindFirstObjectByType<Camera>();
        cameraInstance.player = GameObject.Find("CubePrefab(Clone)").transform;

        #endregion
 
        // #region Gestion du sol v1

        // Debug.Log("Coordonnée X du dernier objet : " + lastObjectX);

        // // Pas propre tel quel
        // int groundInitialPosition = -15;

        // for (int x = groundInitialPosition; x <= lastObjectX; x++)
        // {
        //     Vector3 groundPosition = tilemap.GetCellCenterWorld(new Vector3Int(x, -1, 0));
        //     groundPosition += new Vector3(tilemap.cellSize.x / 2, tilemap.cellSize.y / 2, 0);
        //     Instantiate(Ground, groundPosition, Quaternion.identity, tilemap.transform);
        // }

        // #endregion

        #region Gestion du sol v2

        Debug.Log("lastObjectX = " + lastObjectX);
        int groundInitialPosition = -15;

        // Définition de la taille du sol fonction de la taille du niveau
        int groundSize = lastObjectX - groundInitialPosition;
        Ground.transform.localScale = tilemap.GetCellCenterWorld(new Vector3Int(groundSize, 1, 0));
    
        // Calcul de la position du sol
        Vector3 groundPosition = tilemap.GetCellCenterWorld(new Vector3Int((lastObjectX + 1 - Mathf.Abs(groundInitialPosition))/2, -1, 0));
        groundPosition += new Vector3(tilemap.cellSize.x / 2, tilemap.cellSize.y / 2, 0);

        // Instanciation du sol
        Instantiate(Ground, groundPosition, Quaternion.identity, tilemap.transform);

        #endregion
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