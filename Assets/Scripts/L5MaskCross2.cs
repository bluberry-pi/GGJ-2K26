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
    int objectsInside = 0;


    private void Start()
    {
        if (door1 != null)
            closedLocalPosition = door1.localPosition;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"[ENTER] {other.name} | {other.tag} | MaskedMode: {maskScript.IsMaskedMode}");

        if (!maskScript.IsMaskedMode && other.CompareTag("MaskedPlayer"))
        {
            Debug.Log("[BLOCKED] MaskedPlayer blocked");
            return;
        }

        objectsInside++;

        Debug.Log($"[COUNT] Inside = {objectsInside}");

        if (objectsInside == 1)
        {
            Debug.Log("[ACTION] Opening door");
            StartMove(openLocalPosition, openSpeed);
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log($"[EXIT] {other.name} | {other.tag} | MaskedMode: {maskScript.IsMaskedMode}");

        if (!maskScript.IsMaskedMode && other.CompareTag("MaskedPlayer"))
        {
            Debug.Log("[BLOCKED] MaskedPlayer exit ignored");
            return;
        }

        objectsInside = Mathf.Max(0, objectsInside - 1);

        Debug.Log($"[COUNT] Inside = {objectsInside}");

        if (objectsInside == 0)
        {
            Debug.Log("[ACTION] Closing door");
            StartMove(closedLocalPosition, closeSpeed);
        }
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