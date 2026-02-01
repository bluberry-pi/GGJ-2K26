using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    private AudioSource audioSource;

    void Awake()
    {
        // Singleton protection (prevents duplicate music)
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        int buildIndex = scene.buildIndex;

        // Music starts ONLY from scene index 2
        if (buildIndex >= 2)
        {
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
        {
            // Ensure silence in early scenes
            if (audioSource.isPlaying)
                audioSource.Stop();
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}