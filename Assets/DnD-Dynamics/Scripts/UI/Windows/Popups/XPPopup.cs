using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XPPopup : PopupBase
{
    [Header("XP Display")]
    [SerializeField] private TextMeshProUGUI _currentLevelText;
    [SerializeField] private TextMeshProUGUI _nextLevelText;
    [SerializeField] private TextMeshProUGUI _currentXPText;
    [SerializeField] private TextMeshProUGUI _requiredXPText;
    [SerializeField] private Slider _xpSlider;
    [SerializeField] private TextMeshProUGUI _xpProgressText;

    [Header("XP Input")]
    [SerializeField] private TMP_InputField _xpInputField;
    [SerializeField] private Button _addXPButton;
    [SerializeField] private Button _removeXPButton;
    [SerializeField] private Button _levelUpButton;

    [Header("Visual Feedback")]
    [SerializeField] private GameObject _levelUpAvailableIndicator;

    private int _currentLevel;
    private int _currentXP;

    public event Action<int> OnXPChanged;
    public event Action OnLevelUpRequested;

    public void Setup(int currentLevel, int currentXP)
    {
        _currentLevel = currentLevel;
        _currentXP = currentXP;

        UpdateUI();
    }

    protected override void Awake()
    {
        base.Awake();

        _addXPButton?.onClick.AddListener(OnAddXPClicked);
        _removeXPButton?.onClick.AddListener(OnRemoveXPClicked);
        _levelUpButton?.onClick.AddListener(OnLevelUpClicked);
    }

    private void UpdateUI()
    {
        int xpForCurrentLevel = ExperienceTable.GetExperienceForLevel(_currentLevel);
        int xpForNextLevel = ExperienceTable.GetExperienceForLevel(_currentLevel + 1);
        int xpProgress = _currentXP - xpForCurrentLevel;
        int xpNeeded = xpForNextLevel - xpForCurrentLevel;
        float progress = ExperienceTable.CalculateProgress(_currentLevel, _currentXP);
        int remainingXP = ExperienceTable.GetRemainingXP(_currentLevel, _currentXP);
        bool canLevelUp = ExperienceTable.CanLevelUp(_currentLevel, _currentXP);

        if (_currentLevelText != null)
            _currentLevelText.text = $"Уровень: {_currentLevel}";

        if (_nextLevelText != null)
            _nextLevelText.text = _currentLevel < 20 ? $"Следующий: {_currentLevel + 1}" : "Максимальный уровень";

        if (_currentXPText != null)
            _currentXPText.text = $"Текущий опыт: {_currentXP}";

        if (_requiredXPText != null)
            _requiredXPText.text = _currentLevel < 20 ? $"Требуется: {xpForNextLevel}" : "Максимум достигнут";

        if (_xpSlider != null)
            _xpSlider.value = progress;

        if (_xpProgressText != null)
        {
            _xpProgressText.text = _currentLevel < 20 ? $"{xpProgress} / {xpNeeded}" : "MAX";
        }

        if (_levelUpButton != null)
            _levelUpButton.interactable = canLevelUp;

        _levelUpAvailableIndicator?.SetActive(canLevelUp);
    }

    private void OnAddXPClicked()
    {
        if (int.TryParse(_xpInputField.text, out int amount) && amount > 0)
        {
            OnXPChanged?.Invoke(amount);
            Close();
        }
        else
        {
            Debug.LogWarning("[XPPopup] Некорректное количество опыта");
        }
    }

    private void OnRemoveXPClicked()
    {
        if (int.TryParse(_xpInputField.text, out int amount) && amount > 0)
        {
            OnXPChanged?.Invoke(-amount);
            Close();
        }
        else
        {
            Debug.LogWarning("[XPPopup] Некорректное количество опыта");
        }
    }

    private void OnLevelUpClicked()
    {
        if (ExperienceTable.CanLevelUp(_currentLevel, _currentXP))
        {
            OnLevelUpRequested?.Invoke();
            Close();
        }
    }

    protected override void OnPopupOpened()
    {
        Debug.LogWarning("[XPPopup] A pop-up window is open for editing the experience.");
    }

    protected override void OnPopupClosed()
    {
        Debug.LogWarning("[XPPopup] A pop-up window for editing the experience is closed");
    }
}