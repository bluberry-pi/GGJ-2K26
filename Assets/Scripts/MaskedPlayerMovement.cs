using UnityEngine;
using System.Collections;

public class MaskedPlayerMovement : MonoBehaviour
{
    public float moveSpeed = 8f;
    public float rotationSpeed = 720f; // Degrees per second
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
        
        // Rotate towards movement direction (only when MASKED has control)
        if (maskScript.IsMaskedMode && input != Vector2.zero)
        {
            RotateTowards(input);
        }
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
        {
            normalRb.linearVelocity = actualVelocity;
            
            // Rotate normal player to match masked's rotation
            if (actualMovement != Vector2.zero)
            {
                RotateTransform(normalMovement.transform, actualMovement);
            }
        }
        else
        {
            normalRb.linearVelocity = Vector2.zero;
        }
    }
    
    IEnumerator CopyNormalVelocityAfterPhysics()
    {
        yield return new WaitForFixedUpdate();
        
        // Get normal's ACTUAL movement after physics
        Vector2 actualMovement = normalRb.position - lastNormalPos;
        Vector2 actualVelocity = actualMovement / Time.fixedDeltaTime;
        
        bool normalMoved = actualMovement.sqrMagnitude > 0.000001f;
        
        if (normalMoved)
        {
            maskedRb.linearVelocity = actualVelocity;
            
            // Rotate masked player to match normal's rotation
            if (actualMovement != Vector2.zero)
            {
                RotateTransform(transform, actualMovement);
            }
        }
        else
        {
            maskedRb.linearVelocity = Vector2.zero;
        }
    }
    
    void RotateTowards(Vector2 direction)
    {
        // Calculate target angle from direction vector
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        // Adjust for sprite orientation (assuming sprite faces right by default)
        targetAngle -= 90f;
        
        // Smoothly rotate towards target angle
        float currentAngle = transform.eulerAngles.z;
        float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);
        
        transform.rotation = Quaternion.Euler(0, 0, newAngle);
    }
    
    void RotateTransform(Transform target, Vector2 direction)
    {
        // Calculate target angle from direction vector
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        // Adjust for sprite orientation
        targetAngle -= 90f;
        
        // Smoothly rotate towards target angle
        float currentAngle = target.eulerAngles.z;
        float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);
        
        target.rotation = Quaternion.Euler(0, 0, newAngle);
    }
}