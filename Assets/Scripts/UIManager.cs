using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private GameManager _gameManager;
    
    private void Start()
    {
        _gameManager = GameManager.Instance;
    }

    public void OnClickSwitchSceneButton(string sceneName)
    {
        StartCoroutine(_gameManager.LoadScene(sceneName));
    }
    
    // A modifier plus tard
    public void OnClickLevelButton()
    {
        StartCoroutine(_gameManager.LoadScene("Level"));
        _gameManager.PlayMode = true;
        
        // TODO
        // Audio
        // AudioSource audio;
        // audio = GetComponent<AudioSource>();
        // audio.Play();
    }
}