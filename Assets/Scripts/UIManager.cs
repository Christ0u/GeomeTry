using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Button playButton;
    [SerializeField] GameManager gameManager;

    public void PlayGame()
    {
        if(!gameManager.playMode)
        {
            gameManager.playMode = true;
            playButton.gameObject.SetActive(false);
        }

    }
}
