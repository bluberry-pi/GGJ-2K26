using UnityEngine;

public class DoorOpenKey : MonoBehaviour
{
    [Header("Key Reference")]
    public GameObject normalKey;
    [Header("Door Parts")]
    public Transform doorPart1;
    public Transform doorPart2;

    [Header("Door Positions")]
    public Vector3 openPos1;
    public Vector3 openPos2;

    [Header("Speeds")]
    public float openSpeed = 5f;
    public float closeSpeed = 3f;

    private Vector3 startPos1;
    private Vector3 startPos2;

    private bool playerInside;

    void Start()
    {
        startPos1 = doorPart1.localPosition;
        startPos2 = doorPart2.localPosition;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        playerInside = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        playerInside = false;
    }

    void Update()
    {
        bool canOpen = playerInside && normalKey != null && normalKey.activeInHierarchy;

        if (canOpen)
        {
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