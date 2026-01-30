using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public ButtonScript button;
    public Transform doorPart1;
    public Transform doorPart2;
    
    // Use Vector3 for full control
    public Vector3 openPos1;
    public Vector3 openPos2;
    
    public float openSpeed = 5f;
    public float closeSpeed = 3f;
    
    private Vector3 startPos1;
    private Vector3 startPos2;
    
    void Start()
    {
        // Store LOCAL positions instead of world positions
        startPos1 = doorPart1.localPosition;
        startPos2 = doorPart2.localPosition;
    }
    
    void Update()
    {
        if (button != null && button.underCollision)
        {
            // Open the door - use localPosition
            doorPart1.localPosition = Vector3.MoveTowards(
                doorPart1.localPosition,
                openPos1,
                openSpeed * Time.deltaTime
            );
            doorPart2.localPosition = Vector3.MoveTowards(
                doorPart2.localPosition,
                openPos2,
                openSpeed * Time.deltaTime
            );
        }
        else
        {
            // Close the door - use localPosition
            doorPart1.localPosition = Vector3.MoveTowards(
                doorPart1.localPosition,
                startPos1,
                closeSpeed * Time.deltaTime
            );
            doorPart2.localPosition = Vector3.MoveTowards(
                doorPart2.localPosition,
                startPos2,
                closeSpeed * Time.deltaTime
            );
        }
    }
}