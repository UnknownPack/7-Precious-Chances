using UnityEngine;

public class Fireball : MonoBehaviour
{

    public float speed = 5f;
    private AudioSource _audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        if (transform.position.x > 15)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other) { 
        if (other.CompareTag("Player")) {
            DontDestroyOnLoad(gameObject);
            GameManager.Instance.LoseLife();
            _audioSource.Play();
            Destroy(GetComponent<SpriteRenderer>());
            Destroy(GetComponent<CircleCollider2D>());
            Debug.Log("Fireball hit player");
        }

        if (other.CompareTag("Cover"))
        {
            Debug.Log("hit a piece of cover");
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Cover"))
        {
            Debug.Log("hit a piece of cover");
            Destroy(gameObject);
        }
    }
}
