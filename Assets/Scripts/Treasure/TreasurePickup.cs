using UnityEngine;

public class TreasurePickup : MonoBehaviour
{
    public int score;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.AddScore(score);
            TreasureManager2D.treasuresRemaining--;
            if (TreasureManager2D.treasuresRemaining <= 0)
            {
                TreasureRoomDoorSpawner spawner = FindFirstObjectByType<TreasureRoomDoorSpawner>();
                if (spawner != null)
                    spawner.SpawnDoor();
            }
            Destroy(gameObject);
        }
    }
}
