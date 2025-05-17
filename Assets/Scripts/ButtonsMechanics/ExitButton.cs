using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    private UIManager _uiManager;
    
    [SerializeField] private Sprite exitDoorClose;
    [SerializeField] private Sprite exitDoorOpen;
    [SerializeField] private AudioClip openDoorSound;
    [SerializeField] private AudioClip closeDoorSound;
    private Button button;
    private Image buttonImage;
    private AudioSource audioSource;

    void Start()
    {
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();
        audioSource = GetComponent<AudioSource>();

        if (button == null || buttonImage == null || audioSource == null)
        {
            Debug.LogError("Eléments manquants au fonctionnement du ExitButton.");
            return;
        }
    }

    public void OnMouseEnter()
    {
        if (buttonImage != null && exitDoorOpen != null)
        {
            buttonImage.sprite = exitDoorOpen;
            if (audioSource != null && openDoorSound != null)
            {
                audioSource.PlayOneShot(openDoorSound);
            }
        }
        else
        {
            Debug.LogError("Sprite ou son manquant pour l'ouverture de la porte.");
        }
    }

    public void OnMouseExit()
    {
        if (buttonImage != null && exitDoorClose != null)
        {
            buttonImage.sprite = exitDoorClose;
            if (audioSource != null && closeDoorSound != null)
            {
                audioSource.PlayOneShot(closeDoorSound);
            }
        }
        else
        {
            Debug.LogError("Sprite ou son manquant pour la fermeture de la porte.");
        }
    }
}
