using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("[SceneController] Instance created and set.");
        }
        else
        {
            Debug.LogWarning("[SceneController] Duplicate instance destroyed.");
            Destroy(gameObject);
        }
    }

    public void NextLevel()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        Debug.Log("[SceneController] Current Scene Index: " + currentIndex);
        Debug.Log("[SceneController] Attempting to load Scene Index: " + nextIndex);
        Debug.Log("[SceneController] Total Scenes In Build: " + SceneManager.sceneCountInBuildSettings);

        if (nextIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogError("[SceneController] Next scene index out of range. Check Build Settings.");
            return;
        }

        SceneManager.LoadSceneAsync(nextIndex);
    }

    public void LoadScene(string sceneName)
    {
        Debug.Log("[SceneController] Loading scene by name: " + sceneName);
        SceneManager.LoadSceneAsync(sceneName);
    }
}