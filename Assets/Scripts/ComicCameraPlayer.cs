using System.Collections;
using UnityEngine;

public class ComicCameraPlayer : MonoBehaviour
{
    [Header("Comic")]
    public SpriteRenderer comicSprite;

    [Header("Panel Rects (in order)")]
    public Rect[] panels =
    {
        new Rect(0.0f, 0.774f, 0.501f, 0.228f),
        new Rect(0.509f, 0.774f, 0.491f, 0.228f),
        new Rect(0.001f, 0.535f, 0.496f, 0.231f),
        new Rect(0.509f, 0.535f, 0.496f, 0.231f),
        new Rect(0.001f, 0.295f, 0.496f, 0.231f),
        new Rect(0.504f, 0.295f, 0.496f, 0.231f),
        new Rect(0.251f, 0.037f, 0.541f, 0.231f)
    };

    [Header("Timing")]
    public float moveSpeed = 2.5f;
    public float zoomSpeed = 2.5f;
    public float holdTime = 1.2f;

    Camera cam;
    Bounds bounds;

    void Start()
    {
        cam = GetComponent<Camera>();
        cam.orthographic = true;

        bounds = comicSprite.bounds;

        StartCoroutine(PlayComic());
    }

    IEnumerator PlayComic()
    {
        foreach (Rect panel in panels)
        {
            yield return MoveToPanel(panel);
            yield return new WaitForSeconds(holdTime);
        }
    }

    IEnumerator MoveToPanel(Rect panel)
    {
        Vector3 targetPos = new Vector3(
            bounds.min.x + bounds.size.x * (panel.x + panel.width / 2f),
            bounds.min.y + bounds.size.y * (panel.y + panel.height / 2f),
            transform.position.z
        );

        float targetZoom = bounds.size.y * panel.height / 2f;

        while (
            Vector2.Distance(transform.position, targetPos) > 0.01f ||
            Mathf.Abs(cam.orthographicSize - targetZoom) > 0.01f
        )
        {
            transform.position = Vector3.Lerp(
                transform.position,
                targetPos,
                Time.deltaTime * moveSpeed
            );

            cam.orthographicSize = Mathf.Lerp(
                cam.orthographicSize,
                targetZoom,
                Time.deltaTime * zoomSpeed
            );

            yield return null;
        }
    }
}