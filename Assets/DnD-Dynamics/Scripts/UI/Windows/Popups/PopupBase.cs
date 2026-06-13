using UnityEngine;
using UnityEngine.UI;

public abstract class PopupBase : MonoBehaviour
{
    [Header("Popup Elements")]
    [SerializeField] protected GameObject popupPanel;
    [SerializeField] protected Button _buttonClose;

    protected bool _isOpen = false;

    public bool IsOpen => _isOpen;

    protected virtual void Awake()
    {
        _buttonClose?.onClick.AddListener(Close);

        if (popupPanel != null)
            popupPanel.SetActive(false);
    }

    public virtual void Open()
    {
        if (popupPanel != null)
        {
            popupPanel.SetActive(true);
            _isOpen = true;

            OnPopupOpened();
        }
    }

    public virtual void Close()
    {
        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
            _isOpen = false;

            OnPopupClosed();
        }
    }

    protected abstract void OnPopupOpened();

    protected abstract void OnPopupClosed();
}