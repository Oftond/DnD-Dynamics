using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingOverlay : MonoBehaviour
{
    [Header("Loading Elements")]
    [SerializeField] private GameObject _loadingPanel;
    [SerializeField] private Image _spinnerImage;
    [SerializeField] private TextMeshProUGUI _loadingText;

    [Header("Animation Settings")]
    [SerializeField] private float _rotationSpeed = 180;

    private bool _isActive = false;

    private void Update()
    {
        if (_isActive && _spinnerImage != null)
            _spinnerImage.rectTransform.Rotate(0, 0, -_rotationSpeed * Time.deltaTime);
    }

    public void Show(string message = "Загрузка...")
    {
        if (_loadingPanel != null)
            _loadingPanel.SetActive(true);

        if (_loadingText != null)
            _loadingText.text = message;

        _isActive = true;
    }

    public void Hide()
    {
        if (_loadingPanel != null)
            _loadingPanel.SetActive(false);

        _isActive = false;
    }
}