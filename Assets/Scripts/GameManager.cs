using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool playMode = false;

    public void Start()
    {
        // StartCoroutine(nameof(LoadScene));
    }
    
    public IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(2);
        int actualScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(actualScene+1);
    }
}
