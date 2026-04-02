using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterView : MonoBehaviour, ICharacterView
{
    [Header("UI References")]
    [SerializeField] private Transform charactersContainer;
    [SerializeField] private GameObject characterItemPrefab;
    [SerializeField] private TextMeshProUGUI errorText;
    [SerializeField] private TextMeshProUGUI successText;
    [SerializeField] private GameObject loadingSpinner;

    private Dictionary<string, CharacterListItemView> _characterItems = new Dictionary<string, CharacterListItemView>();

    public event System.Action<string> OnCharacterSelected;

    public void DisplayCharacters(List<CharacterUIData> characters)
    {
        ClearCharacters();

        if (characters == null || characters.Count == 0)
        {
            ShowEmptyState(true);
            return;
        }

        ShowEmptyState(false);

        foreach (var character in characters)
        {
            AddCharacterItem(character);
        }
    }

    public void DisplayCharacterDetails(CharacterUIData character)
    {
        OnCharacterSelected?.Invoke(character.Id);
    }

    public void ShowError(string message)
    {
        if (errorText != null)
        {
            errorText.text = message;
            errorText.gameObject.SetActive(true);
            Invoke(nameof(HideError), 3f);
        }
        Debug.LogError(message);
    }

    public void ShowSuccess(string message)
    {
        if (successText != null)
        {
            successText.text = message;
            successText.gameObject.SetActive(true);
            Invoke(nameof(HideSuccess), 2f);
        }
        Debug.Log(message);
    }

    public void ShowLoading(bool show)
    {
        if (loadingSpinner != null)
            loadingSpinner.SetActive(show);
    }

    public void ClearSelection()
    {
        foreach (var item in _characterItems.Values)
        {
            item.SetSelected(false);
        }
    }

    private void AddCharacterItem(CharacterUIData character)
    {
        if (characterItemPrefab == null || charactersContainer == null) return;

        var itemObject = Instantiate(characterItemPrefab, charactersContainer);
        var itemView = itemObject.GetComponent<CharacterListItemView>();

        if (itemView != null)
        {
            itemView.Setup(character);
            itemView.OnClicked += () => OnCharacterSelected?.Invoke(character.Id);
            _characterItems[character.Id] = itemView;
        }
    }

    private void ClearCharacters()
    {
        foreach (var item in _characterItems.Values)
        {
            if (item != null)
                Destroy(item.gameObject);
        }
        _characterItems.Clear();
    }

    private void ShowEmptyState(bool show)
    {
        // Можно добавить пустое состояние
    }

    private void HideError()
    {
        if (errorText != null)
            errorText.gameObject.SetActive(false);
    }

    private void HideSuccess()
    {
        if (successText != null)
            successText.gameObject.SetActive(false);
    }
}