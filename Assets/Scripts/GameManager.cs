using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{

    [SerializeField] private int lives = 7;
    private int score;
    private static GameManager instance;

    private UIDocument uiDocument;
    private Label _score, _lives;
    private Vector2 playerStartPosition;
    private GameObject player;

    public static GameManager Instance {
        get {
            if (instance == null) {
                GameObject gameManagerobject = new GameObject("GameManager");
                instance = gameManagerobject.AddComponent<GameManager>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        score = 0;

        _score = uiDocument.rootVisualElement.Q<Label>("_score");
        _lives = uiDocument.rootVisualElement.Q<Label>("_lives");

        player = GameObject.FindWithTag("Player");
        if (player != null) {
            playerStartPosition = player.transform.position;
        }
        else {
            Debug.LogError("Player not found in the scene");
        }

        UpdateUI();
    }

    void UpdateUI() {
        if (_score != null) _score.text = "Score: " + score;
        if (_lives != null) _lives.text = "Lives: " + lives;
    }

    public void AddScore(int value) {
        score += value;
        UpdateUI();
    }

    public void LoseLife() {
        lives--;
        UpdateUI();

        if (lives > 0) {
            RespawnPlayer();
        }
        else {
            GameOver();
        }
    }

    private void RespawnPlayer() {
        if (player != null) {
            player.transform.position = playerStartPosition;
            ClearFireballs();
        }
    }

    private void ClearFireballs() {
        GameObject[] fireballs = GameObject.FindGameObjectsWithTag("Fireball");
        foreach (GameObject fireball in fireballs) {
            Destroy(fireball);
        }
    }

    private void GameOver() {
        Debug.Log("Game Over!");

        //Need to implement game over logic
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
