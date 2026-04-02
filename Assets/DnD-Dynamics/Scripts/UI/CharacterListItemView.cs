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
    [SerializeField] private Image hpFillImage;
    [SerializeField] private Button selectButton;
    [SerializeField] private GameObject selectedIndicator;

    private CharacterUIData _character;

    public event System.Action OnClicked;

    public void Setup(CharacterUIData character)
    {
        _character = character;

        if (nameText != null)
            nameText.text = character.Name;

        if (classRaceText != null)
            classRaceText.text = character.ClassRaceText;

        if (levelText != null)
            levelText.text = $"Óð. {character.Level}";

        if (hpText != null)
            hpText.text = character.HpText;

        if (hpFillImage != null)
        {
            var fillAmount = (float)character.CurrentHp / character.MaxHp;
            hpFillImage.fillAmount = fillAmount;
            hpFillImage.color = Extensions.GetHpColor(character.CurrentHp, character.MaxHp);
        }

        if (selectButton != null)
            selectButton.onClick.AddListener(() => OnClicked?.Invoke());
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