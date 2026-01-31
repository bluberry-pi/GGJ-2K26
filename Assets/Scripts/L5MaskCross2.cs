using UnityEngine;
using System.Collections;

public class L5MaskCross2 : MonoBehaviour
{
    [Header("Door (CHILD object)")]
    public Transform door1;

    [Header("Local Positions")]
    public Vector3 openLocalPosition;
    public Vector3 closedLocalPosition;

    [Header("Speeds")]
    public float openSpeed = 3f;
    public float closeSpeed = 2f;

    [Header("Mask Reference")]
    public MaskScript maskScript;

    Coroutine moveRoutine;

    private void Start()
    {
        if (door1 != null)
            closedLocalPosition = door1.localPosition;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Ignore MaskedPlayer when NOT in masked mode
        if (!maskScript.IsMaskedMode && other.CompareTag("MaskedPlayer"))
            return;

        StartMove(openLocalPosition, openSpeed);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Same rule on exit
        if (!maskScript.IsMaskedMode && other.CompareTag("MaskedPlayer"))
            return;

        StartMove(closedLocalPosition, closeSpeed);
    }

    void StartMove(Vector3 targetLocalPos, float speed)
    {
        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        moveRoutine = StartCoroutine(MoveDoor(targetLocalPos, speed));
    }

    IEnumerator MoveDoor(Vector3 targetLocalPos, float speed)
    {
        while (Vector3.Distance(door1.localPosition, targetLocalPos) > 0.01f)
        {
            door1.localPosition = Vector3.MoveTowards(
                door1.localPosition,
                targetLocalPos,
                speed * Time.deltaTime
            );

            yield return null;
        }

        door1.localPosition = targetLocalPos;
    }
}