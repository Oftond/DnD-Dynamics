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
    private CharacterPresenter _presenter;
    private CharacterModel _model;

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

        _model = new CharacterModel();
        _presenter = new CharacterPresenter(_model);

        InitializeWindows();
    }

    private void InitializeWindows()
    {
        if (mainMenuWindow != null)
        {
            mainMenuWindow.Initialize();

            mainMenuWindow.OnCharactersClicked += ShowCharacterList;
            mainMenuWindow.OnCreateClicked += ShowCreateCharacter;
            mainMenuWindow.OnExitClicked += ExitApplication;
        }

        if (characterListWindow != null)
        {
            characterListWindow.Initialize();

            characterListWindow.SetPresenter(_presenter);
            characterListWindow.OnBackClicked += ShowMainMenu;
            characterListWindow.OnCreateClicked += ShowCreateCharacter;

            characterListWindow.OnCharacterSelected += OnCharacterSelected;
        }

        if (characterDetailWindow != null)
        {
            characterDetailWindow.Initialize();
            characterDetailWindow.SetPresenter(_presenter);
            characterDetailWindow.OnBackClicked += ShowCharacterList;
        }

        if (createCharacterWindow != null)
        {
            createCharacterWindow.Initialize();
            createCharacterWindow.SetPresenter(_presenter);

            createCharacterWindow.OnCancelClicked += ShowCharacterList;
            createCharacterWindow.OnCreateClicked += OnCreateCharacter;
        }
    }

    private void OnCharacterSelected(string characterId)
    {
        Debug.Log($"UIManager: Character selected with ID: {characterId}");

        _presenter.SelectCharacter(characterId);

        ShowCharacterDetail();
    }

    private void OnCreateCharacter(string name, int race, int characterClass,
    int strength, int dexterity, int constitution,
    int intelligence, int wisdom, int charisma)
    {
        Debug.Log($"Creating character: {name}, Race: {race}, Class: {characterClass}");

        _presenter.CreateCharacter(name, (CharacterRace)race, (CharacterClass)characterClass,
            strength, dexterity, constitution, intelligence, wisdom, charisma);

        ShowCharacterList();

        characterListWindow?.RefreshCharacters();
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

    private void ExitApplication()
    {
        Debug.Log("Exiting application...");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
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