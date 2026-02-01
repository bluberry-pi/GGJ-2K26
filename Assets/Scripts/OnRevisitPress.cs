using UnityEngine;
using UnityEngine.SceneManagement;

public class OnRevisitPress : MonoBehaviour
{
    public void LoadNextLevel()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentIndex + 1);
    }
}