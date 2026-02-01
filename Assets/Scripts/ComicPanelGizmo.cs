using UnityEngine;

[ExecuteAlways]
public class ComicPanelGizmo : MonoBehaviour
{
    [Range(0f, 1f)] public float x;
    [Range(0f, 1f)] public float y;
    [Range(0f, 1f)] public float width = 0.25f;
    [Range(0f, 1f)] public float height = 0.25f;

    SpriteRenderer sr;

    void OnEnable()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void OnDrawGizmos()
    {
        if (!sr) return;

        Bounds b = sr.bounds;

        Vector3 size = new Vector3(
            b.size.x * width,
            b.size.y * height,
            0
        );

        Vector3 center = new Vector3(
            b.min.x + b.size.x * (x + width / 2f),
            b.min.y + b.size.y * (y + height / 2f),
            0
        );

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(center, size);
    }
}