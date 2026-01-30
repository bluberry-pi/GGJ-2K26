using UnityEngine;

public class MaskScript : MonoBehaviour
{
    public SpriteRenderer[] normalSprites;
    public SpriteRenderer[] maskedSprites;

    public bool IsMaskedMode { get; private set; }

    int playerNormalLayer;
    int normalObstacleLayer;

    void Start()
    {
        playerNormalLayer = LayerMask.NameToLayer("PlayerNormal");
        normalObstacleLayer = LayerMask.NameToLayer("NormalObstacle");

        SetMask(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            ToggleMask();
    }

    void ToggleMask()
    {
        bool normalEnabled = normalSprites[0].enabled;
        SetMask(normalEnabled);
    }

    void SetMask(bool showMask)
    {
        IsMaskedMode = showMask;

        foreach (var s in normalSprites)
            if (s) s.enabled = !showMask;

        foreach (var s in maskedSprites)
            if (s) s.enabled = showMask;

        Physics2D.IgnoreLayerCollision(
            playerNormalLayer,
            normalObstacleLayer,
            showMask
        );
    }
}