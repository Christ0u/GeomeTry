using UnityEngine;

public class DevUtils : MonoBehaviour
{
    public int GenerateRandomInt(int min, int max)
    {
        return Random.Range(min, max + 1);  // max + 1 pour inclure la valeur max
    }

    public Color GenerateRandomColor()
    {
        float red = Random.Range(0f, 1f);
        float green = Random.Range(0f, 1f);
        float blue = Random.Range(0f, 1f);

        return new Color(red, green, blue);
    }
}
