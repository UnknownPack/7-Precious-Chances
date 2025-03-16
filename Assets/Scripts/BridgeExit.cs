using UnityEngine;
using UnityEngine.SceneManagement;

public class BridgeExit : MonoBehaviour
{

    [SerializeField] private string nextSceneName = "TreasureRoom";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            if (nextSceneName == "TreasureRoom") {
                GameManager.Instance.EnterTreasureRoom();
            }
            else {
                GameManager.Instance.EnterBridgeLevel();
            }
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
