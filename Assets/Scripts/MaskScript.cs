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

    public bool IsMaskedMode { get; private set; }

    int playerNormalLayer;
    int normalObstacleLayer;
    int buttonLayer;
    int maskedPlayerLayer;
    int boxLayer;

    void Start()
    {
        playerNormalLayer   = LayerMask.NameToLayer("PlayerNormal");
        normalObstacleLayer = LayerMask.NameToLayer("NormalObstacle");
        buttonLayer         = LayerMask.NameToLayer("Button");
        maskedPlayerLayer   = LayerMask.NameToLayer("MaskedPlayer");
        boxLayer            = LayerMask.NameToLayer("Box");

        SetMask(false);
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

        if (normalModeObject)
            normalModeObject.SetActive(!showMask);

        if (maskedModeObject)
            maskedModeObject.SetActive(showMask);

        foreach (var s in normalSprites)
            if (s) s.enabled = !showMask;

        foreach (var s in maskedSprites)
            if (s) s.enabled = showMask;

        // 🔁 Disable scripts in NORMAL mode, enable in MASK mode
        foreach (var script in disableInNormalMode)
        {
            if (script)
                script.enabled = showMask;
        }

        bool ignoreInNormal = !showMask;

        SafeIgnore(playerNormalLayer, normalObstacleLayer, showMask);
        SafeIgnore(buttonLayer, maskedPlayerLayer, ignoreInNormal);
        SafeIgnore(maskedPlayerLayer, boxLayer, ignoreInNormal);
    }

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