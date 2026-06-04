using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterListItemView : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _classRaceText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _hpText;
    [SerializeField] private Button selectButton;

    private CharacterUIData _character;

    public event Action OnClicked;

    public void Setup(CharacterUIData character)
    {
        _character = character;

        Debug.Log($"Setting up character item: Name={character.Name}, Class={character.ClassName}, Race={character.RaceName}");

        if (_nameText != null)
            _nameText.text = character.Name;
        else
            Debug.LogError("NameText is not assigned in CharacterListItemView!");

        if (_classRaceText != null)
            _classRaceText.text = $"{character.ClassName} - {character.RaceName}";

        if (_levelText != null)
            _levelText.text = $"Óð. {character.Level}";

        if (_hpText != null)
            _hpText.text = $"HP: {character.CurrentHp} / {character.MaxHp}";

        if (selectButton != null)
        {
            selectButton.onClick.RemoveAllListeners();
            selectButton.onClick.AddListener(() => {
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

    public CharacterUIData GetCharacter()
    {
        return _character;
    }
}