using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotificationPopup : PopupBase
{
    [Header("Notification Elements")]
    [SerializeField] private TextMeshProUGUI _messageText;
    [SerializeField] private Image _iconImage;
    [SerializeField] private Image _backgroundImage;

    [Header("Styles")]
    [SerializeField] private Color _successColor = new Color(0.2f, 0.8f, 0.2f);
    [SerializeField] private Color _errorColor = new Color(0.8f, 0.2f, 0.2f);
    [SerializeField] private Sprite _successIcon;
    [SerializeField] private Sprite _errorIcon;

    [Header("Auto-hide")]
    [SerializeField] private bool _autoHide = true;
    [SerializeField] private float _autoHideDelay = 3f;

    private float _hideTimer;

    private void Update()
    {
        if (_autoHide && _isOpen)
        {
            _hideTimer -= Time.deltaTime;
            if (_hideTimer <= 0)
                Close();
        }
    }

    public void ShowSuccess(string message)
    {
        SetupNotification(message, _successColor, _successIcon);
        Open();
    }

    public void ShowError(string message)
    {
        SetupNotification(message, _errorColor, _errorIcon);
        Open();
    }

    private void SetupNotification(string message, Color color, Sprite icon)
    {
        if (_messageText != null)
            _messageText.text = message;

        if (_backgroundImage != null)
            _backgroundImage.color = color;

        if (_iconImage != null)
        {
            _iconImage.sprite = icon;
            _iconImage.color = color;
        }
    }

    protected override void OnPopupOpened()
    {
        Debug.LogWarning("[NotificationPopup] A pop-up notification window is open.");

        if (_autoHide)
            _hideTimer = _autoHideDelay;
    }

    protected override void OnPopupClosed()
    {
        Debug.LogWarning("[NotificationPopup] A pop-up notification window is closed.");
    }
}