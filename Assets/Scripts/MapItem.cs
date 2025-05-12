// Structure pour les items de la map
public struct MapItem
{
    public readonly string Type { get; }
    public readonly int X { get; }
    public readonly int Y { get; }
    public readonly int Rotation { get; }
    public readonly float XOffset { get; }
    public readonly float YOffset { get; }

    public MapItem(string type, int x, int y, int rotation, float xOffset, float yOffset)
    {
        Type = type;
        X = x;
        Y = y;
        Rotation = rotation;
        XOffset = xOffset;
        YOffset = yOffset;
    }
}