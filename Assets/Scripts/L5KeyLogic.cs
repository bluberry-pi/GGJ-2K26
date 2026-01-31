using UnityEngine;

public class L5KeyLogic : MonoBehaviour
{
    public GameObject key;
    public GameObject playerKey;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("NormalPlayer"))
        {
            playerKey.SetActive(true);
            key.SetActive(false); 
        }
    }
}