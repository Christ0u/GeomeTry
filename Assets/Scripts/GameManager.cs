using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool PlayMode { get; set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Garde le GameManager entre les scènes et permet de ne faire qu'une instance (à faire pour le joueur ?)
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public IEnumerator LoadScene(string sceneName)
    {
        if (!SceneExists(sceneName)) { yield break; }

        AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync(sceneName);

        if (loadSceneAsync == null) { yield break; }

        yield return new WaitUntil(() => loadSceneAsync.isDone);
    }

    #region Utils
    private bool SceneExists(string sceneName)
    {
        // Vérifie si la scène est dans les Build Settings
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneFileName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            if (sceneFileName == sceneName)
                return true;
        }
        return false;
    }
    #endregion
}