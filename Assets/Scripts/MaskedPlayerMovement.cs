using UnityEngine;

public class MaskedPlayerMovement : MonoBehaviour
{
    public float moveSpeed = 8f;

    public Rigidbody2D maskedRb;
    public Rigidbody2D normalRb;

    public MaskScript maskScript;
    public NormalPlayerMovement normalMovement;

    Vector2 input;
    Vector2 lastMaskedPos;

    void Start()
    {
        lastMaskedPos = maskedRb.position;
    }

    void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input = input.normalized;
    }

    void FixedUpdate()
    {
        if (maskScript.IsMaskedMode)
        {
            // MASKED is leader
            maskedRb.linearVelocity = input * moveSpeed;

            bool maskedMoved =
                Vector2.SqrMagnitude(maskedRb.position - lastMaskedPos) > 0.000001f;

            if (maskedMoved)
                normalRb.linearVelocity = maskedRb.linearVelocity;
            else
                normalRb.linearVelocity = Vector2.zero;
        }
        else
        {
            // NORMAL is leader
            if (normalMovement.DidMoveThisFrame)
                maskedRb.linearVelocity = normalRb.linearVelocity;
            else
                maskedRb.linearVelocity = Vector2.zero;
        }

        lastMaskedPos = maskedRb.position;
    }
}