using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    private const int maxNumOfLives = 7;
    [SerializeField] private int lives = 7;
    private int score;
    private int currentLevel = 0;
    private static GameManager instance;

    private UIDocument uiDocument;
    private Label _score, _lives;
    private Vector2 playerStartPosition;
    private PlayerController player;
    private AudioSource audioSource;
    [SerializeField] private AudioClip bridgeMusic;
    [SerializeField] private AudioClip treasureRoomMusic;
    [SerializeField] private VolumeProfile volumeProfile;
    private ChromaticAberration chromaticAberration;
    private ColorAdjustments colorAdjustments;

    private int[] baseTreasureValues = { 10, 60, 200, 400 };

    public bool IsPlayerBehindDoor { get; private set; }
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

    private void Update()
    {
        if (player == null)
        {
            player = FindObjectOfType<PlayerController>();
        }
    }

    void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        score = 0;  
        lives = maxNumOfLives; 
        currentLevel = 0;
        _score = uiDocument.rootVisualElement.Q<Label>("_score");
        _lives = uiDocument.rootVisualElement.Q<Label>("_lives");
        UpdateUI();
        audioSource = gameObject.GetComponent<AudioSource>();
        if (volumeProfile.TryGet(out chromaticAberration)) {
            chromaticAberration.intensity.overrideState = true;
            chromaticAberration.intensity.value = 0;
            
        }
        if (volumeProfile.TryGet(out colorAdjustments)) {
            colorAdjustments.hueShift.overrideState = true;
            colorAdjustments.hueShift.value = 0;
            
        }
        EnterBridgeLevel();
    }

    void UpdateUI() {
        if (_score != null) _score.text = "Score: " + score; 
        if (_lives != null) { _lives.text = "Lives: " + lives; }
    }

    public void AddScore(int value) {
        score += GetTreasureValue(value);
        UpdateUI();
    }

    public void IncreaseLevel() {
        audioSource.pitch = 1 + (0.1f * currentLevel);
        Time.timeScale = 1 + (0.1f * currentLevel);
        //TODO: Add logic to change colour of the level(s)
        chromaticAberration.intensity.value = 0.1f * currentLevel;
        colorAdjustments.hueShift.value = 10 * currentLevel;
        currentLevel++;
    }

    public void EnterTreasureRoom() {
        audioSource.clip = treasureRoomMusic;
        audioSource.Play();
    }

    public void EnterBridgeLevel() {
        audioSource.clip = bridgeMusic;
        audioSource.Play();
        IncreaseLevel();
    }

    public int GetCurrentLevel() {
        return currentLevel;
    }

    public int GetTreasureValue(int baseValue) {
        float multiplier = Mathf.Pow(1.2f, currentLevel - 1);
        return Mathf.RoundToInt(baseValue * multiplier);
    }

    public void LoseLife()
    {
        StartCoroutine(EndLife());
    }

    IEnumerator EndLife()
    {
        if (player != null)
            player.PlayerDeathAnimation();

        yield return new WaitForSeconds(0.6f);
        lives--;
        UpdateUI(); 
        if (lives > 0) 
            SceneManager.LoadScene("Bridge");
        else
        {
            currentLevel = 1;
            lives = maxNumOfLives;
            score = 0;
            SceneManager.LoadScene("MainMenu");
            UpdateUI();
            Destroy(gameObject);
        }
    }
    public void SetPlayerBehindDoor(bool value)
    {
        IsPlayerBehindDoor = value;
    }
}
        