using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Windows")]
    [SerializeField] private MainMenuWindow mainMenuWindow;
    [SerializeField] private CharacterListWindow characterListWindow;
    [SerializeField] private CharacterDetailWindow characterDetailWindow;
    [SerializeField] private CreateCharacterWindow createCharacterWindow;

    [Header("Loading")]
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private TextMeshProUGUI loadingText;

    [Header("Notifications")]
    [SerializeField] private GameObject notificationPanel;
    [SerializeField] private TextMeshProUGUI notificationText;

    private static UIManager _instance;
    public static UIManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeWindows();
    }

    private void InitializeWindows()
    {
        if (mainMenuWindow != null)
            mainMenuWindow.Initialize();

        if (characterListWindow != null)
            characterListWindow.Initialize();

        if (characterDetailWindow != null)
            characterDetailWindow.Initialize();

        if (createCharacterWindow != null)
            createCharacterWindow.Initialize();
    }

    public void ShowMainMenu()
    {
        HideAllWindows();
        if (mainMenuWindow != null)
            mainMenuWindow.Show();
    }

    public void ShowCharacterList()
    {
        HideAllWindows();
        if (characterListWindow != null)
            characterListWindow.Show();
    }

    public void ShowCharacterDetail()
    {
        HideAllWindows();
        if (characterDetailWindow != null)
            characterDetailWindow.Show();
    }

    public void ShowCreateCharacter()
    {
        HideAllWindows();
        if (createCharacterWindow != null)
            createCharacterWindow.Show();
    }

    public void HideAllWindows()
    {
        if (mainMenuWindow != null) mainMenuWindow.Hide();
        if (characterListWindow != null) characterListWindow.Hide();
        if (characterDetailWindow != null) characterDetailWindow.Hide();
        if (createCharacterWindow != null) createCharacterWindow.Hide();
    }

    public void ShowLoading(string message = "Загрузка...")
    {
        if (loadingPanel != null)
        {
            if (loadingText != null)
                loadingText.text = message;
            loadingPanel.SetActive(true);
        }
    }

    public void HideLoading()
    {
        if (loadingPanel != null)
            loadingPanel.SetActive(false);
    }

    public void ShowNotification(string message, float duration = 2f)
    {
        if (notificationPanel != null)
        {
            if (notificationText != null)
                notificationText.text = message;
            notificationPanel.SetActive(true);
            Invoke(nameof(HideNotification), duration);
        }
    }

    private void HideNotification()
    {
        if (notificationPanel != null)
            notificationPanel.SetActive(false);
    }

    public void ShowError(string message)
    {
        ShowNotification($"{message}", 3f);
        Debug.LogError(message);
    }

    public void ShowSuccess(string message)
    {
        ShowNotification($"{message}", 2f);
        Debug.Log(message);
    }
}