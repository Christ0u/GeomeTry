using UnityEngine;
using UnityEngine.SceneManagement;

public class Camera : MonoBehaviour
{
    private GameManager _gameManager; // A enlever plus tard
    
    [SerializeField] private Transform player;
    
    private float _smoothSpeed = 0.125f;
    private float _startFollowingXPosition = 0.0f; // La caméra commence à suivre le cube à partir de cette position en X
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
    // -------------------

    void Update()
    {
        if (!_isFollowing && player != null)
        {
            if (player.position.x >= _startFollowingXPosition)
            {
                _isFollowing = true;
            }
        }

        if(_isFollowing && player != null)
        {
            var desiredPosition = player.position + offset;
            var smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed);
            var freezeYPosition = new Vector3(smoothedPosition.x, transform.position.y, transform.position.z);
            
            transform.position = freezeYPosition;
        }
    }
}
