using UnityEngine;
using System.Collections.Generic;

public class TrapSpawner : MonoBehaviour
{
    [SerializeField] private GameObject trapPrefab;
    [SerializeField] private int trapsPerLevel = 1;
    [SerializeField] private int maxTraps = 7;
    [SerializeField] private Vector2 spawnAreaMin;
    [SerializeField] private Vector2 spawnAreaMax;
    [SerializeField] private float minDistanceBetweenTraps = 2.5f;

    private List<Vector2> spawnedTrapPositions = new List<Vector2>();

    void Start()
    {
        SpawnTraps();
    }

    void SpawnTraps()
    {
        int level = GameManager.Instance.GetCurrentLevel();
        int trapsToSpawn = Mathf.Min(trapsPerLevel * level, maxTraps);

        spawnedTrapPositions.Clear();

        for (int i = 0; i < trapsToSpawn; i++)
        {
            Vector2 spawnPosition = GetValidSpawnPosition();
            if (spawnPosition != Vector2.zero)
            {
                spawnedTrapPositions.Add(spawnPosition);
                Instantiate(trapPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }

    private Vector2 GetValidSpawnPosition()
    {
        int maxAttempts = 30;
        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            Vector2 randomPosition = new Vector2(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                spawnAreaMin.y
            );

            if (IsFarEnoughFromOtherTraps(randomPosition))
            {
                return randomPosition;
            }
        }
        Debug.LogWarning("Failed to find valid trap spawn after " + maxAttempts + " attempts.");
        return Vector2.zero;
    }

    private bool IsFarEnoughFromOtherTraps(Vector2 newPosition)
    {
        foreach (Vector2 existingPosition in spawnedTrapPositions)
        {
            float distance = Vector2.Distance(newPosition, existingPosition);
            if (distance < minDistanceBetweenTraps)
            {
                return false;
            }
        }
        return true;
    }
}
