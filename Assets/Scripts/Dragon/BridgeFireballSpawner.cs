using UnityEngine;

public class BridgeFireballSpawner : MonoBehaviour
{

    public GameObject fireballPrefab;
    public float fireballSpeed = 5f;
    public float spawnInterval = 2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("SpawnFireball", 1f, spawnInterval);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnFireball() {
        if (fireballPrefab != null) {
            GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
            fireball.GetComponent<Fireball>().speed = fireballSpeed;
        }
        else {
            Debug.LogError("Fireball prefab is not assigned");
        }
    }

}
