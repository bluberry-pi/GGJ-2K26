using UnityEngine;
using System.Collections;

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
            // MASKED is leader - apply input
            maskedRb.linearVelocity = input * moveSpeed;
            
            // Start coroutine to copy velocity AFTER physics resolves
            StartCoroutine(CopyMaskedVelocityAfterPhysics());
        }
        else
        {
            // NORMAL is leader - copy its ACTUAL velocity (physics already resolved)
            if (normalMovement.DidMoveThisFrame)
                maskedRb.linearVelocity = normalRb.linearVelocity;
            else
                maskedRb.linearVelocity = Vector2.zero;
        }
        
        lastMaskedPos = maskedRb.position;
    }
    
    IEnumerator CopyMaskedVelocityAfterPhysics()
    {
        // Wait for physics to finish this frame
        yield return new WaitForFixedUpdate();
        
        // NOW copy the actual velocity (after collisions/friction applied)
        bool maskedMoved =
            Vector2.SqrMagnitude(maskedRb.position - lastMaskedPos) > 0.000001f;
        
        if (maskedMoved || input != Vector2.zero)
            normalRb.linearVelocity = maskedRb.linearVelocity;
        else
            normalRb.linearVelocity = Vector2.zero;
    }
}