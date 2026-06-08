using DnD_Dynamics.MVP.Presenter;
using DnD_Dynamics.MVP.View;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CharacterDetailWindow : MonoBehaviour, ICharacterDetailView
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
    [SerializeField] private GameObject _loadingSpinner;

    [Header("Portrait Settings")]
    [SerializeField] private int _maxTextureSize = 512;
    [SerializeField] private string _portraitsFolder = "Portraits";
    [SerializeField] private Button _changePortraitButton;

    private CharacterDetailPresenter _presenter;
    private string _currentCharacterId;

    private Dictionary<string, Texture2D> _textureCache = new Dictionary<string, Texture2D>();

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
            UpdateUI();
        });

        healButton?.onClick.AddListener(() =>
        {
            OnHealClicked?.Invoke(GetDamageHealAmount());
            UpdateUI();
        });

        levelUpButton?.onClick.AddListener(() =>
        {
            OnLevelUpClicked?.Invoke();
            UpdateUI();
        });

        deleteButton?.onClick.AddListener(() => OnDeleteClicked?.Invoke());

        editButton?.onClick.AddListener(() => OnEditClicked?.Invoke());

        backButton?.onClick.AddListener(() => OnBackClicked?.Invoke());

        _changePortraitButton?.onClick.AddListener(OnLoadPortraitClicked);

        if (damageHealInput != null)
            damageHealInput.text = "5";
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

    public void DisplayCharacterDetails(CharacterUIData character)
    {
        UpdateUI();
    }

    public void ShowError(string message)
    {
        Debug.LogError($"Error: {message}");
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

    public void ClearSelection()
    {
        ClearUI();
    }

    private void UpdateUI()
    {
        if (_presenter == null) return;

        var character = _presenter.GetSelectedCharacter();

        if (character == null)
        {
            Debug.LogError("Error of the selected character", this);
            return;
        }

        _currentCharacterId = character.Id;

        StartCoroutine(LoadTextureFromPath(character.PortraitPath));

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
            backstoryText.text = string.IsNullOrEmpty(character.Backstory) ? "История не указана" : character.Backstory;

        if (notesText != null)
            notesText.text = string.IsNullOrEmpty(character.Notes) ? "Нет заметок" : character.Notes;
    }

    private async void OnLoadPortraitClicked()
    {
        if (string.IsNullOrEmpty(_currentCharacterId))
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

    private int GetDamageHealAmount()
    {
        if (damageHealInput != null && int.TryParse(damageHealInput.text, out int amount))
            return Mathf.Max(1, amount);

        return 5;
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
        // 1. Копируем файл в папку приложения
        string savePath = GetPortraitSavePath(_currentCharacterId);

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

        // 2. Загружаем текстуру из сохранённого пути
        yield return LoadTextureFromPath(savePath);

        // 3. Сохраняем путь в модели (вызываем async метод БЕЗ await)
        if (_presenter != null)
        {
            // Вызываем fire-and-forget, но с обработкой ошибок
            _ = SavePortraitPathAsync(savePath);
        }
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

    private IEnumerator LoadTextureFromPath(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogError($"[Portrait] Файл не найден: {path}");
            yield break;
        }

        // Проверяем кэш
        if (_textureCache.TryGetValue(path, out var cachedTexture))
        {
            ApplyTextureToImage(cachedTexture);
            yield break;
        }

        // Загружаем через UnityWebRequest
        string fileUrl = "file://" + path;
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(fileUrl))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result == UnityWebRequest.Result.Success)
            {
                var texture = DownloadHandlerTexture.GetContent(uwr);

                // Опционально: уменьшаем размер для экономии памяти
                if (texture.width > _maxTextureSize || texture.height > _maxTextureSize)
                {
                    texture = ResizeTexture(texture, _maxTextureSize);
                }

                _textureCache[path] = texture;
                ApplyTextureToImage(texture);
            }
            else
            {
                Debug.LogError($"[Portrait] Ошибка загрузки: {uwr.error}");
                ShowError("Не удалось загрузить изображение");
            }
        }
    }

    private void ApplyTextureToImage(Texture2D texture)
    {
        if (portraitImage == null || texture == null) return;

        var sprite = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f)
        );

        portraitImage.sprite = sprite;
    }

    private Texture2D ResizeTexture(Texture2D source, int maxSize)
    {
        float ratio = Mathf.Min(
            (float)maxSize / source.width,
            (float)maxSize / source.height
        );

        int newWidth = Mathf.RoundToInt(source.width * ratio);
        int newHeight = Mathf.RoundToInt(source.height * ratio);

        RenderTexture rt = RenderTexture.GetTemporary(newWidth, newHeight);
        Graphics.Blit(source, rt);

        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = rt;

        Texture2D resized = new Texture2D(newWidth, newHeight, TextureFormat.RGBA32, false);
        resized.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0, 0);
        resized.Apply();

        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(rt);

        return resized;
    }
}