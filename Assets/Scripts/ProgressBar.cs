using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Image mask;
    public TextMeshProUGUI textMesh;
    private Transform player;
    private float startPosition;
    private float endPosition;
    private float currentFillAmount;

    void Start()
    {
        // Initialisation de la position de départ
        startPosition = Character.InitialPosition;
    }

    void Update()
    {
        // Vérifier si l'objet de fin est trouvé
        if (endPosition == 0)
        {
            FindEndObject();
            if (endPosition == 0)
            {
                Debug.LogError("Impossible de trouver l'objet de fin");
                return;
            }
        }

        // Vérifier si le joueur est trouvé
        if (player == null)
        {
            FindPlayer();
        }
        else
        {
            SetProgress();
        }
    }

    void FindPlayer()
    {
        // Trouver le joueur par son tag
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    void FindEndObject()
    {
        // Trouver l'objet de fin par son tag
        GameObject endObject = GameObject.FindWithTag("EndOfMap");
        if (endObject != null)
        {
            endPosition = endObject.transform.position.x;
        }
    }

    void SetProgress()
    {
        // Position x actuelle du joueur
        float playerPosition = player.position.x;

        // Calcul du pourcentage de progression
        float percentage = (playerPosition - startPosition) / (endPosition - startPosition) * 100f;
        percentage = Mathf.Clamp(percentage, 0f, 100f);

        // Interpolation en douceur
        currentFillAmount = Mathf.Lerp(currentFillAmount, percentage / 100f, Time.deltaTime * 5f);

        // Remplissage de la barre de progression
        mask.fillAmount = currentFillAmount;

        // Affichage du pourcentage avec deux chiffres après la virgule
        textMesh.text = $"{percentage:F2}%";
    }
}