using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArmorClassPopup : PopupBase
{
    [Header("Current Values Display")]
    [SerializeField] private TextMeshProUGUI _baseACText;
    [SerializeField] private TextMeshProUGUI _shieldBonusText;
    [SerializeField] private TextMeshProUGUI _acBonusText;
    [SerializeField] private TextMeshProUGUI _totalACText;

    [Header("Input Fields")]
    [SerializeField] private TMP_InputField _baseACInput;
    [SerializeField] private TMP_InputField _shieldBonusInput;
    [SerializeField] private TMP_InputField _acBonusInput;

    [Header("Shield Toggle")]
    [SerializeField] private Toggle _shieldToggle;
    [SerializeField] private TextMeshProUGUI _shieldStatusText;

    [Header("Buttons")]
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _resetButton;

    private int _baseAC;
    private int _shieldBonus;
    private int _acBonus;
    private bool _isShieldActive;

    public event Action<(int baseAC, int shieldBonus, int acBonus, bool isShieldActive)> OnACSettingsChanged;

    public void Setup(int baseAC, int shieldBonus, int acBonus, bool isShieldActive)
    {
        _baseAC = baseAC;
        _shieldBonus = shieldBonus;
        _acBonus = acBonus;
        _isShieldActive = isShieldActive;

        UpdateUI();
    }

    protected override void Awake()
    {
        base.Awake();

        _saveButton?.onClick.AddListener(OnSaveClicked);
        _resetButton?.onClick.AddListener(OnResetClicked);
        _shieldToggle?.onValueChanged.AddListener(OnShieldToggleChanged);

        _baseACInput?.onValueChanged.AddListener(_ => UpdateTotalACDisplay());
        _shieldBonusInput?.onValueChanged.AddListener(_ => UpdateTotalACDisplay());
        _acBonusInput?.onValueChanged.AddListener(_ => UpdateTotalACDisplay());
    }

    private void UpdateUI()
    {
        if (_baseACInput != null)
            _baseACInput.text = _baseAC.ToString();

        if (_shieldBonusInput != null)
            _shieldBonusInput.text = _shieldBonus.ToString();

        if (_acBonusInput != null)
            _acBonusInput.text = _acBonus.ToString();

        if (_shieldToggle != null)
            _shieldToggle.isOn = _isShieldActive;

        UpdateTotalACDisplay();
    }

    private void UpdateTotalACDisplay()
    {
        int baseAC = int.TryParse(_baseACInput?.text, out int b) ? b : _baseAC;
        int shieldBonus = int.TryParse(_shieldBonusInput?.text, out int s) ? s : _shieldBonus;
        int acBonus = int.TryParse(_acBonusInput?.text, out int a) ? a : _acBonus;
        bool isShieldActive = _shieldToggle != null ? _shieldToggle.isOn : _isShieldActive;

        int totalAC = baseAC + acBonus + (isShieldActive ? shieldBonus : 0);

        if (_baseACText != null)
            _baseACText.text = $"Базовое КД: {baseAC}";

        if (_shieldBonusText != null)
            _shieldBonusText.text = $"Бонус щита: {shieldBonus}";

        if (_acBonusText != null)
            _acBonusText.text = $"Другие бонусы: {acBonus:+0;-0;0}";

        if (_totalACText != null)
            _totalACText.text = $"Итого КД: {totalAC}";

        if (_shieldStatusText != null)
            _shieldStatusText.text = isShieldActive ? "Щит активен" : "Щит неактивен";
    }

    private void OnShieldToggleChanged(bool isOn)
    {
        _isShieldActive = isOn;

        UpdateTotalACDisplay();
    }

    private void OnSaveClicked()
    {
        int baseAC = int.TryParse(_baseACInput.text, out int b) ? b : _baseAC;
        int shieldBonus = int.TryParse(_shieldBonusInput.text, out int s) ? s : _shieldBonus;
        int acBonus = int.TryParse(_acBonusInput.text, out int a) ? a : _acBonus;
        bool isShieldActive = _shieldToggle != null ? _shieldToggle.isOn : _isShieldActive;

        OnACSettingsChanged?.Invoke((baseAC, shieldBonus, acBonus, isShieldActive));
        Close();
    }

    private void OnResetClicked()
    {
        Setup(_baseAC, _shieldBonus, _acBonus, _isShieldActive);
    }

    protected override void OnPopupOpened()
    {
        Debug.LogWarning("[ArmorClassPopup] A pop-up window for editing the armor class is open.");
    }

    protected override void OnPopupClosed()
    {
        Debug.LogWarning("[ArmorClassPopup] A pop-up window for editing the armor class is closed.");
    }
}