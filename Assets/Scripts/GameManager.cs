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

    void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        score = 0;  
        lives = maxNumOfLives; 
        currentLevel = 1;
        _score = uiDocument.rootVisualElement.Q<Label>("_score");
        _lives = uiDocument.rootVisualElement.Q<Label>("_lives");
        UpdateUI();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    void UpdateUI() {
        if (_score != null) _score.text = "Score: " + score;
        string livesDisplay = "Lives: ";
        if (_lives != null)
        {
             
            for (int i = 0; i < lives; i++)
            {
                livesDisplay += '*';
            }
            _lives.text = livesDisplay;
            Debug.Log("Lives: " + _lives.text);
        }
    }

    public void AddScore(int value) {
        score += GetTreasureValue(value);
        UpdateUI();
    }

    public void IncreaseLevel() {
        audioSource.pitch = 1 + (0.1f * currentLevel);
        Time.timeScale = 1 + (0.1f * currentLevel);
        //TODO: Add logic to change colour of the level(s)
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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "TreasureRoom")
        {
            GameObject door = GameObject.FindWithTag("DoorSpawnPoint");
            if (door != null && player != null)
            {
                player.transform.position = door.transform.position;
            }
        }
    }
}
