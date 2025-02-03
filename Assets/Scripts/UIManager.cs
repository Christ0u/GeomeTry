using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Button playButton;

    public void PlayGame()
    {
        if (!Global.playMode)
        {
            Global.playMode = true;
            playButton.gameObject.SetActive(false);

            // Audio
            AudioSource audio;
            audio = GetComponent<AudioSource>();
            audio.Play();
        }
    }
}