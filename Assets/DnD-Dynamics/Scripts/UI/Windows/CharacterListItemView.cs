using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CharacterListItemView : MonoBehaviour
{
    [Inject] private IPortraitDataService _portraitDataService;

    [Header("UI Elements")]
    [SerializeField] private Image _portraitImage;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _classRaceText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _hpText;
    [SerializeField] private Button _selectButton;

    [Header("Default")]
    [SerializeField] private Sprite _defaultPortraitSprite;

    private CharacterUIData _character;

    public event Action OnClicked;

    private void Start()
    {
        if (_portraitDataService != null)
            _portraitDataService.OnPortraitLoaded += OnPortraitLoadedFromCache;
    }

    private void OnDestroy()
    {
        if (_portraitDataService != null)
            _portraitDataService.OnPortraitLoaded -= OnPortraitLoadedFromCache;
    }

    public void Setup(CharacterUIData character)
    {
        _character = character;

        Debug.Log($"Setting up character item: Name={character.Name}, Class={character.ClassName}, Race={character.RaceName}");

        LoadPortrait(character.PortraitPath);

        if (_nameText != null)
            _nameText.text = character.Name;
        else
            Debug.LogError("NameText is not assigned in CharacterListItemView!", this);

        if (_classRaceText != null)
            _classRaceText.text = $"{character.ClassName} - {character.RaceName}";

        if (_levelText != null)
            _levelText.text = $"Óð. {character.Level}";

        if (_hpText != null)
            _hpText.text = $"HP: {character.CurrentHp} / {character.MaxHp}";

        if (_selectButton != null)
        {
            _selectButton.onClick.RemoveAllListeners();
            _selectButton.onClick.AddListener(() => {
                Debug.Log($"Character item clicked: {character.Name}");
                OnClicked?.Invoke();
            });
        }
    }

    public void SetSelected(bool selected)
    {
        //if (selectedIndicator != null)
        //    selectedIndicator.SetActive(selected);
    }

    public CharacterUIData GetCharacter() => _character;

    private void LoadPortrait(string portraitPath)
    {
        if (_portraitImage == null)
            return;

        if (string.IsNullOrEmpty(portraitPath) || !File.Exists(portraitPath))
        {
            _portraitImage.sprite = _defaultPortraitSprite;

            return;
        }

        var texture = _portraitDataService.GetPortrait(portraitPath);

        if (texture != null)
        {
            _portraitImage.sprite = _portraitDataService.CreateSpriteFromTexture(texture);
        }
        else
        {
            _portraitImage.sprite = _defaultPortraitSprite;
        }
    }

    private void OnPortraitLoadedFromCache(string path, Texture2D texture)
    {
        if (_character != null && _character.PortraitPath == path)
        {
            if (_portraitImage != null)
                _portraitImage.sprite = _portraitDataService.CreateSpriteFromTexture(texture);
        }
    }
}