using UnityEngine;
using System.IO;
using Defective.JSON;
using System;
using JetBrains.Annotations;
using System.Runtime.CompilerServices; // Dépendance externe : https://assetstore.unity.com/packages/tools/input-management/json-object-710#description

public class GameHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Chargement du sprite
        Sprite blockSprite = Resources.Load<Sprite>("Block");
        Sprite spikeSprite = Resources.Load<Sprite>("Spike");
        Sprite smallSpikeSprite = Resources.Load<Sprite>("SpikeSmall");

        // Echelle des sprites
        float scale = 1.2f;

        // Lecture du fichier JSON
        string mapPath = Application.dataPath + "/Scripts/Map/map.json";
        string rawMap = File.ReadAllText(mapPath);

        // Parsing du fichier JSON
        JSONObject map = new JSONObject(rawMap);

        int index = 0;

        // Construction de la map
        foreach (JSONObject item in map["map"])
        {
            Debug.Log(item["type"].stringValue + " aux coordonnées (" + item["x"].intValue + "; " + item["y"].intValue + ")");

            // Création d'un GameObject
            GameObject gameItem = new GameObject();
            gameItem.name = $"{item["type"].stringValue}_" + index;

            // Ajout du sprite au GameObject
            SpriteRenderer spriteRenderer = gameItem.AddComponent<SpriteRenderer>();
            if ($"{item["type"].stringValue}" == "spike")
            {
                spriteRenderer.sprite = spikeSprite;
            }
            else if ($"{item["type"].stringValue}" == "block")
            {
                spriteRenderer.sprite = blockSprite;
            }
            else if ($"{item["type"].stringValue}" == "smallSpike")
            {
                spriteRenderer.sprite = smallSpikeSprite;
            }


            // Positionnement du GameObject

            float xStartPosition = GameObject.Find("Player").transform.position.x; //-10
            float yStartPosition = GameObject.Find("Player").transform.position.y; //-3.385001

            float x = xStartPosition + (item["x"].intValue * scale);
            float y = yStartPosition + (item["y"].intValue * scale);

            gameItem.transform.position = new Vector2(x, y);

            index++;
        }
    }
}