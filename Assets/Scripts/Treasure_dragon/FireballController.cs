using UnityEngine;

public class FireballController : MonoBehaviour
{
    public float speed = 5f;

    public float lifeTime = 3f;

    private Rigidbody2D rb;
    private AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.gravityScale = 0;       
            rb.linearVelocity = Vector2.up * speed; 
        }

        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DontDestroyOnLoad(gameObject);
            _audioSource.Play();
            Destroy(GetComponent<SpriteRenderer>());
            Destroy(GetComponent<CircleCollider2D>());
            GameManager.Instance.LoseLife();
        }
        if (other.CompareTag("Cover"))
        {
            Destroy(gameObject);
        }
    }

}
