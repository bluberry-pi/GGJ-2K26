using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CratePull : MonoBehaviour
{
    public float followSpeed = 10f;

    private Transform player;
    private Rigidbody2D rb;
    private bool canToggle;
    private bool isPulling;
    private Vector2 offset;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    void Update()
    {
        if (player == null) return;

        if (canToggle && Input.GetKeyDown(KeyCode.E))
        {
            isPulling = !isPulling;

            if (isPulling)
            {
                offset = rb.position - (Vector2)player.position;
                rb.mass = 0.5f; // lighter while pulled, less bullying
            }
            else
            {
                rb.mass = 2f; // back to normal
            }
        }
    }

    void FixedUpdate()
    {
        if (!isPulling || player == null) return;

        Vector2 targetPos = (Vector2)player.position + offset;
        Vector2 newPos = Vector2.Lerp(
            rb.position,
            targetPos,
            followSpeed * Time.fixedDeltaTime
        );

        rb.MovePosition(newPos);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("NormalPlayer") || other.CompareTag("MaskedPlayer"))
        {
            player = other.transform;
            canToggle = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform == player)
        {
            canToggle = false;
            // pulling stays active, as requested
        }
    }
}