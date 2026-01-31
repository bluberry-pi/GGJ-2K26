using UnityEngine;

public class Level4KeySetup : MonoBehaviour
{
    [Header("Objects to toggle")]
    public GameObject disableOnUse;
    public GameObject enableOnUse;

    private bool playerInRange;

    void Update()
    {
        if (!playerInRange) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (disableOnUse != null)
                disableOnUse.SetActive(false);

            if (enableOnUse != null)
                enableOnUse.SetActive(true);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("MaskedPlayer") || other.CompareTag("NormalPlayer"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("MaskedPlayer") || other.CompareTag("NormalPlayer"))
            playerInRange = false;
    }
}