using UnityEngine;

public class MusicBootstrap : MonoBehaviour
{
    public GameObject musicManagerPrefab;

    void Awake()
    {
        if (MusicManager.Instance == null)
        {
            Instantiate(musicManagerPrefab);
        }
    }
}