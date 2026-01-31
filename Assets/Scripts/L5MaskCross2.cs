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

    // -------------------- ENTER --------------------
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsDoorObject(other))
            return;

        Debug.Log($"[ENTER] {other.name} | {other.tag} | MaskedMode: {maskScript.IsMaskedMode}");

        // Block masked player ONLY when mask mode is OFF
        if (!maskScript.IsMaskedMode && other.CompareTag("MaskedPlayer"))
        {
            Debug.Log("[BLOCKED] MaskedPlayer blocked on enter");
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

    // -------------------- EXIT --------------------
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!IsDoorObject(other))
            return;

        Debug.Log($"[EXIT] {other.name} | {other.tag} | MaskedMode: {maskScript.IsMaskedMode}");

        objectsInside = Mathf.Max(0, objectsInside - 1);
        Debug.Log($"[COUNT] Inside = {objectsInside}");

        if (objectsInside == 0)
        {
            Debug.Log("[ACTION] Closing door");
            StartMove(closedLocalPosition, closeSpeed);
        }
    }

    // -------------------- HELPERS --------------------
    bool IsDoorObject(Collider2D other)
    {
        // Players
        if (other.CompareTag("NormalPlayer")) return true;
        if (other.CompareTag("MaskedPlayer")) return true;

        // Crates (use tag or layer, your choice)
        if (other.CompareTag("Crate")) return true;

        return false;
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