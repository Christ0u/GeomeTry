using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Image mask;
    private Transform player;
    private float startPosition;
    private float endPosition = 300f;
    private float currentFillAmount;
    public TextMeshProUGUI textMesh;

    void Start()
    {
        startPosition = Character.InitialPosition;
    }

    void Update()
    {
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
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
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