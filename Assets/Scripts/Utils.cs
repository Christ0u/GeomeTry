using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;
using System.Runtime.CompilerServices;

// Structure pour les items de la map
public struct MapItem
{
    public readonly string Type { get; }
    public readonly int X { get; }
    public readonly int Y { get; }
    public readonly int Rotation { get; }

    public MapItem(string type, int x, int y, int rotation)
    {
        Type = type;
        X = x;
        Y = y;
        Rotation = rotation;
    }
}

enum GameMode
{
    Normal,
    Practice
}

public static class Utils
{
    private static GameObject Tile = Resources.Load<GameObject>("Prefabs/Decors/TilePrefab");
    private static GameObject SmallTile = Resources.Load<GameObject>("Prefabs/Decors/SmallTilePrefab");
    private static GameObject Spike = Resources.Load<GameObject>("Prefabs/Decors/SpikePrefab");
    private static GameObject SmallSpike = Resources.Load<GameObject>("Prefabs/Decors/SmallSpikePrefab");
    private static GameObject ShipPortal = Resources.Load<GameObject>("Prefabs/Portals/ShipPortal");
    private static GameObject CubePortal = Resources.Load<GameObject>("Prefabs/Portals/CubePortal");
    private static GameObject WavePortal = Resources.Load<GameObject>("Prefabs/Portals/WavePortal");
    public static Dictionary<string, GameObject> Prefabs { get; private set; }

    static Utils()
    {
        Prefabs = new Dictionary<string, GameObject>
        {
            { "tile",       Tile },
            { "smallTile",  SmallTile },
            { "spike",      Spike },
            { "smallSpike", SmallSpike },
            { "shipPortal", ShipPortal },
            { "cubePortal", CubePortal },
            { "wavePortal", WavePortal }
        };
    }
}