using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private bool facingRight = true;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        float x = Input.GetAxisRaw("Horizontal"); // A/D or left/right arrow
        float z = Input.GetAxisRaw("Vertical");   // W/S or up/down arrow

        Vector3 input = new Vector3(x, 0f, z).normalized;
        Vector3 velocity = input * moveSpeed;
        
        rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);

        if (x > 0 && !facingRight)
        {
            Flip();
        }
        else if (x < 0 && facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }
}
