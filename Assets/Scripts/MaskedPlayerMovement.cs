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
    Vector2 lastNormalPos; // Track normal player's actual movement
    
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
            // NORMAL is leader - copy ACTUAL velocity after physics
            if (normalMovement.DidMoveThisFrame)
            {
                // Get the actual movement that happened (after collision resolution)
                Vector2 actualMovement = normalRb.position - lastNormalPos;
                Vector2 actualVelocity = actualMovement / Time.fixedDeltaTime;
                
                maskedRb.linearVelocity = actualVelocity;
            }
            else
            {
                maskedRb.linearVelocity = Vector2.zero;
            }
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
}