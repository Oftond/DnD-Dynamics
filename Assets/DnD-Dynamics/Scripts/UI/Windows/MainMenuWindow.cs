using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuWindow : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button _charactersButton;
    [SerializeField] private Button _createButton;
    [SerializeField] private Button _handbookButton;
    [SerializeField] private Button _exitButton;

    [Header("Title")]
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _versionText;

    public event Action OnCharactersClicked;
    public event Action OnCreateClicked;
    public event Action OnHandbookClicked;
    public event Action OnExitClicked;

    public void Initialize()
    {
        if (_charactersButton != null)
            _charactersButton.onClick.AddListener(() => OnCharactersClicked?.Invoke());

        if (_createButton != null)
            _createButton.onClick.AddListener(() => OnCreateClicked?.Invoke());

        if (_handbookButton != null)
            _handbookButton.onClick.AddListener(() => OnHandbookClicked?.Invoke());

        if (_exitButton != null)
            _exitButton.onClick.AddListener(() => OnExitClicked?.Invoke());

        if (_versionText != null)
            _versionText.text = $"v{Constants.APP_VERSION}";
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