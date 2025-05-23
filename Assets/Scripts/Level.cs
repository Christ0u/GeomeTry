using UnityEngine;
using Defective.JSON; // Dépendance externe : https://assetstore.unity.com/packages/tools/input-management/json-object-710#description
using System.Collections.Generic;
using System.Linq;

public class Level
{
    // Propriétés
    public int Id { get; private set; }
    public string Name { get; private set; }
    public int Difficulty { get; private set; }
    public List<MapItem> Map { get; private set; }
    public AudioClip Music { get; private set; }
    private MapItem _lastMapItem;

    // Constructeur
    public Level(TextAsset mapFile)
    {
        // Lecture du fichier
        string rawData = mapFile.text;

        // Parsing du fichier JSON
        JSONObject jsonData = new JSONObject(rawData);

        // Enregistrement des propriétés
        Name = jsonData["name"].stringValue;

        Id = jsonData["id"].intValue;

        Difficulty = jsonData["difficulty"].intValue;

        Map = new List<MapItem>();
        foreach (JSONObject item in jsonData["map"].list)
        {
            MapItem mapItem = new MapItem(
                item["type"].stringValue,
                item["x"].intValue,
                item["y"].intValue,
                item.HasField("rotation") ? item["rotation"].intValue : 0,
                item.HasField("xOffset") ? item["xOffset"].floatValue : 0.0f,
                item.HasField("yOffset") ? item["yOffset"].floatValue : 0.0f
            );
            Map.Add(mapItem);

            if (mapItem.X > _lastMapItem.X)
            {
                _lastMapItem = mapItem;
            }
        }

        // Chargement de la musique
        Music = Resources.Load<AudioClip>("Songs/Musics/" + jsonData["music"].stringValue);
    }

    public MapItem getLastMapItem()
    {
        return _lastMapItem;
    }

    public int getHighestY(int startX, int endX)
    {
        // Trouver l'élément avec la plus grande valeur de Y dans la plage donnée
        var highestItem = Map
            .Where(item => item.X >= startX && item.X <= endX && item.Y > 0)
            .OrderByDescending(item => item.Y)
            .FirstOrDefault();

        return highestItem.Y;
    }
}