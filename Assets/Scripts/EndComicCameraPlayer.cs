using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndComicCameraPlayer : MonoBehaviour
{
    public SpriteRenderer comicSprite;

    public Rect[] panels =
    {
        new Rect(0.0f, 0.707f, 0.488f, 0.293f),
        new Rect(0.505f, 0.707f, 0.488f, 0.293f),
        new Rect(0.0f, 0.396f, 0.488f, 0.303f),
        new Rect(0.504f, 0.396f, 0.488f, 0.303f),
        new Rect(0.001f, 0.001f, 0.996f, 0.38f)
    };

    public float moveSpeed = 2.5f;
    public float zoomSpeed = 2.5f;
    public float holdTime = 1.2f;

    Camera cam;
    Bounds bounds;

    void OnEnable()
    {
        if (comicSprite == null) return;

        cam = GetComponent<Camera>();
        cam.orthographic = true;
        bounds = comicSprite.bounds;

        StopAllCoroutines();
        StartCoroutine(PlayComic());
    }

    IEnumerator PlayComic()
    {
        foreach (Rect panel in panels)
        {
            yield return MoveToPanel(panel);
            yield return new WaitForSeconds(holdTime);
        }

        yield return new WaitForSeconds(1f);
        enabled = false;
        SceneManager.LoadScene(0);
    }

    IEnumerator MoveToPanel(Rect panel)
    {
        Vector3 targetPos = new Vector3(
            bounds.min.x + bounds.size.x * (panel.x + panel.width / 2f),
            bounds.min.y + bounds.size.y * (panel.y + panel.height / 2f),
            transform.position.z
        );

        float baseZoom = bounds.size.y * panel.height / 2f;
        float targetZoom = Mathf.Abs(cam.orthographicSize - baseZoom) < 0.25f
            ? baseZoom + 0.25f
            : baseZoom;

        yield return null;

        while (
            Vector2.Distance(transform.position, targetPos) > 0.001f ||
            Mathf.Abs(cam.orthographicSize - targetZoom) > 0.001f
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

        cam.orthographicSize = baseZoom;
    }
}