using UnityEngine;

public class CratePull : MonoBehaviour
{
    public float followSpeed = 10f;

    private Transform player;
    private bool canToggle;
    private bool isPulling;
    private Vector3 offset;

    void Update()
    {
        if (player == null) return;

        // Allow P toggle only while in trigger
        if (canToggle && Input.GetKeyDown(KeyCode.E))
        {
            isPulling = !isPulling;

            if (isPulling)
            {
                offset = transform.position - player.position;
            }
        }

        // FOLLOW DOES NOT CARE ABOUT TRIGGER
        if (isPulling)
        {
            Vector3 targetPos = player.position + offset;
            transform.position = Vector3.Lerp(
                transform.position,
                targetPos,
                followSpeed * Time.deltaTime
            );
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("NormalPlayer") || other.CompareTag("MaskedPlayer"))
        {
            player = other.transform;
            canToggle = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform == player)
        {
            canToggle = false;
            // do NOT touch isPulling
        }
    }
}