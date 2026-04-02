using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterListWindow : MonoBehaviour, ICharacterView
{
    [Header("UI Elements")]
    [SerializeField] private Transform charactersContainer;
    [SerializeField] private GameObject characterItemPrefab;
    [SerializeField] private Button createButton;
    [SerializeField] private Button backButton;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private GameObject emptyStatePanel;
    [SerializeField] private TextMeshProUGUI emptyStateText;

    [Header("Loading")]
    [SerializeField] private GameObject loadingSpinner;

    private CharacterPresenter _presenter;
    private Dictionary<string, CharacterListItemView> _characterItems = new Dictionary<string, CharacterListItemView>();

    public event Action<string> OnCharacterSelected;
    public event Action OnCreateClicked;
    public event Action OnBackClicked;

    public void Initialize()
    {
        if (createButton != null)
            createButton.onClick.AddListener(() => OnCreateClicked?.Invoke());

        if (backButton != null)
            backButton.onClick.AddListener(() => OnBackClicked?.Invoke());

        if (titleText != null)
            titleText.text = "Мои персонажи";
    }

    public void SetPresenter(CharacterPresenter presenter)
    {
        _presenter = presenter;
        _presenter.SetView(this);
    }

    private void Awake()
    {
        if (createButton != null)
            createButton.onClick.AddListener(() => OnCreateClicked?.Invoke());

        if (backButton != null)
            backButton.onClick.AddListener(() => OnBackClicked?.Invoke());
    }

    public void Show()
    {
        gameObject.SetActive(true);
        _presenter?.RefreshCharacters();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void DisplayCharacters(List<CharacterUIData> characters)
    {
        ClearCharacters();

        if (characters == null || characters.Count == 0)
        {
            ShowEmptyState(true);
            return;
        }

        ShowEmptyState(false);

        foreach (var character in characters)
        {
            AddCharacterItem(character);
        }
    }

    public void DisplayCharacterDetails(CharacterUIData character)
    {
        OnCharacterSelected?.Invoke(character.Id);
    }

    public void ShowError(string message)
    {
        Debug.LogError($"Error: {message}");
    }

    public void ShowSuccess(string message)
    {
        Debug.Log($"Success: {message}");
    }

    public void ShowLoading(bool show)
    {
        if (loadingSpinner != null)
            loadingSpinner.SetActive(show);
    }

    public void ClearSelection()
    {

    }

    private void AddCharacterItem(CharacterUIData character)
    {
        if (characterItemPrefab == null || charactersContainer == null) return;

        var itemObject = Instantiate(characterItemPrefab, charactersContainer);
        var itemView = itemObject.GetComponent<CharacterListItemView>();

        if (itemView != null)
        {
            itemView.Setup(character);
            itemView.OnClicked += () => OnCharacterSelected?.Invoke(character.Id);
            _characterItems[character.Id] = itemView;
        }
    }

    private void ClearCharacters()
    {
        foreach (var item in _characterItems.Values)
        {
            if (item != null)
                Destroy(item.gameObject);
        }
        _characterItems.Clear();
    }

    private void ShowEmptyState(bool show)
    {
        if (emptyStatePanel != null)
            emptyStatePanel.SetActive(show);

        if (emptyStateText != null && show)
            emptyStateText.text = "У вас пока нет персонажей.\nНажмите кнопку 'Создать' чтобы начать";
    }
}