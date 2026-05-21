using DnD_Dynamics.MVP.Presenters;
using DnD_Dynamics.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace DnD_Dynamics.UI.Windows
{
    public class HandbookWindow : MonoBehaviour
    {
        [Header("Tabs")]
        [SerializeField] private Button spellsTab;
        [SerializeField] private Button itemsTab;
        [SerializeField] private Button monstersTab;
        [SerializeField] private GameObject activeTabIndicator;

        [Header("View Mode Toggles")]
        [SerializeField] private Toggle allModeToggle;
        [SerializeField] private Toggle favoritesModeToggle;
        [SerializeField] private Toggle homebrewModeToggle;
        [SerializeField] private GameObject modeTogglePanel;

        [Header("Search & Filter")]
        [SerializeField] private TMP_InputField searchInput;
        [SerializeField] private Button filterToggleButton;
        [SerializeField] private GameObject filterPanel;

        [Header("Spell Filters")]
        [SerializeField] private TMP_Dropdown spellLevelFilter;
        [SerializeField] private TMP_Dropdown spellSchoolFilter;
        [SerializeField] private TMP_Dropdown spellClassFilter;

        [Header("Item Filters")]
        [SerializeField] private TMP_Dropdown itemRarityFilter;
        [SerializeField] private TMP_Dropdown itemTypeFilter;

        [Header("Monster Filters")]
        [SerializeField] private Slider monsterCrMinSlider;
        [SerializeField] private Slider monsterCrMaxSlider;
        [SerializeField] private TextMeshProUGUI monsterCrMinText;
        [SerializeField] private TextMeshProUGUI monsterCrMaxText;
        [SerializeField] private TMP_Dropdown monsterTypeFilter;
        [SerializeField] private TMP_Dropdown monsterSizeFilter;

        [Header("Content")]
        [SerializeField] private Transform contentContainer;
        [SerializeField] private GameObject cardPrefab;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private GameObject emptyStatePanel;
        [SerializeField] private TextMeshProUGUI emptyStateText;

        [Header("Details Panel")]
        [SerializeField] private GameObject detailsPanel;
        [SerializeField] private TextMeshProUGUI detailsTitle;
        [SerializeField] private TextMeshProUGUI detailsText;
        [SerializeField] private Button detailsFavoriteButton;
        [SerializeField] private Button detailsCloseButton;
        [SerializeField] private Button editButton;
        [SerializeField] private Button deleteButton;

        private IDataService _dataService;
        private IHandbookFilterService _filterService;
        private List<CharacterClass> _allClasses;
        private Dictionary<int, string> _classIdMap = new Dictionary<int, string>();

        private HandbookCategory _currentCategory = HandbookCategory.Spells;
        private ViewMode _currentViewMode = ViewMode.All;
        private List<HandbookEntity> _currentItems = new List<HandbookEntity>();
        private Dictionary<string, HandbookCard> _cards = new Dictionary<string, HandbookCard>();
        private HandbookEntity _selectedItem;

        private enum ViewMode { All, Favorites, Homebrew }

        [Inject]
        public void Construct(IDataService dataService, IHandbookFilterService filterService)
        {
            _dataService = dataService;
            _filterService = filterService;
        }

        private async Task LoadClassFilterOptions()
        {
            _allClasses = await _dataService.GetClassesAsync();

            spellClassFilter.ClearOptions();
            var options = new List<TMP_Dropdown.OptionData> { new TMP_Dropdown.OptionData("Все классы") };

            for (int i = 0; i < _allClasses.Count; i++)
            {
                options.Add(new TMP_Dropdown.OptionData(_allClasses[i].Name));
                _classIdMap[i + 1] = _allClasses[i].Id;
            }
            spellClassFilter.AddOptions(options);
        }

        private async void Start()
        {
            await LoadClassFilterOptions();

            InitializeTabs();

            InitializeViewModes();

            InitializeFilters();

            InitializeDetailsPanel();

            await LoadCategory(HandbookCategory.Spells);
        }

        private void InitializeTabs()
        {
            spellsTab.onClick.AddListener(async () => await LoadCategory(HandbookCategory.Spells));
            itemsTab.onClick.AddListener(async () => await LoadCategory(HandbookCategory.Items));
            monstersTab.onClick.AddListener(async () => await LoadCategory(HandbookCategory.Monsters));

            searchInput.onValueChanged.AddListener(async (q) => await OnSearchChanged(q));
            filterToggleButton.onClick.AddListener(() => filterPanel.SetActive(!filterPanel.activeSelf));
        }

        private void InitializeViewModes()
        {
            allModeToggle.onValueChanged.AddListener(async (isOn) =>
            {
                if (isOn)
                {
                    _currentViewMode = ViewMode.All;

                    await ApplyFiltersAsync();
                }
            });
            favoritesModeToggle.onValueChanged.AddListener(async (isOn) =>
            {
                if (isOn)
                {
                    _currentViewMode = ViewMode.Favorites;

                    await ApplyFiltersAsync();
                }
            });
            homebrewModeToggle.onValueChanged.AddListener(async (isOn) =>
            {
                if (isOn)
                {
                    _currentViewMode = ViewMode.Homebrew;

                    await ApplyFiltersAsync();
                }
            });
        }

        private void InitializeFilters()
        {
            spellLevelFilter.options = new List<TMP_Dropdown.OptionData>
            {
                new TMP_Dropdown.OptionData("Все уровни"),
                new TMP_Dropdown.OptionData("Заговоры"),
                new TMP_Dropdown.OptionData("1 круг"), new TMP_Dropdown.OptionData("2 круг"),
                new TMP_Dropdown.OptionData("3 круг"), new TMP_Dropdown.OptionData("4 круг"),
                new TMP_Dropdown.OptionData("5 круг"), new TMP_Dropdown.OptionData("6 круг"),
                new TMP_Dropdown.OptionData("7 круг"), new TMP_Dropdown.OptionData("8 круг"),
                new TMP_Dropdown.OptionData("9 круг")
            };
            spellLevelFilter.onValueChanged.AddListener(async _ => await ApplyFiltersAsync());

            spellSchoolFilter.options = new List<TMP_Dropdown.OptionData>
            {
                new TMP_Dropdown.OptionData("Все школы"),
                new TMP_Dropdown.OptionData("Ограждение"), new TMP_Dropdown.OptionData("Призыв"),
                new TMP_Dropdown.OptionData("Прорицание"), new TMP_Dropdown.OptionData("Очарование"),
                new TMP_Dropdown.OptionData("Воплощение"), new TMP_Dropdown.OptionData("Иллюзия"),
                new TMP_Dropdown.OptionData("Некромантия"), new TMP_Dropdown.OptionData("Преобразование")
            };
            spellSchoolFilter.onValueChanged.AddListener(async _ => await ApplyFiltersAsync());
            spellClassFilter.onValueChanged.AddListener(async _ => await ApplyFiltersAsync());

            itemRarityFilter.options = new List<TMP_Dropdown.OptionData>
            {
                new TMP_Dropdown.OptionData("Все редкости"),
                new TMP_Dropdown.OptionData("Обычный"), new TMP_Dropdown.OptionData("Необычный"),
                new TMP_Dropdown.OptionData("Редкий"), new TMP_Dropdown.OptionData("Очень редкий"),
                new TMP_Dropdown.OptionData("Легендарный"), new TMP_Dropdown.OptionData("Артефакт")
            };
            itemRarityFilter.onValueChanged.AddListener(async _ => await ApplyFiltersAsync());

            itemTypeFilter.options = new List<TMP_Dropdown.OptionData>
            {
                new TMP_Dropdown.OptionData("Все типы"),
                new TMP_Dropdown.OptionData("Оружие"), new TMP_Dropdown.OptionData("Доспех"),
                new TMP_Dropdown.OptionData("Щит"), new TMP_Dropdown.OptionData("Волшебная палочка"),
                new TMP_Dropdown.OptionData("Жезл"), new TMP_Dropdown.OptionData("Посох"),
                new TMP_Dropdown.OptionData("Кольцо"), new TMP_Dropdown.OptionData("Чудесный предмет"),
                new TMP_Dropdown.OptionData("Зелье"), new TMP_Dropdown.OptionData("Свиток"),
                new TMP_Dropdown.OptionData("Инструмент"), new TMP_Dropdown.OptionData("Прочее")
            };
            itemTypeFilter.onValueChanged.AddListener(async _ => await ApplyFiltersAsync());

            monsterCrMinSlider.onValueChanged.AddListener(async v => { monsterCrMinText.text = v.ToString("F1"); await ApplyFiltersAsync(); });
            monsterCrMaxSlider.onValueChanged.AddListener(async v => { monsterCrMaxText.text = v.ToString("F1"); await ApplyFiltersAsync(); });
            monsterTypeFilter.onValueChanged.AddListener(async _ => await ApplyFiltersAsync());
            monsterSizeFilter.onValueChanged.AddListener(async _ => await ApplyFiltersAsync());
        }

        private void InitializeDetailsPanel()
        {
            detailsCloseButton.onClick.AddListener(() => detailsPanel.SetActive(false));
            detailsFavoriteButton.onClick.AddListener(() =>
            {
                if (_selectedItem != null)
                {
                    _dataService.ToggleFavoriteAsync(_selectedItem.Id, _currentCategory);
                    detailsFavoriteButton.GetComponent<Image>().color = _selectedItem.IsFavorite ? Color.yellow : Color.gray;
                    UpdateCardFavorite(_selectedItem.Id, _selectedItem.IsFavorite);
                }
            });
        }

        private async Task LoadCategory(HandbookCategory category)
        {
            _currentCategory = category;
            _currentViewMode = ViewMode.All;
            allModeToggle.isOn = true;
            ClearCards();
            await ApplyFiltersAsync();
            UpdateEmptyStateText();
        }

        private void UpdateEmptyStateText()
        {
            string modeText = _currentViewMode switch
            {
                ViewMode.Favorites => "избранных",
                ViewMode.Homebrew => "своих",
                _ => ""
            };
            emptyStateText.text = $"Нет {modeText} {GetCategoryDisplayName()}";
        }

        private string GetCategoryDisplayName()
        {
            return _currentCategory switch
            {
                HandbookCategory.Spells => "заклинаний",
                HandbookCategory.Items => "предметов",
                HandbookCategory.Monsters => "монстров",
                HandbookCategory.Races => "рас",
                HandbookCategory.Classes => "классов",
                _ => "элементов"
            };
        }

        private async Task ApplyFiltersAsync()
        {
            string searchQuery = searchInput.text;
            List<HandbookEntity> baseItems;

            switch (_currentCategory)
            {
                case HandbookCategory.Spells:
                    var allSpells = (await _dataService.GetSpellsAsync()).Cast<HandbookEntity>().ToList();

                    baseItems = _currentViewMode switch
                    {
                        ViewMode.Favorites => allSpells.Where(s => s.IsFavorite).ToList(),

                        ViewMode.Homebrew => allSpells.Where(s => s.IsHomebrew).ToList(),

                        _ => allSpells.Cast<HandbookEntity>().ToList()
                    };
                    break;

                case HandbookCategory.Items:
                    var allItems = (await _dataService.GetItemsAsync()).Cast<HandbookEntity>().ToList();

                    baseItems = _currentViewMode switch
                    {
                        ViewMode.Favorites => allItems.Where(i => i.IsFavorite).ToList(),
                        ViewMode.Homebrew => allItems.Where(i => i.IsHomebrew).ToList(),
                        _ => allItems
                    };
                    break;

                case HandbookCategory.Monsters:
                    var allMonsters = (await _dataService.GetMonstersAsync()).Cast<HandbookEntity>().ToList();
                    baseItems = _currentViewMode switch
                    {
                        ViewMode.Favorites => allMonsters.Where(m => m.IsFavorite).ToList(),
                        ViewMode.Homebrew => allMonsters.Where(m => m.IsHomebrew).ToList(),
                        _ => allMonsters
                    };
                    break;

                default:
                    baseItems = new List<HandbookEntity>();
                    break;
            }

            _currentItems = baseItems;
            List<HandbookEntity> filtered;

            switch (_currentCategory)
            {
                case HandbookCategory.Spells:
                    int level = spellLevelFilter.value == 0 ? -1 : spellLevelFilter.value - 1;
                    SpellSchool? school = spellSchoolFilter.value == 0 ? null : (SpellSchool?)(spellSchoolFilter.value - 1);
                    string requiredClassId = spellClassFilter.value > 0 && _classIdMap.ContainsKey(spellClassFilter.value) ? _classIdMap[spellClassFilter.value] : null;

                    var spells = _currentItems.Cast<Spell>().ToList();
                    var filteredSpells = _filterService.FilterSpells(spells, level == -1 ? null : level, school, requiredClassId);
                    filtered = _filterService.SearchByName(filteredSpells, searchQuery).Cast<HandbookEntity>().ToList();
                    break;

                case HandbookCategory.Items:
                    ItemRarity? rarity = itemRarityFilter.value == 0 ? null : (ItemRarity?)(itemRarityFilter.value - 1);
                    ItemType? type = itemTypeFilter.value == 0 ? null : (ItemType?)(itemTypeFilter.value - 1);

                    var items = _currentItems.Cast<Item>().ToList();
                    var rarities = rarity.HasValue ? new List<ItemRarity> { rarity.Value } : null;
                    var types = type.HasValue ? new List<ItemType> { type.Value } : null;
                    var filteredItems = _filterService.FilterItems(items, rarities, types);
                    filtered = _filterService.SearchByName(filteredItems, searchQuery).Cast<HandbookEntity>().ToList();
                    break;

                case HandbookCategory.Monsters:
                    float minCr = monsterCrMinSlider.value;
                    float maxCr = monsterCrMaxSlider.value;
                    MonsterType? monsterType = monsterTypeFilter.value == 0 ? null : (MonsterType?)(monsterTypeFilter.value - 1);
                    MonsterSize? size = monsterSizeFilter.value == 0 ? null : (MonsterSize?)(monsterSizeFilter.value - 1);

                    var monsters = _currentItems.Cast<Monster>().ToList();
                    var filteredMonsters = _filterService.FilterMonsters(monsters, minCr, maxCr, monsterType, size);
                    filtered = _filterService.SearchByName(filteredMonsters, searchQuery).Cast<HandbookEntity>().ToList();
                    break;

                default:
                    filtered = _filterService.SearchByName(_currentItems, searchQuery);
                    break;
            }

            DisplayItems(filtered);
            emptyStatePanel.SetActive(filtered.Count == 0);
        }

        private void DisplayItems(List<HandbookEntity> items)
        {
            ClearCards();

            foreach (var item in items)
            {
                var cardObj = Instantiate(cardPrefab, contentContainer);
                var card = cardObj.GetComponent<HandbookCard>();
                card.Setup(item);
                card.OnClick += ShowDetails;
                card.OnFavoriteToggle += (i, isFavorite) =>
                {
                    _dataService.ToggleFavoriteAsync(i.Id, _currentCategory);
                    i.IsFavorite = isFavorite;
                };
                _cards[item.Id] = card;
            }

            scrollRect.verticalNormalizedPosition = 1f;
        }

        private void ShowDetails(HandbookEntity item)
        {
            _selectedItem = item;
            detailsTitle.text = item.Name;
            detailsText.text = BuildDescriptionText(item);
            detailsFavoriteButton.GetComponent<Image>().color = item.IsFavorite ? Color.yellow : Color.gray;
            detailsPanel.SetActive(true);
        }

        private string BuildDescriptionText(HandbookEntity item)
        {
            if (item is Spell spell)
            {
                return $"{spell.GetSchoolDisplayName()} | {spell.GetLevelDisplayName()}\n" +
                       $"Время: {spell.GetCastingTimeDisplayName()}\n" +
                       $"Дистанция: {spell.Range}\n" +
                       $"Компоненты: {spell.GetComponentsDisplayString()}\n" +
                       $"Длительность: {spell.GetDurationDisplayName()}\n" +
                       (spell.IsRitual ? "📖 Ритуал\n" : "") +
                       $"\n{spell.FullDescription}";
            }
            else if (item is Item itemData)
            {
                return $"{itemData.GetTypeDisplayName()} | {itemData.GetRarityDisplayName()}\n" +
                       $"Вес: {itemData.Weight} фт. | Стоимость: {itemData.Cost} зм\n" +
                       $"\n{itemData.Description}";
            }
            else if (item is Monster monster)
            {
                return $"{monster.GetSizeDisplayName()} {monster.GetTypeDisplayName()}\n" +
                       $"КБ: {monster.ArmorClass} | ХП: {monster.HitPoints}\n" +
                       $"Скорость: {monster.WalkSpeed} фт.\n" +
                       $"Сложность: {monster.ChallengeRating}\n" +
                       $"\n{monster.Description}";
            }
            return item.Description;
        }

        private void UpdateCardFavorite(string id, bool isFavorite)
        {
            if (_cards.TryGetValue(id, out var card))
                card.SetFavorite(isFavorite);
        }

        private void ClearCards()
        {
            foreach (var card in _cards.Values)
                Destroy(card.gameObject);
            _cards.Clear();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        private async Task OnSearchChanged(string query) => await ApplyFiltersAsync();
    }
}