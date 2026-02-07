using UnityEngine;

public class Bullet : MonoBehaviour
{
    
    public float speed = 20f;
    public Rigidbody rb;
    public int damage = 100;
    void Start()
    {
        rb.linearVelocity = transform.right * speed;    
    }

    private void OnTriggerEnter(Collider hitInfo)
    {
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if(enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
