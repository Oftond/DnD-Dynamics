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
    private CharacterUIData _currentCharacter;

    public event Action OnBackClicked;
    public event Action<int> OnDamageClicked;
    public event Action<int> OnHealClicked;
    public event Action OnLevelUpClicked;
    public event Action OnDeleteClicked;
    public event Action OnEditClicked;

    public void Initialize()
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
        _currentCharacter = character;
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
        _currentCharacter = null;
        ClearUI();
    }

    private void UpdateUI()
    {
        if (_currentCharacter == null) return;

        if (characterNameText != null)
            characterNameText.text = _currentCharacter.Name;

        if (classRaceText != null)
            classRaceText.text = _currentCharacter.ClassRaceText;

        if (levelText != null)
            levelText.text = _currentCharacter.LevelText;

        if (hpText != null)
            hpText.text = $"{_currentCharacter.CurrentHp} / {_currentCharacter.MaxHp}";

        if (hpFractionText != null)
            hpFractionText.text = $"{_currentCharacter.CurrentHp}/{_currentCharacter.MaxHp}";

        if (hpSlider != null)
            hpSlider.value = (float)_currentCharacter.CurrentHp / _currentCharacter.MaxHp;

        if (strengthText != null)
            strengthText.text = _currentCharacter.StrengthText;

        if (dexterityText != null)
            dexterityText.text = _currentCharacter.DexterityText;

        if (constitutionText != null)
            constitutionText.text = _currentCharacter.ConstitutionText;

        if (intelligenceText != null)
            intelligenceText.text = _currentCharacter.IntelligenceText;

        if (wisdomText != null)
            wisdomText.text = _currentCharacter.WisdomText;

        if (charismaText != null)
            charismaText.text = _currentCharacter.CharismaText;

        if (armorClassText != null)
            armorClassText.text = _currentCharacter.ArmorClassText;

        if (initiativeText != null)
            initiativeText.text = _currentCharacter.InitiativeText;

        if (proficiencyText != null)
            proficiencyText.text = _currentCharacter.ProficiencyText;

        if (goldText != null)
            goldText.text = _currentCharacter.Gold.ToString();

        if (silverText != null)
            silverText.text = _currentCharacter.Silver.ToString();

        if (copperText != null)
            copperText.text = _currentCharacter.Copper.ToString();

        if (backstoryText != null)
            backstoryText.text = string.IsNullOrEmpty(_currentCharacter.Backstory)
                ? "История не указана" : _currentCharacter.Backstory;

        if (notesText != null)
            notesText.text = string.IsNullOrEmpty(_currentCharacter.Notes)
                ? "Нет заметок" : _currentCharacter.Notes;
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