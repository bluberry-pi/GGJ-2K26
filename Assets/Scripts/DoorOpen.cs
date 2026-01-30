using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public ButtonScript button;

    public Transform doorPart1;
    public Transform doorPart2;

    public Vector2 openPos1;
    public Vector2 openPos2;

    public float openSpeed = 5f;
    public float closeSpeed = 3f;

    private Vector3 startPos1;
    private Vector3 startPos2;

    void Start()
    {
        startPos1 = doorPart1.position;
        startPos2 = doorPart2.position;
    }

    void Update()
    {
        Vector3 targetPos1;
        Vector3 targetPos2;

        if (button != null && button.underCollision)
        {
            targetPos1 = new Vector3(openPos1.x, openPos1.y, startPos1.z);
            targetPos2 = new Vector3(openPos2.x, openPos2.y, startPos2.z);

            doorPart1.position = Vector3.MoveTowards(
                doorPart1.position,
                targetPos1,
                openSpeed * Time.deltaTime
            );

            doorPart2.position = Vector3.MoveTowards(
                doorPart2.position,
                targetPos2,
                openSpeed * Time.deltaTime
            );
        }
        else
        {
            doorPart1.position = Vector3.MoveTowards(
                doorPart1.position,
                startPos1,
                closeSpeed * Time.deltaTime
            );

            doorPart2.position = Vector3.MoveTowards(
                doorPart2.position,
                startPos2,
                closeSpeed * Time.deltaTime
            );
        }
    }
}