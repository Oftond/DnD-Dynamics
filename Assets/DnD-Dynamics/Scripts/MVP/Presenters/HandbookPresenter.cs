using DnD_Dynamics.Models;
using DnD_Dynamics.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DnD_Dynamics.MVP.Presenters
{
    public enum HandbookCategory
    {
        Spells,
        Items,
        Monsters,
        Races,
        Classes
    }

    public class HandbookPresenter
    {
        private readonly HandbookModel _model;
        private readonly IHandbookFilterService _filterService;
        private IHandbookView _view;

        private HandbookCategory _currentCategory = HandbookCategory.Spells;
        private List<HandbookEntity> _currentItems = new List<HandbookEntity>();
        private HandbookEntity _selectedItem;

        public HandbookPresenter(HandbookModel model, IHandbookFilterService filterService)
        {
            _model = model;
            _filterService = filterService;

            _model.OnSpellsChanged += _ => RefreshCurrentCategory();
            _model.OnItemsChanged += _ => RefreshCurrentCategory();
            _model.OnMonstersChanged += _ => RefreshCurrentCategory();
            _model.OnRacesChanged += _ => RefreshCurrentCategory();
            _model.OnClassesChanged += _ => RefreshCurrentCategory();
        }

        public void SetView(IHandbookView view)
        {
            _view = view;
        }

        public void LoadCategory(HandbookCategory category, bool favoritesOnly = false, bool homebrewOnly = false)
        {
            _currentCategory = category;
            var allItems = _model.GetItemsByCategory(category);

            _currentItems = ApplyViewMode(allItems, favoritesOnly, homebrewOnly);
            ApplyFiltersAndSearch();
        }

        private List<HandbookEntity> ApplyViewMode(List<HandbookEntity> items, bool favoritesOnly, bool homebrewOnly)
        {
            if (favoritesOnly)
                return items.Where(x => x.IsFavorite).ToList();

            if (homebrewOnly)
                return items.Where(x => x.IsHomebrew).ToList();

            return items;
        }

        public void ApplyFiltersAndSearch(string searchQuery = null,
            int? spellLevel = null, SpellSchool? spellSchool = null,
            ItemRarity? itemRarity = null, ItemType? itemType = null,
            float? monsterMinCr = null, float? monsterMaxCr = null,
            MonsterType? monsterType = null, MonsterSize? monsterSize = null)
        {
            List<HandbookEntity> filtered = _currentItems;

            switch (_currentCategory)
            {
                case HandbookCategory.Spells:
                    var spells = _currentItems.Cast<Spell>().ToList();
                    var filteredSpells = _filterService.FilterSpells(spells, spellLevel, spellSchool, null);
                    filtered = _filterService.SearchByName(filteredSpells, searchQuery).Cast<HandbookEntity>().ToList();
                    break;

                case HandbookCategory.Items:
                    var items = _currentItems.Cast<Item>().ToList();
                    var rarities = itemRarity.HasValue ? new List<ItemRarity> { itemRarity.Value } : null;
                    var types = itemType.HasValue ? new List<ItemType> { itemType.Value } : null;
                    var filteredItems = _filterService.FilterItems(items, rarities, types);
                    filtered = _filterService.SearchByName(filteredItems, searchQuery).Cast<HandbookEntity>().ToList();
                    break;

                case HandbookCategory.Monsters:
                    var monsters = _currentItems.Cast<Monster>().ToList();
                    var filteredMonsters = _filterService.FilterMonsters(monsters, monsterMinCr, monsterMaxCr, monsterType, monsterSize);
                    filtered = _filterService.SearchByName(filteredMonsters, searchQuery).Cast<HandbookEntity>().ToList();
                    break;

                case HandbookCategory.Races:
                    var races = _currentItems.Cast<CharacterRace>().ToList();
                    filtered = _filterService.SearchByName(races, searchQuery).Cast<HandbookEntity>().ToList();
                    break;

                case HandbookCategory.Classes:
                    var classes = _currentItems.Cast<CharacterClass>().ToList();
                    filtered = _filterService.SearchByName(classes, searchQuery).Cast<HandbookEntity>().ToList();
                    break;
            }

            _view?.DisplayItems(filtered);
        }

        private void RefreshCurrentCategory()
        {
            var allItems = _model.GetItemsByCategory(_currentCategory);
            _currentItems = allItems;
            ApplyFiltersAndSearch();
        }

        public void SelectItem(string id)
        {
            _selectedItem = _currentItems.FirstOrDefault(x => x.Id == id);
            if (_selectedItem != null)
                _view?.DisplayDetails(_selectedItem);
        }

        public void ToggleFavorite(string id)
        {
            _model.ToggleFavorite(id, _currentCategory);
        }

        public void DeleteItem(string id)
        {
            switch (_currentCategory)
            {
                case HandbookCategory.Spells:
                    _model.DeleteSpell(id);
                    break;
                case HandbookCategory.Items:
                    _model.DeleteItem(id);
                    break;
                case HandbookCategory.Monsters:
                    _model.DeleteMonster(id);
                    break;
                case HandbookCategory.Races:
                    _model.DeleteRace(id);
                    break;
                case HandbookCategory.Classes:
                    _model.DeleteClass(id);
                    break;
            }
            RefreshCurrentCategory();
        }

        public void CreateNewSpell(Spell spell) => _model.AddSpell(spell);

        public void CreateNewItem(Item item) => _model.AddItem(item);

        public void CreateNewMonster(Monster monster) => _model.AddMonster(monster);

        public void CreateNewRace(CharacterRace race) => _model.AddRace(race);

        public void CreateNewClass(CharacterClass charClass) => _model.AddClass(charClass);

        public HandbookCategory GetCurrentCategory() => _currentCategory;
    }
}