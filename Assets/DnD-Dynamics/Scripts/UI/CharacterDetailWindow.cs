using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDetailWindow : MonoBehaviour, ICharacterView
{
    [Header("Basic Info")]
    [SerializeField] private TextMeshProUGUI characterNameText;
    [SerializeField] private TextMeshProUGUI classRaceText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Image portraitImage;

    [Header("Health")]
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI hpFractionText;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private TMP_InputField damageHealInput;
    [SerializeField] private Button damageButton;
    [SerializeField] private Button healButton;

    [Header("Stats")]
    [SerializeField] private TextMeshProUGUI strengthText;
    [SerializeField] private TextMeshProUGUI dexterityText;
    [SerializeField] private TextMeshProUGUI constitutionText;
    [SerializeField] private TextMeshProUGUI intelligenceText;
    [SerializeField] private TextMeshProUGUI wisdomText;
    [SerializeField] private TextMeshProUGUI charismaText;

    [Header("Combat")]
    [SerializeField] private TextMeshProUGUI armorClassText;
    [SerializeField] private TextMeshProUGUI initiativeText;
    [SerializeField] private TextMeshProUGUI proficiencyText;

    [Header("Wealth")]
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI silverText;
    [SerializeField] private TextMeshProUGUI copperText;

    [Header("Info")]
    [SerializeField] private TextMeshProUGUI backstoryText;
    [SerializeField] private TextMeshProUGUI notesText;

    [Header("Buttons")]
    [SerializeField] private Button levelUpButton;
    [SerializeField] private Button deleteButton;
    [SerializeField] private Button editButton;
    [SerializeField] private Button backButton;

    [Header("Loading")]
    [SerializeField] private GameObject loadingSpinner;

    private CharacterPresenter _presenter;

    public event Action OnBackClicked;
    public event Action<int> OnDamageClicked;
    public event Action<int> OnHealClicked;
    public event Action OnLevelUpClicked;
    public event Action OnDeleteClicked;
    public event Action OnEditClicked;

    public void Initialize()
    {
        if (damageButton != null)
            damageButton.onClick.AddListener(() =>
            {
                var test = GetDamageHealAmount();
                OnDamageClicked?.Invoke(test);
                print($"УРОН: {test}");
                UpdateUI();
            });

        if (healButton != null)
            healButton.onClick.AddListener(() =>
            {
                OnHealClicked?.Invoke(GetDamageHealAmount());
                UpdateUI();
            });

        if (levelUpButton != null)
            levelUpButton.onClick.AddListener(() =>
            {
                OnLevelUpClicked?.Invoke();
                UpdateUI();
            });

        if (deleteButton != null)
            deleteButton.onClick.AddListener(() => OnDeleteClicked?.Invoke());

        if (editButton != null)
            editButton.onClick.AddListener(() => OnEditClicked?.Invoke());

        if (backButton != null)
            backButton.onClick.AddListener(() => OnBackClicked?.Invoke());

        if (damageHealInput != null)
            damageHealInput.text = "5";
    }

    public void SetPresenter(CharacterPresenter presenter)
    {
        _presenter = presenter;
        _presenter.SetView(this);
    }

    private void Awake()
    {
        if (damageButton != null)
            damageButton.onClick.AddListener(() => OnDamageClicked?.Invoke(GetDamageHealAmount()));

        if (healButton != null)
            healButton.onClick.AddListener(() => OnHealClicked?.Invoke(GetDamageHealAmount()));

        if (levelUpButton != null)
            levelUpButton.onClick.AddListener(() => OnLevelUpClicked?.Invoke());

        if (deleteButton != null)
            deleteButton.onClick.AddListener(() => OnDeleteClicked?.Invoke());

        if (editButton != null)
            editButton.onClick.AddListener(() => OnEditClicked?.Invoke());

        if (backButton != null)
            backButton.onClick.AddListener(() => OnBackClicked?.Invoke());
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void DisplayCharacters(System.Collections.Generic.List<CharacterUIData> characters)
    {

    }

    public void DisplayCharacterDetails(CharacterUIData character)
    {
        UpdateUI();
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
        ClearUI();
    }

    private void UpdateUI()
    {
        if (_presenter == null) return;

        var character = _presenter.GetSelectedCharacter();

        if (characterNameText != null)
            characterNameText.text = character.Name;

        if (classRaceText != null)
            classRaceText.text = character.ClassRaceText;

        if (levelText != null)
            levelText.text = character.LevelText;

        if (hpText != null)
            hpText.text = $"{character.CurrentHp} / {character.MaxHp}";

        if (hpFractionText != null)
            hpFractionText.text = $"{character.CurrentHp}/{character.MaxHp}";

        if (hpSlider != null)
            hpSlider.value = (float)character.CurrentHp / character.MaxHp;

        if (strengthText != null)
            strengthText.text = $"Сила: {character.StrengthText}";

        if (dexterityText != null)
            dexterityText.text = $"Ловкость: {character.DexterityText}";

        if (constitutionText != null)
            constitutionText.text = $"Телосложение: {character.ConstitutionText}";

        if (intelligenceText != null)
            intelligenceText.text = $"Интеллект: {character.IntelligenceText}";

        if (wisdomText != null)
            wisdomText.text = $"Мудрость: {character.WisdomText}";

        if (charismaText != null)
            charismaText.text = $"Харизма: {character.CharismaText}";

        if (armorClassText != null)
            armorClassText.text = character.ArmorClassText;

        if (initiativeText != null)
            initiativeText.text = character.InitiativeText;

        if (proficiencyText != null)
            proficiencyText.text = character.ProficiencyText;

        if (goldText != null)
            goldText.text = $"Золото: {character.Gold}";

        if (silverText != null)
            silverText.text = $"Серебро: {character.Silver}";

        if (copperText != null)
            copperText.text = $"Медь: {character.Copper}";

        if (backstoryText != null)
            backstoryText.text = string.IsNullOrEmpty(character.Backstory)
                ? "История не указана" : character.Backstory;

        if (notesText != null)
            notesText.text = string.IsNullOrEmpty(character.Notes)
                ? "Нет заметок" : character.Notes;
    }

    private void ClearUI()
    {
        if (characterNameText != null) characterNameText.text = "";
        if (classRaceText != null) classRaceText.text = "";
        if (hpText != null) hpText.text = "0/0";
        if (hpSlider != null) hpSlider.value = 0;
    }

    private int GetDamageHealAmount()
    {
        if (damageHealInput != null && int.TryParse(damageHealInput.text, out int amount))
        {
            return Mathf.Max(1, amount);
        }
        return 5;
    }
}