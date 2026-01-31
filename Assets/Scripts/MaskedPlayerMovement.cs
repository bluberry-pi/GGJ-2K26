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
    Vector2 lastNormalPos;
    
    void Start()
    {
        lastMaskedPos = maskedRb.position;
        lastNormalPos = normalRb.position;
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
            
            StartCoroutine(CopyMaskedVelocityAfterPhysics());
        }
        else
        {
            // NORMAL is leader - wait for physics then copy
            StartCoroutine(CopyNormalVelocityAfterPhysics());
        }
        
        lastMaskedPos = maskedRb.position;
        lastNormalPos = normalRb.position;
    }
    
    IEnumerator CopyMaskedVelocityAfterPhysics()
    {
        yield return new WaitForFixedUpdate();
        
        // Get masked's ACTUAL movement after physics
        Vector2 actualMovement = maskedRb.position - lastMaskedPos;
        Vector2 actualVelocity = actualMovement / Time.fixedDeltaTime;
        
        bool maskedMoved = actualMovement.sqrMagnitude > 0.000001f;
        
        if (maskedMoved || input != Vector2.zero)
            normalRb.linearVelocity = actualVelocity;
        else
            normalRb.linearVelocity = Vector2.zero;
    }
    
    IEnumerator CopyNormalVelocityAfterPhysics()
    {
        yield return new WaitForFixedUpdate();
        
        // Get normal's ACTUAL movement after physics (including slowdown from pushing)
        Vector2 actualMovement = normalRb.position - lastNormalPos;
        Vector2 actualVelocity = actualMovement / Time.fixedDeltaTime;
        
        bool normalMoved = actualMovement.sqrMagnitude > 0.000001f;
        
        if (normalMoved)
            maskedRb.linearVelocity = actualVelocity;
        else
            maskedRb.linearVelocity = Vector2.zero;
    }
}