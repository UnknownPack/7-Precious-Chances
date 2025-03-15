using System.Collections;
using UnityEngine;

public class DragonController : MonoBehaviour
{
    public BoxCollider2D floorCollider2D;
    public float moveSpeed;
    public float edgePadding;
    public GameObject fireballPrefab;
    public Transform firePoint;
    public float fireInterval;
    public float fireballSpeed;
    float minX;
    float maxX;
    int direction = 1;

    void Start()
    {
        if (floorCollider2D != null)
        {
            Bounds b = floorCollider2D.bounds;
            minX = b.min.x + edgePadding;
            maxX = b.max.x - edgePadding;
        }
        StartCoroutine(FireballRoutine());
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
            Fire();
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
        }
    }
}
