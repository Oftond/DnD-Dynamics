using System;
using TMPro;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UI;

public class CreateCharacterWindow : MonoBehaviour
{
    [Header("Character Info")]
    [SerializeField] private InputField nameInput;
    [SerializeField] private Dropdown raceDropdown;
    [SerializeField] private Dropdown classDropdown;

    [Header("Stats")]
    [SerializeField] private InputField strengthInput;
    [SerializeField] private InputField dexterityInput;
    [SerializeField] private InputField constitutionInput;
    [SerializeField] private InputField intelligenceInput;
    [SerializeField] private InputField wisdomInput;
    [SerializeField] private InputField charismaInput;

    [Header("Buttons")]
    [SerializeField] private Button createButton;
    [SerializeField] private Button cancelButton;

    [Header("Validation")]
    [SerializeField] private TextMeshProUGUI errorText;

    private CharacterPresenter _presenter;

    public event Action<string, int, int, int, int, int, int, int, int> OnCreateClicked;
    public event Action OnCancelClicked;

    public void Initialize()
    {
        if (raceDropdown != null)
        {
            raceDropdown.ClearOptions();
            var races = Enum.GetNames(typeof(CharacterRace));
            raceDropdown.AddOptions(new System.Collections.Generic.List<string>(races));
        }

        if (classDropdown != null)
        {
            classDropdown.ClearOptions();
            var classes = Enum.GetNames(typeof(CharacterClass));
            classDropdown.AddOptions(new System.Collections.Generic.List<string>(classes));
        }

        if (createButton != null)
            createButton.onClick.AddListener(OnCreate);

        if (cancelButton != null)
            cancelButton.onClick.AddListener(() => OnCancelClicked?.Invoke());

        SetDefaultStats();
    }

    public void SetPresenter(CharacterPresenter presenter)
    {
        _presenter = presenter;
    }

    public void Show()
    {
        gameObject.SetActive(true);
        SetDefaultStats();
        ClearError();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnCreate()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(nameInput.text) || nameInput.text.Length < 2)
            {
                ShowError("Имя должно содержать минимум 2 символа");
                return;
            }

            var strength = GetStatValue(strengthInput, Constants.DEFAULT_STRENGTH);
            var dexterity = GetStatValue(dexterityInput, Constants.DEFAULT_DEXTERITY);
            var constitution = GetStatValue(constitutionInput, Constants.DEFAULT_CONSTITUTION);
            var intelligence = GetStatValue(intelligenceInput, Constants.DEFAULT_INTELLIGENCE);
            var wisdom = GetStatValue(wisdomInput, Constants.DEFAULT_WISDOM);
            var charisma = GetStatValue(charismaInput, Constants.DEFAULT_CHARISMA);

            if (!ValidateStats(strength, dexterity, constitution, intelligence, wisdom, charisma))
            {
                ShowError("Характеристики должны быть от 3 до 20");
                return;
            }

            OnCreateClicked?.Invoke(
                nameInput.text,
                raceDropdown.value + 1,
                classDropdown.value + 1,
                strength, dexterity, constitution,
                intelligence, wisdom, charisma
            );
        }
        catch (Exception ex)
        {
            ShowError($"Ошибка: {ex.Message}");
            Debug.LogError(ex);
        }
    }

    private int GetStatValue(InputField input, int defaultValue)
    {
        if (input != null && int.TryParse(input.text, out int value))
        {
            return Math.Clamp(value, Constants.MIN_ABILITY_SCORE, Constants.MAX_ABILITY_SCORE);
        }
        return defaultValue;
    }

    private bool ValidateStats(params int[] stats)
    {
        foreach (var stat in stats)
        {
            if (stat < Constants.MIN_ABILITY_SCORE || stat > Constants.MAX_ABILITY_SCORE)
                return false;
        }
        return true;
    }

    private void SetDefaultStats()
    {
        if (strengthInput != null) strengthInput.text = Constants.DEFAULT_STRENGTH.ToString();
        if (dexterityInput != null) dexterityInput.text = Constants.DEFAULT_DEXTERITY.ToString();
        if (constitutionInput != null) constitutionInput.text = Constants.DEFAULT_CONSTITUTION.ToString();
        if (intelligenceInput != null) intelligenceInput.text = Constants.DEFAULT_INTELLIGENCE.ToString();
        if (wisdomInput != null) wisdomInput.text = Constants.DEFAULT_WISDOM.ToString();
        if (charismaInput != null) charismaInput.text = Constants.DEFAULT_CHARISMA.ToString();
    }

    private void ShowError(string message)
    {
        if (errorText != null)
        {
            errorText.text = message;
            errorText.gameObject.SetActive(true);
            Invoke(nameof(ClearError), 3f);
        }
    }

    private void ClearError()
    {
        if (errorText != null)
            errorText.gameObject.SetActive(false);
    }
}