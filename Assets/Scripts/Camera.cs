using UnityEngine;
using UnityEngine.SceneManagement;

public class Camera : MonoBehaviour
{
    private GameManager _gameManager; // A enlever plus tard

    [SerializeField] public Transform player;

    private float _smoothSpeed = 0.05f;
    private float _startFollowingXPosition = -5.0f; // La caméra commence à suivre le cube à partir de cette position en X
    public Vector3 offset;
    private bool _isFollowing = false;

    // A enlever plus tard
    private void Start()
    {
        _gameManager = GameManager.Instance;

        if (SceneManager.GetActiveScene().name != "Main Menu" && !_gameManager.PlayMode)
        {
            SceneManager.LoadScene("Main Menu");
        }
    }

    void FixedUpdate()
    {
        if (!_isFollowing && player != null)
        {
            if (player.position.x >= _startFollowingXPosition)
            {
                _isFollowing = true;
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
