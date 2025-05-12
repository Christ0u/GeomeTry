using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Defective.JSON;
using System.IO;

public class Game : MonoBehaviour
{
    public GameObject CharacterInstance;
    public GameObject Ground;
    public Tilemap Tilemap;

    // Propriétés
    private GameMode _gameMode;
    private Level _level;
    AudioSource audioSource;

    void Start()
    {
        if (GameManager.Instance != null && GameManager.Instance.SelectedLevel != null)
        {
            _level = GameManager.Instance.SelectedLevel;
            LaunchLevel(_level);
        }
        else
        {
            Debug.LogError("Le niveau sélectionné est nul !");
            return;
        }
    }

    // Méthodes
    void LaunchLevel(Level level)
    {
        //Debug.Log("Lancement du niveau " + level.Name);

        #region Génération de le map

        foreach (MapItem mapItem in level.Map)
        {
            Vector3 position = Tilemap.GetCellCenterWorld(new Vector3Int(mapItem.X, mapItem.Y, 0));

            //Ajustement de la position pour que l'objet soit centré sur une case de la tilemap
            position += new Vector3(Tilemap.cellSize.x / 2 + mapItem.XOffset, Tilemap.cellSize.y / 2 + mapItem.YOffset, 0);

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
        Vector3 endWallPosition = Tilemap.GetCellCenterWorld(new Vector3Int(level.getLastMapItem().X + 16, 0, 0));
        endWallPosition += new Vector3(Tilemap.cellSize.x / 2, Tilemap.cellSize.y / 2, 0);
        if (Prefab.Prefabs.TryGetValue("endWall", out GameObject endWallPrefab))
        {
            Instantiate(endWallPrefab, endWallPosition, Quaternion.identity, Tilemap.transform);
        }

        #endregion

        #region Génération du personnage et de la caméra

        // Instanciation du personnage et de la caméra
        CharacterInstance = Resources.Load<GameObject>("Prefabs/Player/CubePrefab");
        Instantiate(CharacterInstance, Tilemap.GetCellCenterWorld(new Vector3Int(-10, 1, 0)) + new Vector3(Tilemap.cellSize.x / 2, Tilemap.cellSize.y / 2, 0), Quaternion.identity, Tilemap.transform);
        Camera cameraInstance = FindFirstObjectByType<Camera>();
        cameraInstance.player = GameObject.Find("CubePrefab(Clone)").transform;

        #endregion

        #region Génération du sol
        int offset = 25;

        // Position initiale du personnage
        int characterOrigin = (int)CharacterInstance.transform.position.x;

        // Définition des coordonnées du sol
        int groundOrigin = characterOrigin - offset;
        int groundEnd = level.getLastMapItem().X + offset;

        // Définition de la taille du sol
        int groundSize = groundEnd - groundOrigin;
        Vector3 groundScale = Tilemap.GetCellCenterWorld(new Vector3Int(groundSize, 3, 0));

        // Définitioin de la position du sol
        Vector3 groundPosition = Tilemap.GetCellCenterWorld(new Vector3Int((groundEnd + groundOrigin) / 2, -2, 0));
        groundPosition += new Vector3(Tilemap.cellSize.x / 2, Tilemap.cellSize.y / 2, 0);

        // Instanciation du sol
        GameObject groundInstance = Instantiate(Ground, groundPosition, Quaternion.identity, Tilemap.transform);
        groundInstance.transform.localScale = groundScale;

        // Couleur du sol
        Renderer groundRenderer = groundInstance.GetComponent<Renderer>();
        if (groundRenderer != null)
        {
            // Générer une couleur aléatoire
            Color groundColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            groundRenderer.material.color = groundColor;
        }
        #endregion


        #region Génération du plafond

        GameObject ceilPrefab = Resources.Load<GameObject>("Prefabs/Decors/CeilPrefab");

        if (ceilPrefab == null)
        {
            Debug.LogError("Le prefab de plafond (CeilPrefab) est introuvable.");
            return;
        }

        // Récupération des portails
        var portals = level.Map
            .Where(item => item.Type.Contains("Portal"))
            .OrderBy(item => item.X)
            .ToList();

        for (int i = 0; i < portals.Count; i++)
        {
            //Debug.Log("Portail du type " + portals[i].Type + " trouvé aux coordonnées : " + portals[i].X + ", " + portals[i].Y);

            if (portals[i].Type == "shipPortal")
            {
                int startX = portals[i].X + 2;
                int endX;

                int nextPortalIndex = i + 1;

                if (nextPortalIndex < portals.Count)
                {
                    endX = portals[nextPortalIndex].X;
                }
                else
                {
                    endX = level.getLastMapItem().X + offset;
                }

                // Récupérer la position Y la plus haute dans l'intervalle [startX, endX]
                int highestY = level.getHighestY(startX, endX);

                // Définir la taille et la position du plafond
                int ceilSize = endX - startX;
                Vector3 ceilScale = Tilemap.GetCellCenterWorld(new Vector3Int(ceilSize, 7, 0));
                Vector3 ceilPosition = Tilemap.GetCellCenterWorld(new Vector3Int((startX + endX) / 2, highestY + 4, 0)); // Position Y ajustée
                ceilPosition += new Vector3(Tilemap.cellSize.x / 2, Tilemap.cellSize.y / 2, 0);

                // Instancier le plafond
                GameObject ceilInstance = Instantiate(ceilPrefab, ceilPosition, Quaternion.identity, Tilemap.transform);
                ceilInstance.transform.localScale = ceilScale;
            }
        }

        #endregion

        #region Musique

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = level.Music;

        #endregion
    }

    void Update()
    {
        // Gestion de la musique
        GameObject character = GameObject.FindGameObjectWithTag("Player");
        if (character == null)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
        else
        {
            float initialPosition = Character.InitialPosition;
            // Si le personnage est en vie et qu'il a dépassé sa position initiale, on joue la musique
            if (character.gameObject.GetComponent<Character>().isAlive && character.transform.position.x >= initialPosition)
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
            else
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
            }
        }
        // TEMPORAIRE
        // Exporter la map en JSON lorsque la touche "P" est pressée
        if (Input.GetKeyDown(KeyCode.P))
        {
            ExportCurrentMapToJson();
        }

    }

    // TEMPORAIRE
    void ExportCurrentMapToJson()
    {
        if (Tilemap == null)
        {
            Debug.LogError("La Tilemap n'est pas définie !");
            return;
        }

        JSONObject json = new JSONObject();
        json.AddField("id", -1); // L'id est maintenant la première propriété
        json.AddField("name", "LevelName");
        json.AddField("difficulty", -1);

        JSONObject mapArray = new JSONObject();
        mapArray.type = JSONObject.Type.Array;

        // Récupérer tous les enfants de la Tilemap et les trier par x
        var children = Tilemap.transform.Cast<Transform>()
            .Select(child => new
            {
                GameObject = child.gameObject,
                CellPosition = Tilemap.WorldToCell(child.position)
            })
            .OrderBy(item => item.CellPosition.x)
            .ThenBy(item => item.CellPosition.y) // Optionnel : trier aussi par y si nécessaire
            .ToList();

        // Parcourir les enfants triés
        foreach (var item in children)
        {
            GameObject obj = item.GameObject;
            Vector3Int cellPosition = item.CellPosition;

            // Récupérer le type à partir du nom de l'objet
            string type = obj.name.Replace("(Clone)", "").Replace("Prefab", "").Trim();

            // Mettre en minuscule la première lettre du type
            if (!string.IsNullOrEmpty(type))
            {
                type = char.ToLower(type[0]) + type.Substring(1);
            }

            // Ignorer le joueur (par exemple, si son nom est "Cube")
            if (type == "cube")
            {
                continue;
            }

            JSONObject mapObject = new JSONObject();
            mapObject.AddField("type", type); // Utiliser le nom nettoyé et modifié comme type
            mapObject.AddField("x", cellPosition.x);
            mapObject.AddField("y", cellPosition.y);

            mapArray.Add(mapObject);
        }

        json.AddField("map", mapArray);

        // Ajouter la musique
        json.AddField("music", "MusicName");

        string jsonString = json.ToString(true);

        // Sauvegarder le fichier JSON dans le dossier Resources\Maps
        string path = Path.Combine(Application.dataPath, "Resources", "Maps", $"{_level?.Name ?? "UnnamedLevel"}_export.json");
        File.WriteAllText(path, jsonString);

        Debug.Log($"Map exportée avec succès : {path}");
    }
}
