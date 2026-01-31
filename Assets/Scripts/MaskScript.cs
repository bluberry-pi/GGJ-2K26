using UnityEngine;

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

    public bool IsMaskedMode { get; private set; }

    int playerNormalLayer;
    int normalObstacleLayer;
    int playerMaskedLayer;   // PlayerMasked1
    int maskObstacleLayer;   // MaskObstacle

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
            SetMask(!IsMaskedMode);
    }

    void SetMask(bool masked)
    {
        IsMaskedMode = masked;

        // Toggle mode GameObjects
        if (normalModeObject)
            normalModeObject.SetActive(!masked);

        if (maskedModeObject)
            maskedModeObject.SetActive(masked);

        // Toggle sprites
        ToggleSprites(normalSprites, !masked);
        ToggleSprites(maskedSprites, masked);

        // Toggle scripts
        ToggleScripts(disableInNormalMode, masked);
        ToggleScripts(disableInMaskedMode, !masked);

        // Collision rules
        ApplyCollisionRules();
    }

    void ApplyCollisionRules()
    {
        // NORMAL MODE:
        // PlayerMasked1 ignores MaskObstacle
        SafeIgnore(playerMaskedLayer, maskObstacleLayer, !IsMaskedMode);

        // OPTIONAL WORLD SEPARATION:
        // PlayerNormal ignores NormalObstacle in masked mode
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