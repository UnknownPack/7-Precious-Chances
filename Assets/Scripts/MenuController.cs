using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuController : MonoBehaviour
{
    private UIDocument _uiDocument;
    private Button startButton, exitButton;
    void Start()
    {
        _uiDocument = GetComponent<UIDocument>();
        startButton = _uiDocument.rootVisualElement.Q<Button>("start");
        exitButton = _uiDocument.rootVisualElement.Q<Button>("exit");
        startButton.clicked += () => SceneManager.LoadScene(1);
        exitButton.clicked += () => Application.Quit();
    } 
}
