using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public bool underCollision;
    private int triggerCount;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.attachedRigidbody != null)
            triggerCount++;

        underCollision = triggerCount > 0;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.attachedRigidbody != null)
            triggerCount--;

        underCollision = triggerCount > 0;
    }
}