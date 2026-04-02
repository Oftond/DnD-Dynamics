using TMPro;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UI;

public class MainMenuWindow : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button charactersButton;
    [SerializeField] private Button createButton;
    [SerializeField] private Button exitButton;

    [Header("Title")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI versionText;

    public event System.Action OnCharactersClicked;
    public event System.Action OnCreateClicked;
    public event System.Action OnExitClicked;

    public void Initialize()
    {
        if (charactersButton != null)
            charactersButton.onClick.AddListener(() => OnCharactersClicked?.Invoke());

        if (createButton != null)
            createButton.onClick.AddListener(() => OnCreateClicked?.Invoke());

        if (exitButton != null)
            exitButton.onClick.AddListener(() => OnExitClicked?.Invoke());

        if (versionText != null)
            versionText.text = $"v{Constants.APP_VERSION}";
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}