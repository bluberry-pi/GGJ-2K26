using UnityEngine;

public class Level4KeyPickup : MonoBehaviour
{
    public GameObject normalkey;
    public GameObject playernormalkey;

    private bool playerInRange;

    private void Update()
    {
        if (playerInRange && normalkey.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            normalkey.SetActive(false);
            playernormalkey.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        playerInRange = false;

    }
}