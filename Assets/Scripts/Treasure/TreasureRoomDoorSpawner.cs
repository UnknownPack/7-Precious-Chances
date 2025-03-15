using UnityEngine;

public class TreasureRoomDoorSpawner : MonoBehaviour
{
    [SerializeField] private GameObject doorPrefab;

    [SerializeField] private Transform doorSpawnPoint;

    private bool doorSpawned = false;

    private void Start()
    {
        SpawnDoor();
    }

    public void SpawnDoor()
    {
        if (!doorSpawned)
        {
            Instantiate(doorPrefab, doorSpawnPoint.position, doorSpawnPoint.rotation);
            doorSpawned = true;
        }
    }
}
