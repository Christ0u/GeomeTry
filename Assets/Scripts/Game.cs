using UnityEngine;
using UnityEngine.Tilemaps;

public class Game : MonoBehaviour
{
    public GameObject Character;
    public GameObject Ground;
    public Tilemap Tilemap;

    // Propriétés
    private GameMode _gameMode;
    private Level _level;

    void Start()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("maps/test");
        _level = new Level(jsonFile);

        LaunchLevel(_level);
    }

    // Méthodes
    void LaunchLevel(Level level)
    {
        Debug.Log("Lancement du niveau " + level.Name);

        #region Génération de le map

        foreach (MapItem mapItem in level.Map)
        {
            Vector3 position = Tilemap.GetCellCenterWorld(new Vector3Int(mapItem.X, mapItem.Y, 0));

            //Ajustement de la position pour que l'objet soit centré sur une case de la tilemap
            position += new Vector3(Tilemap.cellSize.x / 2, Tilemap.cellSize.y / 2, 0);

            // Rotation de l'objet
            Quaternion rotation;
            if (mapItem.Rotation != 0)
            {
                rotation = Quaternion.Euler(0, 0, mapItem.Rotation);
            }
            else
            {
                // Rotation nulle par défaut
                rotation = Quaternion.identity;
            }

            // Instanciation de l'objet en utilisant le dictionnaire
            if (Prefab.Prefabs.TryGetValue(mapItem.Type, out GameObject prefab))
            {
                Instantiate(prefab, position, rotation, Tilemap.transform);
            }
        }

        // Création d'un objet matérialisant la fin de la map
        GameObject obj = new GameObject("EndOfMap");

        Vector3 endPosition = Tilemap.GetCellCenterWorld(new Vector3Int(level.getLastMapItem().X + 1, 0, 0));
        endPosition += new Vector3(Tilemap.cellSize.x / 2, Tilemap.cellSize.y / 2, 0);
        obj.transform.position = endPosition;

        obj.gameObject.tag = "EndOfMap";

        // Instanciation du mur de fin
        Vector3 endWallPosition = Tilemap.GetCellCenterWorld(new Vector3Int(level.getLastMapItem().X + 15, 0, 0));
        endWallPosition += new Vector3(Tilemap.cellSize.x / 2, Tilemap.cellSize.y / 2, 0);
        if (Prefab.Prefabs.TryGetValue("endWall", out GameObject endWallPrefab))
        {
            Instantiate(endWallPrefab, endWallPosition, Quaternion.identity, Tilemap.transform);
        }

        #endregion

        #region Génération du personnage et de la caméra

        // Instanciation du personnage et de la caméra
        Instantiate(Character, Tilemap.GetCellCenterWorld(new Vector3Int(-10, 1, 0)) + new Vector3(Tilemap.cellSize.x / 2, Tilemap.cellSize.y / 2, 0), Quaternion.identity, Tilemap.transform);
        Camera cameraInstance = FindFirstObjectByType<Camera>();
        cameraInstance.player = GameObject.Find("CubePrefab(Clone)").transform;

        #endregion

        #region Génération du sol
        int offset = 25;

        // Position initiale du personnage
        int characterOrigin = (int)Character.transform.position.x;

        // Définition des coordonnées du sol
        int groundOrigin = characterOrigin - offset;
        int groundEnd = level.getLastMapItem().X + offset;

        // Définition de la taille du sol
        int groundSize = groundEnd - groundOrigin;
        Vector3 groundScale = Tilemap.GetCellCenterWorld(new Vector3Int(groundSize, 1, 0));

        // Définitioin de la position du sol
        Vector3 groundPosition = Tilemap.GetCellCenterWorld(new Vector3Int((groundEnd + groundOrigin) / 2, -1, 0));
        groundPosition += new Vector3(Tilemap.cellSize.x / 2, Tilemap.cellSize.y / 2, 0);

        // Instanciation du sol
        GameObject groundInstance = Instantiate(Ground, groundPosition, Quaternion.identity, Tilemap.transform);
        groundInstance.transform.localScale = groundScale;
        #endregion
    }
}