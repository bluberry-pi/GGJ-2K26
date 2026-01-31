using UnityEngine;

public class Level4KeyPickup : MonoBehaviour
{
    public GameObject playernormalkey;
    private bool playerInRange;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            gameObject.SetActive(false);
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