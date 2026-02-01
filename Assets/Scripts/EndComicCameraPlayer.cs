using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndComicCameraPlayer : MonoBehaviour
{
    public SpriteRenderer comicSprite;
    public Rect[] panels = {
        new Rect(0.0f, 0.707f, 0.488f, 0.293f),   // Panel 1 - Top Left
        new Rect(0.505f, 0.707f, 0.488f, 0.293f), // Panel 2 - Top Right
        new Rect(0.0f, 0.396f, 0.488f, 0.303f),   // Panel 3 - Middle Left
        new Rect(0.504f, 0.396f, 0.488f, 0.303f), // Panel 4 - Middle Right
        new Rect(0.109f, 0.01f, 0.782f, 0.38f)    // Panel 5 - Bottom (FIXED)
    };

    public float moveSpeed = 6f;
    public float zoomSpeed = 6f;
    public float holdTime = 1.2f;

    Camera cam;
    Bounds bounds;

    void OnEnable()
    {
        if (comicSprite == null)
        {
            Debug.LogError("Comic Sprite is not assigned!");
            return;
        }

        cam = GetComponent<Camera>();
        if (cam == null)
        {
            Debug.LogError("Camera component not found!");
            return;
        }

        cam.orthographic = true;
        bounds = comicSprite.bounds;

        Debug.Log($"<color=cyan>Comic Bounds: Min={bounds.min}, Max={bounds.max}, Size={bounds.size}</color>");
        Debug.Log($"<color=cyan>Starting camera position: {transform.position}</color>");
        Debug.Log($"<color=cyan>Starting orthographic size: {cam.orthographicSize}</color>");

        // Debug all panel calculations upfront
        Debug.Log("<color=yellow>=== PANEL CALCULATIONS ===</color>");
        for (int i = 0; i < panels.Length; i++)
        {
            Vector3 calcPos = new Vector3(
                bounds.min.x + bounds.size.x * (panels[i].x + panels[i].width * 0.5f),
                bounds.min.y + bounds.size.y * (panels[i].y + panels[i].height * 0.5f),
                transform.position.z
            );
            float calcZoom = bounds.size.y * panels[i].height * 0.5f;
            
            Debug.Log($"<color=yellow>Panel {i + 1}: Rect({panels[i].x:F3}, {panels[i].y:F3}, {panels[i].width:F3}, {panels[i].height:F3}) → Pos={calcPos}, Zoom={calcZoom:F2}</color>");
        }
        Debug.Log("<color=yellow>========================</color>");

        StopAllCoroutines();
        StartCoroutine(PlayComic());
    }

    IEnumerator PlayComic()
    {
        Debug.Log($"<color=green>Starting comic playback with {panels.Length} panels</color>");

        for (int i = 0; i < panels.Length; i++)
        {
            Debug.Log($"<color=magenta>--- Moving to Panel {i + 1}/{panels.Length} ---</color>");
            Debug.Log($"<color=magenta>Panel rect: x={panels[i].x:F3}, y={panels[i].y:F3}, w={panels[i].width:F3}, h={panels[i].height:F3}</color>");
            
            yield return MoveToPanel(panels[i], i);
            
            Debug.Log($"<color=green>Holding on panel {i + 1} for {holdTime} seconds</color>");
            yield return new WaitForSeconds(holdTime);
        }

        Debug.Log("<color=cyan>Comic complete, waiting before scene transition...</color>");
        yield return new WaitForSeconds(1f);
        
        Debug.Log("<color=red>Loading scene 0...</color>");
        enabled = false;
        SceneManager.LoadScene(0);
    }

    IEnumerator MoveToPanel(Rect panel, int panelIndex)
    {
        // Calculate target position in world space
        Vector3 targetPos = new Vector3(
            bounds.min.x + bounds.size.x * (panel.x + panel.width * 0.5f),
            bounds.min.y + bounds.size.y * (panel.y + panel.height * 0.5f),
            transform.position.z
        );

        // Calculate target zoom
        float targetZoom = bounds.size.y * panel.height * 0.5f;

        float startDistance = Vector2.Distance(transform.position, targetPos);
        float startZoomDiff = Mathf.Abs(cam.orthographicSize - targetZoom);

        Debug.Log($"<color=white>Panel {panelIndex + 1} - Target Position: {targetPos}</color>");
        Debug.Log($"<color=white>Panel {panelIndex + 1} - Target Zoom: {targetZoom:F3}</color>");
        Debug.Log($"<color=white>Panel {panelIndex + 1} - Current Position: {transform.position}</color>");
        Debug.Log($"<color=white>Panel {panelIndex + 1} - Current Zoom: {cam.orthographicSize:F3}</color>");
        Debug.Log($"<color=orange>Panel {panelIndex + 1} - Distance to travel: {startDistance:F3}</color>");
        Debug.Log($"<color=orange>Panel {panelIndex + 1} - Zoom difference: {startZoomDiff:F3}</color>");

        float minDuration = 0.4f;
        float timer = 0f;

        while (
            timer < minDuration ||
            Vector2.Distance(transform.position, targetPos) > 0.001f ||
            Mathf.Abs(cam.orthographicSize - targetZoom) > 0.001f
        )
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPos,
                moveSpeed * Time.deltaTime
            );

            cam.orthographicSize = Mathf.MoveTowards(
                cam.orthographicSize,
                targetZoom,
                zoomSpeed * Time.deltaTime
            );

            timer += Time.deltaTime;
            yield return null;
        }

        // Ensure we're exactly at target
        transform.position = targetPos;
        cam.orthographicSize = targetZoom;

        Debug.Log($"<color=lime>Panel {panelIndex + 1} - ✓ ARRIVED! Final Position: {transform.position}, Final Zoom: {cam.orthographicSize:F3}</color>");
    }
}