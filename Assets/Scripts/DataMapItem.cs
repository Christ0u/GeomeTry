// Structure pour les items de la map dans le fichier JSON
using UnityEngine;

public class DataMapItem : MonoBehaviour
{
    public float Progress { get; private set; }
    public int Attempts { get; private set; }
    public int Jumps { get; private set; }

    public DataMapItem(float progress, int attempts, int jumps)
    {
        Progress = progress;
        Attempts = attempts;
        Jumps = jumps;
    }
}
