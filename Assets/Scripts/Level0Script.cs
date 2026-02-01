using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level0Script : MonoBehaviour
{
    [Header("Timing")]
    public float delay = 1.5f;

    [Header("Objects")]
    public GameObject first;
    public GameObject second;
    public GameObject third;

    void Start()
    {
        StartCoroutine(Sequence());
    }

    IEnumerator Sequence()
    {
        yield return new WaitForSeconds(delay);

        if (first) first.SetActive(false);
        if (second) second.SetActive(true);

        yield return new WaitForSeconds(delay);

        if (second) second.SetActive(false);
        if (third) third.SetActive(true);

        yield return new WaitForSeconds(delay);

        LoadNextScene();
    }

    void LoadNextScene()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index + 1);
    }
}