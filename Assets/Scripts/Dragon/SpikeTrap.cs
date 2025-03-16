using System.Collections;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] private float triggerDistance = 2f; // Distance at which trap activates
    [SerializeField] private float riseHeight = 1.5f; // How high the trap moves
    [SerializeField] private float stayUpTime = 0.5f; // Time trap stays up
    [SerializeField] private float cooldownTime = 1f; // Cooldown before trap can activate again
    [SerializeField] private float riseDuration = 0.5f; // Time it takes to rise
    [SerializeField] private float lowerDuration = 0.5f; // Time it takes to lower

    private bool isActivated = false;
    private bool isCooldown = false;
    private Vector2 startPosition;
    private Vector2 targetPosition;
    private GameObject player;
    private AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        startPosition = transform.position;
        targetPosition = new Vector2(startPosition.x, startPosition.y + riseHeight);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (!isActivated && !isCooldown && player != null)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance < triggerDistance)
            {
                StartCoroutine(ActivateTrap());
            }
        }
    }

    private IEnumerator ActivateTrap()
    {
        isActivated = true;

        yield return MoveTrap(startPosition, targetPosition, riseDuration);

        yield return new WaitForSeconds(stayUpTime);

        yield return MoveTrap(targetPosition, startPosition, lowerDuration);

        isActivated = false;
        isCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        isCooldown = false;
    }

    private IEnumerator MoveTrap(Vector2 from, Vector2 to, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            transform.position = Vector2.Lerp(from, to, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = to;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DontDestroyOnLoad(gameObject);
            _audioSource.Play();
            Destroy(GetComponent<SpriteRenderer>());
            Destroy(GetComponent<PolygonCollider2D>());
            GameManager.Instance.LoseLife();
        }
    }
}
