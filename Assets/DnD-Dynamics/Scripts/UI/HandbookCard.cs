using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HandbookCard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI subtitleText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Image levelBadge;
    [SerializeField] private Toggle favoriteToggle;
    [SerializeField] private GameObject homebrewIcon;
    [SerializeField] private Button clickButton;
    [SerializeField] private Image backgroundImage;

    private HandbookEntity _item;

    public event System.Action<HandbookEntity> OnClick;
    public event System.Action<HandbookEntity, bool> OnFavoriteToggle;

    public void Setup(HandbookEntity item)
    {
        _item = item;
        titleText.text = item.Name;
        homebrewIcon.SetActive(item.IsHomebrew);
        favoriteToggle.isOn = item.IsFavorite;

        if (item is Spell spell)
        {
            subtitleText.text = spell.GetSchoolDisplayName();
            levelText.text = spell.GetLevelDisplayName();
            levelBadge.color = GetLevelColor((int)spell.Level);
            levelBadge.gameObject.SetActive(true);
        }
        else if (item is Item itemData)
        {
            subtitleText.text = itemData.GetTypeDisplayName();

            levelBadge.gameObject.SetActive(false);
        }
        else if (item is Monster monster)
        {
            subtitleText.text = monster.GetTypeDisplayName();
            levelText.text = $"CR {monster.ChallengeRating}";
            levelBadge.color = GetCrColor(monster.ChallengeRating);
            levelBadge.gameObject.SetActive(true);
        }

        clickButton.onClick.RemoveAllListeners();
        clickButton.onClick.AddListener(() => OnClick?.Invoke(_item));

        favoriteToggle.onValueChanged.RemoveAllListeners();
        favoriteToggle.onValueChanged.AddListener((isOn) => OnFavoriteToggle?.Invoke(_item, isOn));
    }

    public void SetFavorite(bool isFavorite) => favoriteToggle.isOn = isFavorite;

    private Color GetLevelColor(int level)
    {
        return level switch
        {
            0 => new Color(0.6f, 0.6f, 0.6f),
            1 => new Color(0.2f, 0.8f, 0.2f),
            2 => new Color(0.2f, 0.5f, 0.9f),
            3 => new Color(0.7f, 0.3f, 0.8f),
            4 => new Color(0.9f, 0.4f, 0.6f),
            5 => new Color(0.9f, 0.6f, 0.2f),
            _ => new Color(0.9f, 0.2f, 0.2f)
        };
    }

    private Color GetCrColor(float cr)
    {
        if (cr <= 1) return new Color(0.2f, 0.8f, 0.2f);
        if (cr <= 5) return new Color(0.2f, 0.5f, 0.9f);
        if (cr <= 10) return new Color(0.9f, 0.6f, 0.2f);

        return new Color(0.9f, 0.2f, 0.2f);
    }
}