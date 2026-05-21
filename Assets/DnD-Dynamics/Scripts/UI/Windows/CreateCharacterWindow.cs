using DnD_Dynamics.Models;
using DnD_Dynamics.MVP.Presenter;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace DnD_Dynamics.UI.Windows
{
    public class CreateCharacterWindow : MonoBehaviour, ICreateCharacterView
    {
        [Header("Character Info")]
        [SerializeField] private TMP_InputField nameInput;
        [SerializeField] private TMP_Dropdown raceDropdown;
        [SerializeField] private TMP_Dropdown classDropdown;

        [Header("Stats")]
        [SerializeField] private TMP_InputField strengthInput;
        [SerializeField] private TMP_InputField dexterityInput;
        [SerializeField] private TMP_InputField constitutionInput;
        [SerializeField] private TMP_InputField intelligenceInput;
        [SerializeField] private TMP_InputField wisdomInput;
        [SerializeField] private TMP_InputField charismaInput;

        [Header("Buttons")]
        [SerializeField] private Button createButton;
        [SerializeField] private Button cancelButton;

        [Header("Validation")]
        [SerializeField] private TextMeshProUGUI errorText;
        [SerializeField] private GameObject _loadingSpinner;

        private CreateCharacterPresenter _presenter;
        private List<CharacterRace> _availableRaces;
        private List<CharacterClass> _availableClasses;
        private List<string> _raceIds;
        private List<string> _classIds;

        public event Action<string, string, string, int, int, int, int, int, int> OnCreateClicked;
        public event Action OnCancelClicked;

        private void Start()
        {
            if (createButton != null)
                createButton.onClick.AddListener(OnCreate);

            if (cancelButton != null)
                cancelButton.onClick.AddListener(() => OnCancelClicked?.Invoke());

            SetDefaultStats();
        }

        private async Task LoadRaceOptions()
        {
            if (_presenter == null)
                return;

            _availableRaces = await _presenter.GetRacesAsync();

            _raceIds = new List<string>();
            raceDropdown.ClearOptions();

            var options = new List<TMP_Dropdown.OptionData>();
            foreach (var race in _availableRaces)
            {
                options.Add(new TMP_Dropdown.OptionData(race.Name));
                _raceIds.Add(race.Id);
            }

            raceDropdown.AddOptions(options);
        }

        private async Task LoadClassOptions()
        {
            if (_presenter == null) return;

            _availableClasses = await _presenter.GetClassesAsync();

            _classIds = new List<string>();
            classDropdown.ClearOptions();

            var options = new List<TMP_Dropdown.OptionData>();
            foreach (var characterClass in _availableClasses)
            {
                options.Add(new TMP_Dropdown.OptionData(characterClass.Name));
                _classIds.Add(characterClass.Id);
            }

            classDropdown.AddOptions(options);
        }

        public void SetPresenter(CreateCharacterPresenter presenter)
        {
            _presenter = presenter;
        }

        public async Task Show()
        {
            gameObject.SetActive(true);
            SetDefaultStats();
            ClearError();

            await LoadRaceOptions();
            await LoadClassOptions();
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

                if (raceDropdown.value < 0 || raceDropdown.value >= _raceIds.Count)
                {
                    ShowError("Выберите расу");
                    return;
                }

                if (classDropdown.value < 0 || classDropdown.value >= _classIds.Count)
                {
                    ShowError("Выберите класс");
                    return;
                }

                var strength = GetStatValue(strengthInput, 10);
                var dexterity = GetStatValue(dexterityInput, 10);
                var constitution = GetStatValue(constitutionInput, 10);
                var intelligence = GetStatValue(intelligenceInput, 10);
                var wisdom = GetStatValue(wisdomInput, 10);
                var charisma = GetStatValue(charismaInput, 10);

                if (!ValidateStats(strength, dexterity, constitution, intelligence, wisdom, charisma))
                {
                    ShowError("Характеристики должны быть от 3 до 20");
                    return;
                }

                string raceId = _raceIds[raceDropdown.value];
                string classId = _classIds[classDropdown.value];

                OnCreateClicked?.Invoke(
                    nameInput.text,
                    raceId,
                    classId,
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

        private int GetStatValue(TMP_InputField input, int defaultValue)
        {
            if (input != null && int.TryParse(input.text, out int value))
                return Math.Clamp(value, 3, 20);

            return defaultValue;
        }

        private bool ValidateStats(params int[] stats)
        {
            foreach (var stat in stats)
            {
                if (stat < 3 || stat > 20)
                    return false;
            }

            return true;
        }

        private void SetDefaultStats()
        {
            if (strengthInput != null)
                strengthInput.text = "10";

            if (dexterityInput != null)
                dexterityInput.text = "10";

            if (constitutionInput != null)
                constitutionInput.text = "10";

            if (intelligenceInput != null)
                intelligenceInput.text = "10";

            if (wisdomInput != null)
                wisdomInput.text = "10";

            if (charismaInput != null)
                charismaInput.text = "10";
        }

        public void SetRaces(List<CharacterRace> races, List<string> raceIds)
        {
            throw new NotImplementedException();
        }

        public void SetClasses(List<CharacterClass> classes, List<string> classIds)
        {
            throw new NotImplementedException();
        }

        public void ClearError()
        {
            if (errorText != null)
                errorText.gameObject.SetActive(false);
        }

        public void ShowError(string message)
        {
            if (errorText != null)
            {
                errorText.text = message;
                errorText.gameObject.SetActive(true);
                Invoke(nameof(ClearError), 3f);
            }

            //Показать UI уведомление
        }

        public void ShowSuccess(string message)
        {
            Debug.Log($"Success: {message}");
            //Показать UI уведомление
        }

        public void ShowLoading(bool show)
        {
            if (_loadingSpinner != null)
                _loadingSpinner.SetActive(show);
        }
    }
}