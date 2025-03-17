using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.WSA;

public class UIManager : MonoBehaviour
{
    private GameManager _gameManager;
    
    private void Start()
    {
        _gameManager = GameManager.Instance;
    }

    public void OnClickSwitchSceneButton(string sceneName)
    {
        Debug.Log("Chargement de la scène " + sceneName);
        StartCoroutine(_gameManager.LoadScene(sceneName));
        Debug.Log("Fin chargement de la scène " + sceneName);
    }
    
    public void OnClickLevelButton()
    {
        StartCoroutine(_gameManager.LoadScene("Level"));
        // LaunchLevel(Level);
        _gameManager.PlayMode = true; // -> A déplacer dans le Launch
    }
}