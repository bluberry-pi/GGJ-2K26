using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class L5MaskCross2 : MonoBehaviour
{
    public Transform door1;

    public Vector3 openLocalPosition;
    public Vector3 closedLocalPosition;

    public float openSpeed = 3f;
    public float closeSpeed = 2f;

    public MaskScript maskScript;

    Coroutine moveRoutine;
    HashSet<Collider2D> insideObjects = new HashSet<Collider2D>();

    void Start()
    {
        if (door1 != null)
            closedLocalPosition = door1.localPosition;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActiveAndEnabled) return;
        if (!IsDoorObject(other)) return;

        if (!maskScript.IsMaskedMode && other.CompareTag("MaskedPlayer"))
            return;

        if (insideObjects.Add(other))
        {
            if (insideObjects.Count == 1)
                StartMove(openLocalPosition, openSpeed);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!isActiveAndEnabled) return;
        if (!IsDoorObject(other)) return;

        if (insideObjects.Remove(other))
        {
            if (insideObjects.Count == 0)
                StartMove(closedLocalPosition, closeSpeed);
        }
    }

    bool IsDoorObject(Collider2D other)
    {
        return other.CompareTag("NormalPlayer") ||
               other.CompareTag("MaskedPlayer") ||
               other.CompareTag("Crate");
    }

    void StartMove(Vector3 targetLocalPos, float speed)
    {
        if (!isActiveAndEnabled) return;
        if (door1 == null) return;

        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        moveRoutine = StartCoroutine(MoveDoor(targetLocalPos, speed));
    }

    IEnumerator MoveDoor(Vector3 targetLocalPos, float speed)
    {
        if (door1 == null) yield break;

        while (door1 != null &&
               Vector3.Distance(door1.localPosition, targetLocalPos) > 0.01f)
        {
            door1.localPosition = Vector3.MoveTowards(
                door1.localPosition,
                targetLocalPos,
                speed * Time.deltaTime
            );
            yield return null;
        }

        if (door1 != null)
            door1.localPosition = targetLocalPos;
    }

    void OnDisable()
    {
        StopAllCoroutines();
        insideObjects.Clear();
    }
}