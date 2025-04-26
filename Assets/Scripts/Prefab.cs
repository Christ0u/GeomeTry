using UnityEngine;
using System.Collections.Generic;

public static class Prefab
{
    private static GameObject Tile = Resources.Load<GameObject>("Prefabs/Decors/TilePrefab");
    private static GameObject SmallTile = Resources.Load<GameObject>("Prefabs/Decors/SmallTilePrefab");
    private static GameObject Spike = Resources.Load<GameObject>("Prefabs/Decors/SpikePrefab");
    private static GameObject SmallSpike = Resources.Load<GameObject>("Prefabs/Decors/SmallSpikePrefab");
    private static GameObject ShipPortal = Resources.Load<GameObject>("Prefabs/Portals/ShipPortal");
    private static GameObject CubePortal = Resources.Load<GameObject>("Prefabs/Portals/CubePortal");
    private static GameObject WavePortal = Resources.Load<GameObject>("Prefabs/Portals/WavePortal");
    private static GameObject YellowJumpPad = Resources.Load<GameObject>("Prefabs/JumpPads/YellowJumpPad");
    private static GameObject PinkJumpPad = Resources.Load<GameObject>("Prefabs/JumpPads/PinkJumpPad");
    private static GameObject RedJumpPad = Resources.Load<GameObject>("Prefabs/JumpPads/RedJumpPad");
    private static GameObject EndWall = Resources.Load<GameObject>("Prefabs/Decors/EndWallPrefab");
    public static Dictionary<string, GameObject> Prefabs { get; private set; }

    static Prefab()
    {
        Prefabs = new Dictionary<string, GameObject>
        {
            { "tile",           Tile },
            { "smallTile",      SmallTile },
            { "spike",          Spike },
            { "smallSpike",     SmallSpike },
            { "shipPortal",     ShipPortal },
            { "cubePortal",     CubePortal },
            { "wavePortal",     WavePortal },
            { "yellowJumpPad",  YellowJumpPad },
            { "pinkJumpPad",    PinkJumpPad },
            { "redJumpPad",     RedJumpPad },
            { "endWall",        EndWall }
        };
    }
}