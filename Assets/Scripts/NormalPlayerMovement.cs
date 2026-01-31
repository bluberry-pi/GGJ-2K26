using UnityEngine;

public class NormalPlayerMovement : MonoBehaviour
{
    public float moveSpeed = 8f;
    public float rotationSpeed = 720f; // Degrees per second
    public MaskScript maskScript;
    public Rigidbody2D rb;
    
    Vector2 input;
    Vector2 lastPhysicsPos;
    
    public bool DidMoveThisFrame { get; private set; }
    public Vector2 CurrentInput { get; private set; } // Share input with masked player
    
    void Awake()
    {
        lastPhysicsPos = rb.position;
    }
    
    void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input = input.normalized;
        CurrentInput = input;
        
        // Rotate towards movement direction (only when NORMAL has control)
        if (!maskScript.IsMaskedMode && input != Vector2.zero)
        {
            RotateTowards(input);
        }
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
    
    void RotateTowards(Vector2 direction)
    {
        // Calculate target angle from direction vector
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        // Adjust for sprite orientation (assuming sprite faces right by default)
        // Remove the -90 if your sprite naturally faces up
        targetAngle -= 90f;
        
        // Smoothly rotate towards target angle
        float currentAngle = transform.eulerAngles.z;
        float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);
        
        transform.rotation = Quaternion.Euler(0, 0, newAngle);
    }
}