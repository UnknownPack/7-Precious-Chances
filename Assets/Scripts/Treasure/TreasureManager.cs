using System.Collections.Generic;
using UnityEngine;

public class TreasureManager2D : MonoBehaviour
{
    public List<TreasureData> treasureDataList;
    public BoxCollider2D floorCollider2D;

    // 避免重叠的最小距离
    public float minDistanceBetweenTreasures = 2f;
    // 每个宝藏最大尝试次数
    public int maxAttemptsPerTreasure = 50;

    private List<GameObject> spawnedTreasures = new List<GameObject>();

    private void Start()
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
                continue; // 这里会导致少生成宝藏
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
                // 如果没找到合法位置，强行放置（可能重叠，但保证数量）
                Debug.LogWarning($"[{data.type}] Could not find valid position, forcing placement!");
                finalPos = GetRandomPositionInFloor();
            }

            // 实例化
            GameObject treasureObj = Instantiate(data.prefab, finalPos, Quaternion.identity);
            spawnedTreasures.Add(treasureObj);

            Debug.Log($"Spawned treasure #{i + 1}: {data.type} at {finalPos}");
        }
    }

    private Vector2 GetRandomPositionInFloor()
    {
        Bounds bounds = floorCollider2D.bounds;

        // 只在上半部分
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float midY = (bounds.min.y + bounds.max.y) * 0.5f;  // 计算地板垂直方向的一半
        float y = Random.Range(midY, bounds.max.y);

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
