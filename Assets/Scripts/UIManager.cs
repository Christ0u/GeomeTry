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
    
    public void OnClickPlayButton()
    {
        StartCoroutine(_gameManager.LoadScene("Level"));
        _gameManager.PlayMode = true;
    }
    
    public void OnSelectLevel()
    {
        // TODO
        
        // Changer le playmode ici
        
        // Audio
        // AudioSource audio;
        // audio = GetComponent<AudioSource>();
        // audio.Play();
    }
}