using DnD_Dynamics.MVP.Presenter;
using DnD_Dynamics.MVP.View;
using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Zenject;

public class CharacterDetailWindow : MonoBehaviour, ICharacterDetailView
{
    [Inject] private IPortraitDataService _portraitDataService;

    [Header("Basic Info")]
    [SerializeField] private TextMeshProUGUI characterNameText;
    [SerializeField] private TextMeshProUGUI classRaceText;
    [SerializeField] private TextMeshProUGUI levelText;

    [Header("Speed Info")]
    [SerializeField] private TextMeshProUGUI _speedText;

    [Header("Experience")]
    [SerializeField] private TextMeshProUGUI _xpText;
    [SerializeField] private Button _changeXP;

    [Header("Initiative")]
    [SerializeField] private TextMeshProUGUI _initiativeText;

    [Header("Portrait Info")]
    [SerializeField] private Image portraitImage;
    [SerializeField] private Sprite _defaultPortraitSprite;
    [SerializeField] private Button _changePortraitButton;

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
    [SerializeField] private Button _buttonArmorClass;
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

    [Header("Popups")]
    [SerializeField] private XPPopup _xpPopup;
    [SerializeField] private ArmorClassPopup _armorClassPopup;
    [SerializeField] private NotificationPopup _notificationPopup;
    [SerializeField] private LoadingOverlay _loadingOverlay;

    [Header("Loading")]
    [SerializeField] private GameObject _loadingSpinner;

    [Header("Portrait Settings")]
    [SerializeField] private int _maxTextureSize = 512;
    [SerializeField] private string _portraitsFolder = "Portraits";

    private CharacterDetailPresenter _presenter;
    private string _selectedCharacterId;

    public event Action<int> OnXPChangeRequested;
    public event Action OnLevelUpFromPopupRequested;
    public event Action<(int baseAC, int shieldBonus, int acBonus, bool isShieldActive)> OnACSettingsChanged;

    public event Action OnBackClicked;
    public event Action<int> OnDamageClicked;
    public event Action<int> OnHealClicked;
    public event Action OnLevelUpClicked;
    public event Action OnDeleteClicked;
    public event Action OnEditClicked;

    private void Start()
    {
        EnsurePortraitsFolderExists();

        damageButton?.onClick.AddListener(() =>
        {
            OnDamageClicked?.Invoke(GetDamageHealAmount());
        });

        healButton?.onClick.AddListener(() =>
        {
            OnHealClicked?.Invoke(GetDamageHealAmount());
        });

        levelUpButton?.onClick.AddListener(() =>
        {
            OnLevelUpClicked?.Invoke();
        });

        deleteButton?.onClick.AddListener(() => OnDeleteClicked?.Invoke());

        editButton?.onClick.AddListener(() => OnEditClicked?.Invoke());

        backButton?.onClick.AddListener(() => OnBackClicked?.Invoke());

        _changePortraitButton?.onClick.AddListener(OnLoadPortraitClicked);

        if (_portraitDataService != null)
            _portraitDataService.OnPortraitLoaded += OnPortraitLoaded;

        if (damageHealInput != null)
            damageHealInput.text = "5";

        _buttonArmorClass?.onClick.AddListener(OnArmorClassButtonClicked);

        _changeXP?.onClick.AddListener(OnXPButtonClicked);

        if (_xpPopup != null)
        {
            _xpPopup.OnXPChanged += (amount) => OnXPChangeRequested?.Invoke(amount);
            _xpPopup.OnLevelUpRequested += () => OnLevelUpFromPopupRequested?.Invoke();
        }

        if (_armorClassPopup != null)
        {
            _armorClassPopup.OnACSettingsChanged += (s) => OnACSettingsChanged?.Invoke(s);
        }
    }

    private void OnDestroy()
    {
        if (_portraitDataService != null)
            _portraitDataService.OnPortraitLoaded -= OnPortraitLoaded;
    }

    public void SetPresenter(CharacterDetailPresenter presenter)
    {
        _presenter = presenter;
        _presenter.SetView(this);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnXPButtonClicked()
    {
        var character = _presenter?.GetSelectedCharacter();
        if (character == null) return;

        _xpPopup.Setup(character.Level, character.ExperiencePoints);

        _xpPopup.Open();
    }

    public void DisplayCharacterDetails(CharacterUIData character)
    {
        if (character == null)
        {
            Debug.LogError("Error of the selected character", this);
            return;
        }

        _selectedCharacterId = character.Id;

        LoadPortrait(character.PortraitPath);

        UpdateUI(character);
    }

    public void ShowError(string message)
    {
        Debug.LogError($"Error: {message}");

        _notificationPopup?.ShowError(message);
    }

    public void ShowSuccess(string message)
    {
        Debug.Log($"Success: {message}");

        _notificationPopup?.ShowSuccess(message);
    }

    public void ShowLoading(bool show)
    {
        if (show)
            _loadingOverlay?.Show();
        else
            _loadingOverlay?.Hide();
    }

    public void ClearSelection()
    {
        ClearUI();
    }

    private void UpdateUI(CharacterUIData character)
    {
        if (_presenter == null) return;

        if (_speedText != null)
            _speedText.text = $"{character.BaseSpeed} фт.";

        if (characterNameText != null)
            characterNameText.text = character.Name;

        if (classRaceText != null)
            classRaceText.text = character.ClassRaceText;

        if (levelText != null)
            levelText.text = character.LevelText;

        if (_xpText != null)
            _xpText.text = character.ExperienceText;

        if (hpText != null)
            hpText.text = character.HpText;

        //if (hpSlider != null)
        //    hpSlider.value = (float)character.CurrentHp / character.MaxHp;

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

        if (_initiativeText != null)
            _initiativeText.text = character.InitiativeText;

        if (proficiencyText != null)
            proficiencyText.text = character.ProficiencyText;

        if (goldText != null)
            goldText.text = $"Золото: {character.Gold}";

        if (silverText != null)
            silverText.text = $"Серебро: {character.Silver}";

        if (copperText != null)
            copperText.text = $"Медь: {character.Copper}";

        if (backstoryText != null)
            backstoryText.text = string.IsNullOrEmpty(character.Backstory) ? "История не указана" : character.Backstory;

        if (notesText != null)
            notesText.text = string.IsNullOrEmpty(character.Notes) ? "Нет заметок" : character.Notes;
    }

    private void ClearUI()
    {
        if (characterNameText != null)
            characterNameText.text = "";

        if (classRaceText != null)
            classRaceText.text = "";

        if (hpText != null)
            hpText.text = "0/0";

        if (hpSlider != null)
            hpSlider.value = 0;
    }

    private void OnArmorClassButtonClicked()
    {
        Debug.Log("Open armor class edit dialog");

        var character = _presenter?.GetSelectedCharacter();

        if (character == null)
            return;

        _armorClassPopup.Setup(
            character.BaseArmorClass,
            character.ShieldBonus,
            character.ArmorClassBonus,
            character.IsShieldActive
        );

        _armorClassPopup.Open();
    }

    private int GetDamageHealAmount()
    {
        if (damageHealInput != null && int.TryParse(damageHealInput.text, out int amount))
            return Mathf.Max(1, amount);

        return 5;
    }

    #region Portrait
    private void OnPortraitLoaded(string path, Texture2D texture)
    {
        var character = _presenter?.GetSelectedCharacter();

        if (character != null && character.PortraitPath == path)
            ApplyTextureToImage(texture);
    }

    private void LoadPortrait(string portraitPath)
    {
        if (portraitImage == null) return;

        if (string.IsNullOrEmpty(portraitPath) || !File.Exists(portraitPath))
        {
            portraitImage.sprite = _defaultPortraitSprite;
            return;
        }

        var texture = _portraitDataService.GetPortrait(portraitPath);

        if (texture != null)
            ApplyTextureToImage(texture);
        else
            portraitImage.sprite = _defaultPortraitSprite;
    }

    private void ApplyTextureToImage(Texture2D texture)
    {
        if (portraitImage == null || texture == null)
            return;

        var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        portraitImage.sprite = sprite;
    }

    private async void OnLoadPortraitClicked()
    {
        if (string.IsNullOrEmpty(_selectedCharacterId))
        {
            Debug.LogWarning("[Portrait] Персонаж не выбран");
            return;
        }

#if UNITY_ANDROID || UNITY_IOS
        await LoadPortraitForMobile();
#elif UNITY_EDITOR || UNITY_STANDALONE
        LoadPortraitForEditor();
#else
        Debug.LogWarning("[Portrait] Платформа не поддерживается для загрузки портрета");
#endif
    }

    private void EnsurePortraitsFolderExists()
    {
        var path = Path.Combine(Application.persistentDataPath, _portraitsFolder);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            Debug.Log($"[Portrait] Создана папка: {path}");
        }
    }

    private void LoadPortraitForEditor()
    {
#if UNITY_EDITOR
        var path = UnityEditor.EditorUtility.OpenFilePanel(
            "Выберите изображение портрета",
            "",
            "png,jpg,jpeg"
        );

        if (!string.IsNullOrEmpty(path))
        {
            StartCoroutine(CopyAndLoadPortrait(path));
        }
#endif
    }

    private async Task LoadPortraitForMobile()
    {
        bool permission = NativeGallery.CheckPermission(NativeGallery.PermissionType.Read, NativeGallery.MediaType.Image);

        if (!permission)
        {
            NativeGallery.Permission permissionRequest = await NativeGallery.RequestPermissionAsync(NativeGallery.PermissionType.Read, NativeGallery.MediaType.Image);

            if (permissionRequest == NativeGallery.Permission.Denied)
            {
                Debug.LogWarning("[Portrait] Доступ к галерее запрещён пользователем");
                ShowError("Доступ к галерее запрещён. Разрешите доступ в настройках.");
                return;
            }

            if (permissionRequest == NativeGallery.Permission.ShouldAsk)
            {
                Debug.LogWarning("[Portrait] Доступ к галерее запрещён, нужно спросить");
                ShowError("Доступ к галерее запрещён. Разреши мне, ок?.");
                return;
            }
        }

        NativeGallery.GetImageFromGallery(
            callback: (path) => OnImagePickedFromGallery(path),
            title: "Выберите портрет"
        );
    }

    private void OnImagePickedFromGallery(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            Debug.Log("[Portrait] Пользователь отменил выбор");
            return;
        }

        Debug.Log($"[Portrait] Выбран файл: {path}");
        StartCoroutine(CopyAndLoadPortrait(path));
    }

    private string GetPortraitSavePath(string characterId)
    {
        var folder = Path.Combine(Application.persistentDataPath, _portraitsFolder);

        return Path.Combine(folder, $"{characterId}.png");
    }

    private IEnumerator CopyAndLoadPortrait(string sourcePath)
    {
        string savePath = GetPortraitSavePath(_selectedCharacterId);

        try
        {
            File.Copy(sourcePath, savePath, overwrite: true);
            Debug.Log($"[Portrait] Файл скопирован в: {savePath}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"[Portrait] Ошибка копирования: {ex.Message}");
            ShowError("Не удалось сохранить портрет");
            yield break;
        }

        if (_presenter != null)
            _ = SavePortraitPathAsync(savePath);
    }

    private async Task SavePortraitPathAsync(string path)
    {
        try
        {
            await _presenter.UpdatePortraitPathAsync(path);
            Debug.Log($"[Portrait] Путь сохранён в модели: {path}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"[Portrait] Ошибка сохранения пути: {ex.Message}");
            ShowError("Не удалось сохранить путь к портрету");
        }
    }
    #endregion
}