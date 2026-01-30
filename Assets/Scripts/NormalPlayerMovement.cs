using UnityEngine;

public class NormalPlayerMovement : MonoBehaviour
{
    public float moveSpeed = 8f;
    public MaskScript maskScript;

    public Rigidbody2D rb;

    Vector2 input;
    Vector2 lastPhysicsPos;

    public bool DidMoveThisFrame { get; private set; }

    void Awake()
    {
        lastPhysicsPos = rb.position;
    }

    void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input = input.normalized;
    }

    void FixedUpdate()
    {
        // Only move if NORMAL has authority
        if (!maskScript.IsMaskedMode)
            rb.linearVelocity = input * moveSpeed;

        Vector2 currentPos = rb.position;
        DidMoveThisFrame =
            Vector2.SqrMagnitude(currentPos - lastPhysicsPos) > 0.000001f;

        lastPhysicsPos = currentPos;
    }
}