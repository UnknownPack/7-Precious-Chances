using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // 如果需要判断场景名

public class DragonController : MonoBehaviour
{
    public float moveSpeed;
    public float minX = -4.94f;
    public float maxX = 5.05f;
    public GameObject fireballPrefab;
    public Transform firePoint;
    public float fireInterval;
    public float fireballSpeed;
    int direction = 1;
    private Animator animator;
    private AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        StartCoroutine(FireballRoutine());
        animator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        transform.Translate(Vector2.right * moveSpeed * direction * Time.deltaTime);
        float x = transform.position.x;
        if (x <= minX || x >= maxX)
        {
            direction *= -1;
            Vector3 s = transform.localScale;
            s.x *= -1;
            transform.localScale = s;
            float c = Mathf.Clamp(x, minX, maxX);
            transform.position = new Vector3(c, transform.position.y, transform.position.z);
        }
    }

    IEnumerator FireballRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireInterval);
            if (!GameManager.Instance.IsPlayerBehindDoor)
            {
                Fire();
            }
        }
    }


    void Fire()
    {
       
        if (fireballPrefab != null && firePoint != null)
        {
            GameObject f = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D r = f.GetComponent<Rigidbody2D>();
            if (r != null)
            {
                r.gravityScale = 0;
                r.linearVelocity = Vector2.up * fireballSpeed;
            }
            animator.SetTrigger("FireBall");
            _audioSource.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.LoseLife();

            // Scene currentScene = SceneManager.GetActiveScene();
            // if (currentScene.name == "TreasureRoom")
            // {
            //     GameManager.Instance.TeleportPlayerToDoor();
            // }
      
        }
    }
}
