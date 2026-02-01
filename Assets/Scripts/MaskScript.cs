using UnityEngine;

public class MaskScript : MonoBehaviour
{
    [Header("Sprites")]
    public SpriteRenderer[] normalSprites;
    public SpriteRenderer[] maskedSprites;

    [Header("Mode Root Objects")]
    public GameObject normalModeObject;
    public GameObject maskedModeObject;

    [Header("Scripts To Toggle")]
    public MonoBehaviour[] disableInNormalMode;
    public MonoBehaviour[] disableInMaskedMode;

    [Header("Extra GameObjects To Toggle")]
    public GameObject[] disableObjectsInNormalMode;
    public GameObject[] disableObjectsInMaskedMode;

    [Header("Mask Visuals (instant on/off only)")]
    public GameObject blackMask;
    public GameObject whiteMask;

    public bool IsMaskedMode { get; private set; }

    int playerNormalLayer;
    int normalObstacleLayer;
    int playerMaskedLayer;
    int maskObstacleLayer;

    void Start()
    {
        playerNormalLayer   = LayerMask.NameToLayer("PlayerNormal");
        normalObstacleLayer = LayerMask.NameToLayer("NormalObstacle");
        playerMaskedLayer   = LayerMask.NameToLayer("PlayerMasked1");
        maskObstacleLayer   = LayerMask.NameToLayer("MaskObstacle");

        SetMask(false); // start in normal mode
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            SetMask(!IsMaskedMode);
        }
    }

    void SetMask(bool masked)
    {
        IsMaskedMode = masked;

        // Root objects
        if (normalModeObject)
            normalModeObject.SetActive(!masked);

        if (maskedModeObject)
            maskedModeObject.SetActive(masked);

        // Sprites
        ToggleSprites(normalSprites, !masked);
        ToggleSprites(maskedSprites, masked);

        // Scripts
        ToggleScripts(disableInNormalMode, masked);
        ToggleScripts(disableInMaskedMode, !masked);

        // Extra objects
        ToggleObjects(disableObjectsInNormalMode, !masked);
        ToggleObjects(disableObjectsInMaskedMode, masked);

        // Mask visuals (no animation, just visibility)
        if (blackMask)
            blackMask.SetActive(!masked);

        if (whiteMask)
            whiteMask.SetActive(masked);

        ApplyCollisionRules();
    }

    void ApplyCollisionRules()
    {
        // Masked player should only collide with masked obstacles
        SafeIgnore(playerMaskedLayer, maskObstacleLayer, !IsMaskedMode);

        // Normal player should only collide with normal obstacles
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

    void ToggleObjects(GameObject[] objects, bool state)
    {
        foreach (var obj in objects)
            if (obj) obj.SetActive(state);
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