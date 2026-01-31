using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("[FinishPoint] Trigger entered by: " 
                  + collision.name 
                  + " | Tag: " + collision.tag);

        if (triggered)
        {
            Debug.Log("[FinishPoint] Already triggered. Ignoring.");
            return;
        }

        if (collision.CompareTag("NormalPlayer") || collision.CompareTag("MaskedPlayer"))
        {
            Debug.Log("[FinishPoint] Valid player detected.");

            if (SceneController.instance == null)
            {
                Debug.LogError("[FinishPoint] SceneController.instance is NULL");
                return;
            }

            triggered = true;
            Debug.Log("[FinishPoint] Calling NextLevel()");
            SceneController.instance.NextLevel();
        }
        else
        {
            Debug.Log("[FinishPoint] Object tag not allowed.");
        }
    }
}