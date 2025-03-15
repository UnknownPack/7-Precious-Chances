using System.Collections.Generic;
using UnityEngine;

public class TreasureManager2D : MonoBehaviour
{
    public List<TreasureData> treasureDataList;
    public Rect floorArea;
    public float minDistanceBetweenTreasures = 2f;
    public int maxAttemptsPerTreasure = 50;
    private List<GameObject> spawnedTreasures = new List<GameObject>();

    void Start()
    {
        SpawnAllTreasures();
    }

    public void SpawnAllTreasures()
    {
        ClearSpawnedTreasures();
        for (int i = 0; i < treasureDataList.Count; i++)
        {
            TreasureData data = treasureDataList[i];
            if (data.prefab == null)
            {
                Debug.LogWarning($"[{data.type}] Prefab is null, skip generating!");
                continue;
            }
            bool foundPosition = false;
            Vector2 finalPos = Vector2.zero;
            for (int attempt = 0; attempt < maxAttemptsPerTreasure; attempt++)
            {
                Vector2 candidatePos = GetRandomPositionInFloor();
                if (IsPositionValid(candidatePos, minDistanceBetweenTreasures))
                {
                    finalPos = candidatePos;
                    foundPosition = true;
                    break;
                }
            }
            if (!foundPosition)
            {
                Debug.LogWarning($"[{data.type}] Could not find valid position, forcing placement!");
                finalPos = GetRandomPositionInFloor();
            }
            GameObject treasureObj = Instantiate(data.prefab, finalPos, Quaternion.identity);
            spawnedTreasures.Add(treasureObj);
        }
    }

    private Vector2 GetRandomPositionInFloor()
    {
        float x = Random.Range(floorArea.xMin, floorArea.xMax);
        float y = Random.Range(floorArea.yMin, floorArea.yMax);
        return new Vector2(x, y);
    }

    private bool IsPositionValid(Vector2 candidatePos, float minDist)
    {
        foreach (GameObject existing in spawnedTreasures)
        {
            if (existing == null) continue;
            float dist = Vector2.Distance(candidatePos, existing.transform.position);
            if (dist < minDist)
            {
                return false;
            }
        }
        return true;
    }

    public void ClearSpawnedTreasures()
    {
        foreach (var t in spawnedTreasures)
        {
            if (t != null) Destroy(t);
        }
        spawnedTreasures.Clear();
    }
}
