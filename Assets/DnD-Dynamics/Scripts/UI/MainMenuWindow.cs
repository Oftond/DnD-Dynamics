using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuWindow : MonoBehaviour
{
    [Header("Buttons")]
    private Button _charactersButton;
    private Button _createButton;
    private Button _exitButton;

    [Header("Title")]
    private Label _titleText;
    private Label _versionText;

    public event System.Action OnCharactersClicked;
    public event System.Action OnCreateClicked;
    public event System.Action OnExitClicked;

    public void Initialize()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        // Load UXML and USS
        if (root != null)
        {
            // Register buttons
            _charactersButton = root.Q<Button>("CharactersButton");
            _createButton = root.Q<Button>("CreateButton");
            _exitButton = root.Q<Button>("ExitButton");

            // Register labels
            _titleText = root.Q<Label>("TitleText");
            _versionText = root.Q<Label>("VersionText");

            // Add event listeners
            if (_charactersButton != null)
                _charactersButton.clicked += () => OnCharactersClicked?.Invoke();

            if (_createButton != null)
                _createButton.clicked += () => OnCreateClicked?.Invoke();

            if (_exitButton != null)
                _exitButton.clicked += () => OnExitClicked?.Invoke();

            // Set version text
            if (_versionText != null)
                _versionText.text = $"v{Constants.APP_VERSION}";
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}