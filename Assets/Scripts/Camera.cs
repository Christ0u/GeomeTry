using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] public Transform player;
    public Vector3 offset;
    private float _smoothSpeed = 0.05f;
    private float _startFollowingXPosition = -5f; // La caméra commence à suivre le cube à partir de cette position en X
    private float _endFollowingXPosition; // La caméra arrête de suivre le cube à partir de cette position en X
    private bool _isFollowing = false;
    private bool _isEndObjectFound = false; // Indique si l'objet de fin a été trouvé

    private void Update()
    {
        // Si l'objet de fin n'a pas encore été trouvé, on le recherche
        if (!_isEndObjectFound)
        {
            GameObject endObject = GameObject.FindWithTag("EndOfMap");
            if (endObject != null)
            {
                _endFollowingXPosition = endObject.transform.position.x;
                _isEndObjectFound = true; // L'objet de fin a été trouvé
            }
        }
    }

    private void FixedUpdate()
    {
        if (player != null)
        {
            if (player.position.x >= _startFollowingXPosition && player.position.x <= _endFollowingXPosition)
            {
                _isFollowing = true;
            }
            else
            {
                _isFollowing = false;
            }
        }

        if (_isFollowing && player != null)
        {
            var desiredPosition = player.position + new Vector3(5f, 0, -10f); // + offset
            var smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed);

            if (smoothedPosition.y < 4f)
            {
                smoothedPosition.y = 4f;
            }

            transform.position = smoothedPosition;
        }
    }
}