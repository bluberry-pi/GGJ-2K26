using UnityEngine;
using System.Collections;

public class MaskScript : MonoBehaviour
{
    [Header("Sprites")]
    public SpriteRenderer[] normalSprites;
    public SpriteRenderer[] maskedSprites;

    [Header("Mode GameObjects")]
    public GameObject normalModeObject;
    public GameObject maskedModeObject;

    [Header("Scripts To Toggle")]
    public MonoBehaviour[] disableInNormalMode;
    public MonoBehaviour[] disableInMaskedMode;

    [Header("Mask Visuals")]
    public GameObject blackMask;
    public GameObject whiteMask;

    [Header("Black Mask Target")]
    public Vector3 blackMaskTargetPosition;
    public Vector3 blackMaskTargetScale = Vector3.one;

    [Header("White Mask Target")]
    public Vector3 whiteMaskTargetPosition;
    public Vector3 whiteMaskTargetScale = Vector3.one;

    public float maskMoveSpeed = 5f;
    public float maskScaleSpeed = 5f;

    public bool IsMaskedMode { get; private set; }

    int playerNormalLayer;
    int normalObstacleLayer;
    int playerMaskedLayer;
    int maskObstacleLayer;

    bool isAnimating;

    Vector3 blackMaskStartPos;
    Vector3 blackMaskStartScale;
    Vector3 whiteMaskStartPos;
    Vector3 whiteMaskStartScale;

    void Start()
    {
        playerNormalLayer = LayerMask.NameToLayer("PlayerNormal");
        normalObstacleLayer = LayerMask.NameToLayer("NormalObstacle");
        playerMaskedLayer = LayerMask.NameToLayer("PlayerMasked1");
        maskObstacleLayer = LayerMask.NameToLayer("MaskObstacle");

        if (blackMask)
        {
            blackMaskStartPos = blackMask.transform.position;
            blackMaskStartScale = blackMask.transform.localScale;
        }

        if (whiteMask)
        {
            whiteMaskStartPos = whiteMask.transform.position;
            whiteMaskStartScale = whiteMask.transform.localScale;
        }

        SetMask(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !isAnimating)
        {
            if (!IsMaskedMode)
                StartCoroutine(PlayMaskTransition(blackMask, true));
            else
                StartCoroutine(PlayMaskTransition(whiteMask, false));
        }
    }

    IEnumerator PlayMaskTransition(GameObject mask, bool toMasked)
    {
        if (!mask) yield break;

        isAnimating = true;

        Transform t = mask.transform;

        Vector3 targetPos;
        Vector3 targetScale;

        if (mask == blackMask)
        {
            targetPos = blackMaskTargetPosition;
            targetScale = blackMaskTargetScale;
        }
        else
        {
            targetPos = whiteMaskTargetPosition;
            targetScale = whiteMaskTargetScale;
        }

        // STEP 1: MOVE FIRST
        while (Vector3.Distance(t.position, targetPos) > 0.01f)
        {
            t.position = Vector3.MoveTowards(
                t.position,
                targetPos,
                maskMoveSpeed * Time.deltaTime
            );
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        // STEP 2: SCALE AFTER MOVE
        while (Vector3.Distance(t.localScale, targetScale) > 0.01f)
        {
            t.localScale = Vector3.MoveTowards(
                t.localScale,
                targetScale,
                maskScaleSpeed * Time.deltaTime
            );
            yield return null;
        }

        // SNAP BACK
        if (mask == blackMask)
        {
            t.position = blackMaskStartPos;
            t.localScale = blackMaskStartScale;
        }
        else
        {
            t.position = whiteMaskStartPos;
            t.localScale = whiteMaskStartScale;
        }

        SetMask(toMasked);
        isAnimating = false;
    }

    void SetMask(bool masked)
    {
        IsMaskedMode = masked;

        if (normalModeObject)
            normalModeObject.SetActive(!masked);

        if (maskedModeObject)
            maskedModeObject.SetActive(masked);

        ToggleSprites(normalSprites, !masked);
        ToggleSprites(maskedSprites, masked);

        ToggleScripts(disableInNormalMode, masked);
        ToggleScripts(disableInMaskedMode, !masked);

        ApplyCollisionRules();
    }

    void ApplyCollisionRules()
    {
        SafeIgnore(playerMaskedLayer, maskObstacleLayer, !IsMaskedMode);
        SafeIgnore(playerNormalLayer, normalObstacleLayer, IsMaskedMode);
    }

    void ToggleSprites(SpriteRenderer[] sprites, bool state)
    {
        foreach (var s in sprites)
            if (s) s.enabled = state;
    }

    void ToggleScripts(MonoBehaviour[] scripts, bool state)
    {
        foreach (var script in scripts)
            if (script) script.enabled = state;
    }

    void SafeIgnore(int layerA, int layerB, bool ignore)
    {
        if (IsValidLayer(layerA) && IsValidLayer(layerB))
            Physics2D.IgnoreLayerCollision(layerA, layerB, ignore);
    }

    bool IsValidLayer(int layer)
    {
        return layer >= 0 && layer <= 31;
    }
}