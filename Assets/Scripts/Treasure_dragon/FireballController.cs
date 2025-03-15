using UnityEngine;

public class FireballController : MonoBehaviour
{
    public float speed = 5f;

    public float lifeTime = 3f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.gravityScale = 0;       
            rb.linearVelocity = Vector2.up * speed; 
        }

        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
    }
}
