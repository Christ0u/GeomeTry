using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Button playButton;

    public void PlayGame()
    {
        if(!GameManager.playMode)
        {
            GameManager.playMode = true;
            playButton.gameObject.SetActive(false);
        }

    }
}
