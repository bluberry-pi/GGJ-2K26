using UnityEngine;

public class MaskScript : MonoBehaviour
{
    [Header("Sprites")]
    public SpriteRenderer[] normalSprites;
    public SpriteRenderer[] maskedSprites;

    public bool IsMaskedMode { get; private set; }

    int playerNormalLayer;
    int normalObstacleLayer;
    int buttonLayer;
    int maskedPlayerLayer;
    int boxLayer;

    void Start()
    {
        // Cache layers (NameToLayer returns -1 if layer doesn't exist)
        playerNormalLayer   = LayerMask.NameToLayer("PlayerNormal");
        normalObstacleLayer = LayerMask.NameToLayer("NormalObstacle");
        buttonLayer         = LayerMask.NameToLayer("Button");
        maskedPlayerLayer   = LayerMask.NameToLayer("MaskedPlayer");
        boxLayer            = LayerMask.NameToLayer("Box");

        // Debug once. If you see -1, your layer is missing or misspelled.
        Debug.Log(
            $"Layers → PlayerNormal:{playerNormalLayer}, " +
            $"NormalObstacle:{normalObstacleLayer}, " +
            $"Button:{buttonLayer}, " +
            $"MaskedPlayer:{maskedPlayerLayer}, " +
            $"Box:{boxLayer}"
        );

        SetMask(false); // start in Normal mode
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            ToggleMask();
    }

    void ToggleMask()
    {
        SetMask(!IsMaskedMode);
    }

    void SetMask(bool showMask)
    {
        IsMaskedMode = showMask;

        // ---------- VISUALS ----------
        foreach (var s in normalSprites)
            if (s) s.enabled = !showMask;

        foreach (var s in maskedSprites)
            if (s) s.enabled = showMask;

        // ---------- PHYSICS ----------
        // Normal mode → ignore extra collisions
        // Masked mode → restore collisions
        bool ignoreInNormal = !showMask;

        SafeIgnore(playerNormalLayer, normalObstacleLayer, showMask);
        SafeIgnore(buttonLayer, maskedPlayerLayer, ignoreInNormal);
        SafeIgnore(maskedPlayerLayer, boxLayer, ignoreInNormal);
    }

    // ---------- SAFETY ----------
    void SafeIgnore(int layerA, int layerB, bool ignore)
    {
        if (IsValidLayer(layerA) && IsValidLayer(layerB))
        {
            Physics2D.IgnoreLayerCollision(layerA, layerB, ignore);
        }
    }

    bool IsValidLayer(int layer)
    {
        return layer >= 0 && layer <= 31;
    }
}