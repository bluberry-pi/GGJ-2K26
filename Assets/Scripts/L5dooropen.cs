using UnityEngine;

public class L5dooropen : MonoBehaviour
{
    [Header("Door Objects")]
    public Transform doorPartA;
    public Transform doorPartB;

    [Header("Local Target Positions")]
    public Vector3 doorAPosition;
    public Vector3 doorBPosition;

    [Header("Movement")]
    public float moveSpeed = 3f;

    private bool openDoor = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("L5key"))
        {
            openDoor = true;
        }
    }

    private void Update()
    {
        if (!openDoor) return;

        doorPartA.localPosition = Vector3.MoveTowards(
            doorPartA.localPosition,
            doorAPosition,
            moveSpeed * Time.deltaTime
        );

        doorPartB.localPosition = Vector3.MoveTowards(
            doorPartB.localPosition,
            doorBPosition,
            moveSpeed * Time.deltaTime
        );
    }
}