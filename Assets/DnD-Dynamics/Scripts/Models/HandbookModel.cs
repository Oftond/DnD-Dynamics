using DnD_Dynamics.MVP.Presenters;
using DnD_Dynamics.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DnD_Dynamics.Models
{
    public class HandbookModel
    {
        private readonly IHandbookDataService _dataService;

        private List<Spell> _spells;
        private List<Item> _items;
        private List<Monster> _monsters;
        private List<CharacterRace> _races;
        private List<CharacterClass> _classes;

        public event Action<List<Spell>> OnSpellsChanged;
        public event Action<List<Item>> OnItemsChanged;
        public event Action<List<Monster>> OnMonstersChanged;
        public event Action<List<CharacterRace>> OnRacesChanged;
        public event Action<List<CharacterClass>> OnClassesChanged;

        public HandbookModel(IHandbookDataService dataService)
        {
            _dataService = dataService;
            LoadAllData();
        }

        private void LoadAllData()
        {
            _spells = _dataService.GetAllSpells();
            _items = _dataService.GetAllItems();
            _monsters = _dataService.GetAllMonsters();
            _races = _dataService.GetAllRaces();
            _classes = _dataService.GetAllClasses();
        }

        public List<Spell> GetSpells() => _spells.ToList();
        public List<Item> GetItems() => _items.ToList();
        public List<Monster> GetMonsters() => _monsters.ToList();
        public List<CharacterRace> GetRaces() => _races.ToList();
        public List<CharacterClass> GetClasses() => _classes.ToList();

        public List<HandbookEntity> GetItemsByCategory(HandbookCategory category)
        {
            return category switch
            {
                HandbookCategory.Spells => _spells.Cast<HandbookEntity>().ToList(),
                HandbookCategory.Items => _items.Cast<HandbookEntity>().ToList(),
                HandbookCategory.Monsters => _monsters.Cast<HandbookEntity>().ToList(),
                HandbookCategory.Races => _races.Cast<HandbookEntity>().ToList(),
                HandbookCategory.Classes => _classes.Cast<HandbookEntity>().ToList(),
                _ => new List<HandbookEntity>()
            };
        }

        public void AddSpell(Spell spell)
        {
            spell.IsHomebrew = true;
            _spells.Add(spell);
            _dataService.AddSpell(spell);
            OnSpellsChanged?.Invoke(_spells);
        }

        public void AddItem(Item item)
        {
            item.IsHomebrew = true;
            _items.Add(item);
            _dataService.AddItem(item);
            OnItemsChanged?.Invoke(_items);
        }

        public void AddMonster(Monster monster)
        {
            monster.IsHomebrew = true;
            _monsters.Add(monster);
            _dataService.AddMonster(monster);
            OnMonstersChanged?.Invoke(_monsters);
        }

        public void AddRace(CharacterRace race)
        {
            race.IsHomebrew = true;
            _races.Add(race);
            _dataService.AddRace(race);
            OnRacesChanged?.Invoke(_races);
        }

        public void AddClass(CharacterClass charClass)
        {
            charClass.IsHomebrew = true;
            _classes.Add(charClass);
            _dataService.AddClass(charClass);
            OnClassesChanged?.Invoke(_classes);
        }

        public void DeleteSpell(string id)
        {
            _spells.RemoveAll(s => s.Id == id);
            _dataService.DeleteSpell(id);
            OnSpellsChanged?.Invoke(_spells);
        }

        public void DeleteItem(string id)
        {
            _items.RemoveAll(i => i.Id == id);
            _dataService.DeleteItem(id);
            OnItemsChanged?.Invoke(_items);
        }

        public void DeleteMonster(string id)
        {
            _monsters.RemoveAll(m => m.Id == id);
            _dataService.DeleteMonster(id);
            OnMonstersChanged?.Invoke(_monsters);
        }

        public void DeleteRace(string id)
        {
            _races.RemoveAll(r => r.Id == id);
            _dataService.DeleteRace(id);
            OnRacesChanged?.Invoke(_races);
        }

        public void DeleteClass(string id)
        {
            _classes.RemoveAll(c => c.Id == id);
            _dataService.DeleteClass(id);
            OnClassesChanged?.Invoke(_classes);
        }

        public void ToggleFavorite(string id, HandbookCategory category)
        {
            _dataService.ToggleFavorite(id, category);

            switch (category)
            {
                case HandbookCategory.Spells:
                    var spell = _spells.FirstOrDefault(s => s.Id == id);
                    if (spell != null) spell.IsFavorite = !spell.IsFavorite;
                    OnSpellsChanged?.Invoke(_spells);
                    break;

                case HandbookCategory.Items:
                    var item = _items.FirstOrDefault(i => i.Id == id);
                    if (item != null) item.IsFavorite = !item.IsFavorite;
                    OnItemsChanged?.Invoke(_items);
                    break;

                case HandbookCategory.Monsters:
                    var monster = _monsters.FirstOrDefault(m => m.Id == id);
                    if (monster != null) monster.IsFavorite = !monster.IsFavorite;
                    OnMonstersChanged?.Invoke(_monsters);
                    break;

                case HandbookCategory.Races:
                    var race = _races.FirstOrDefault(r => r.Id == id);
                    if (race != null) race.IsFavorite = !race.IsFavorite;
                    OnRacesChanged?.Invoke(_races);
                    break;

                case HandbookCategory.Classes:
                    var charClass = _classes.FirstOrDefault(c => c.Id == id);
                    if (charClass != null) charClass.IsFavorite = !charClass.IsFavorite;
                    OnClassesChanged?.Invoke(_classes);
                    break;
            }
        }
    }
}