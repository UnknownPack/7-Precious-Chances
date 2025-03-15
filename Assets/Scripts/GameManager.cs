using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private const int maxNumOfLives = 7;
    [SerializeField] private int lives = 7;
    private int score;
    private int currentLevel = 1;
    private static GameManager instance;

    private UIDocument uiDocument;
    private Label _score, _lives;
    private Vector2 playerStartPosition;
    private GameObject player;
    private AudioSource audioSource;
    [SerializeField] private AudioClip bridgeMusic;
    [SerializeField] private AudioClip treasureRoomMusic;

    private int[] baseTreasureValues = { 10, 60, 200, 400 };

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
            DontDestroyOnLoad(player);
            playerStartPosition = player.transform.position;

            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "TreasureRoom") {
                player.transform.position = new Vector2(13.5f, -7.5f);
            }
        }
        else {
            Debug.LogError("Player not found in the scene");
        }

        UpdateUI();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    void UpdateUI() {
        if (_score != null) _score.text = "Score: " + score;
        if (_lives != null) _lives.text = "Lives: " + lives;
    }

    public void AddScore(int value) {
        score += GetTreasureValue(value);
        UpdateUI();
    }

    public void IncreaseLevel() {
        audioSource.pitch = 1 + 0.1f * (currentLevel);
        currentLevel++;
    }

    public int GetCurrentLevel() {
        return currentLevel;
    }

    public int GetTreasureValue(int baseValue) {
        float multiplier = Mathf.Pow(1.2f, currentLevel - 1);
        return Mathf.RoundToInt(baseValue * multiplier);
    }

    public void LoseLife() {
        lives--;
        UpdateUI(); 
        if (lives > 0) 
            SceneManager.LoadScene("Bridge");
        else
        {
            currentLevel = 1;
            lives = maxNumOfLives;
            score = 0;
            SceneManager.LoadScene("Bridge");
            UpdateUI(); 
        }
    } 
}
