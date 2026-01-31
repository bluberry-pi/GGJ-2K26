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
    int buttonLayer;
    int playerMaskedLayer;     // PlayerMasked1
    int maskObstacleLayer;     // MaskObstacle
    int boxLayer;

    void Start()
    {
        playerNormalLayer   = LayerMask.NameToLayer("PlayerNormal");
        normalObstacleLayer = LayerMask.NameToLayer("NormalObstacle");
        buttonLayer         = LayerMask.NameToLayer("Button");
        playerMaskedLayer   = LayerMask.NameToLayer("PlayerMasked1");
        maskObstacleLayer   = LayerMask.NameToLayer("MaskObstacle");
        boxLayer            = LayerMask.NameToLayer("Box");

        SetMask(false); // start in normal mode
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

        // Toggle mode objects
        if (normalModeObject)
            normalModeObject.SetActive(!showMask);

        if (maskedModeObject)
            maskedModeObject.SetActive(showMask);

        // Toggle sprites
        foreach (var s in normalSprites)
            if (s) s.enabled = !showMask;

        foreach (var s in maskedSprites)
            if (s) s.enabled = showMask;

        // Toggle scripts
        foreach (var script in disableInNormalMode)
            if (script) script.enabled = showMask;

        foreach (var script in disableInMaskedMode)
            if (script) script.enabled = !showMask;

        bool ignoreInNormal = !showMask;

        /*
         * NORMAL MODE
         * PlayerMasked1 ignores MaskObstacle
         */
        SafeIgnore(playerMaskedLayer, maskObstacleLayer, ignoreInNormal);

        /*
         * MASKED MODE
         * Restore collisions
         */
        SafeIgnore(playerMaskedLayer, maskObstacleLayer, false);

        /*
         * OPTIONAL: Normal world separation
         */
        SafeIgnore(playerNormalLayer, normalObstacleLayer, showMask);
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