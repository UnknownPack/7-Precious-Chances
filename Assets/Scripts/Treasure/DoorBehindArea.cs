using UnityEngine;

public class DoorBehindArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.SetPlayerBehindDoor(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.SetPlayerBehindDoor(false);
        }
    }
}
