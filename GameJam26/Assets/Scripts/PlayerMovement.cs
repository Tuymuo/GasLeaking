using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
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

        // Conectar con el Animator
        bool isMoving = input.magnitude > 0;
        animator.SetBool("isMoving", isMoving);   
    }
}