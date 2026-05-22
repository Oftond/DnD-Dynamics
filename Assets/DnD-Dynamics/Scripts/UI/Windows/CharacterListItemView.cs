using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterListItemView : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI classRaceText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI hpMaxText;
    [SerializeField] private Image hpFillImage;
    [SerializeField] private Button selectButton;
    [SerializeField] private GameObject selectedIndicator;

    private CharacterUIData _character;

    public event Action OnClicked;

    public void Setup(CharacterUIData character)
    {
        _character = character;

        Debug.Log($"Setting up character item: Name={character.Name}, Class={character.ClassName}, Race={character.RaceName}");

        if (nameText != null)
            nameText.text = character.Name;
        else
            Debug.LogError("NameText is not assigned in CharacterListItemView!");

        if (classRaceText != null)
            classRaceText.text = $"{character.ClassName} - {character.RaceName}";

        if (levelText != null)
            levelText.text = $"Óð. {character.Level}";

        if (hpText != null)
            hpText.text = character.CurrentHp.ToString();

        if (hpMaxText != null)
            hpMaxText.text = character.MaxHp.ToString();

        if (hpFillImage != null)
        {
            float fillAmount = (float)character.CurrentHp / character.MaxHp;
            hpFillImage.fillAmount = fillAmount;

            if (fillAmount <= 0.25f)
                hpFillImage.color = Color.red;
            else if (fillAmount <= 0.5f)
                hpFillImage.color = Color.yellow;
            else
                hpFillImage.color = Color.green;
        }

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
        if (selectedIndicator != null)
            selectedIndicator.SetActive(selected);
    }

    public CharacterUIData GetCharacter()
    {
        return _character;
    }
}