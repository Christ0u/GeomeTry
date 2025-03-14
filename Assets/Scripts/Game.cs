using UnityEngine;
using Defective.JSON; // Dépendance externe : https://assetstore.unity.com/packages/tools/input-management/json-object-710#description
using UnityEngine.Tilemaps;
using System.IO;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
    public GameObject Character;
    public GameObject Tile;
    public GameObject SmallTile;
    public GameObject Spike;
    public GameObject SmallSpike;
    public GameObject Ground;
    public GameObject ShipPortal;
    public GameObject CubePortal;
    public GameObject WavePortal;
    public Tilemap tilemap;

    void Start()
    {
        #region Génération de le map
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

            // Application de la rotation
            Quaternion rotation = Quaternion.Euler(0, 0, item["rotation"].intValue);

            switch ($"{item["type"].stringValue}")
            {
                case "tile":
                    // Instanciation d'un Tile dans la tilemap
                    Instantiate(Tile, position, rotation, tilemap.transform);
                    break;
                case "smallTile":
                    // Instanciation d'un SmallTile dans la tilemap
                    Instantiate(SmallTile, position, rotation, tilemap.transform);
                    break;
                case "spike":
                    // Instanciation d'un Spike dans la tilemap
                    Instantiate(Spike, position, rotation, tilemap.transform);
                    break;
                case "smallSpike":
                    // Instanciation d'un SmallSpike dans la tilemap
                    Instantiate(SmallSpike, position, rotation, tilemap.transform);
                    break;
                case "shipPortal":
                    // Instanciation d'un shipPortal dans la tilemap
                    Instantiate(ShipPortal, position, rotation, tilemap.transform);
                    break;
                case "cubePortal":
                    // Instanciation d'un cubePortal dans la tilemap
                    Instantiate(CubePortal, position, rotation, tilemap.transform);
                    break;
                case "wavePortal":
                    // Instanciation d'un wavePortal dans la tilemap
                    Instantiate(WavePortal, position, rotation, tilemap.transform);
                    break;
            }
        }

        #endregion

        #region Génération du personnage et de la caméra

        // Instanciation du personnage et de la caméra
        Instantiate(Character, tilemap.GetCellCenterWorld(new Vector3Int(-10, 1, 0)) + new Vector3(tilemap.cellSize.x / 2, tilemap.cellSize.y / 2, 0), Quaternion.identity, tilemap.transform);

        Camera cameraInstance = FindFirstObjectByType<Camera>();
        cameraInstance.player = GameObject.Find("CubePrefab(Clone)").transform;

        #endregion

        #region Génération du sol

        int offset = 25;

        // Position initiale du personnage
        int characterOrigin = (int)Character.transform.position.x;

        // Définition des coordonnées du sol
        int groundOrigin = characterOrigin - offset;
        int groundEnd = lastObjectX + offset;

        // Définition de la taille du sol
        int groundSize = groundEnd - groundOrigin;
        Vector3 groundScale = tilemap.GetCellCenterWorld(new Vector3Int(groundSize, 1, 0));

        // Définitioin de la position du sol
        Vector3 groundPosition = tilemap.GetCellCenterWorld(new Vector3Int((groundEnd + groundOrigin) / 2, -1, 0)); groundPosition += new Vector3(tilemap.cellSize.x / 2, tilemap.cellSize.y / 2, 0);

        // Instanciation du sol
        GameObject groundInstance = Instantiate(Ground, groundPosition, Quaternion.identity, tilemap.transform);
        groundInstance.transform.localScale = groundScale;

        #endregion
    }
}