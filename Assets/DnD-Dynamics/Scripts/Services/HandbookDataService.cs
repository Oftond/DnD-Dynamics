using DnD_Dynamics.MVP.Presenters;
using DnD_Dynamics.Services;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Zenject;

namespace DnD_Dynamics.Services
{
    public class HandbookDataService : IHandbookDataService
    {
        private readonly IDataService _dataService;

        [Inject]
        public HandbookDataService(IDataService dataService)
        {
            _dataService = dataService;
        }

        public List<Spell> GetAllSpells() => _dataService.GetAllSpells();

        public Spell GetSpellById(string id) => _dataService.GetSpellById(id);

        public void AddSpell(Spell spell) => _dataService.AddSpell(spell);

        public void UpdateSpell(Spell spell) => _dataService.UpdateSpell(spell);

        public void DeleteSpell(string id) => _dataService.DeleteSpell(id);

        public List<Item> GetAllItems() => _dataService.GetAllItems();

        public Item GetItemById(string id) => _dataService.GetItemById(id);

        public void AddItem(Item item) => _dataService.AddItem(item);

        public void UpdateItem(Item item) => _dataService.UpdateItem(item);

        public void DeleteItem(string id) => _dataService.DeleteItem(id);

        public List<Monster> GetAllMonsters() => _dataService.GetAllMonsters();

        public Monster GetMonsterById(string id) => _dataService.GetMonsterById(id);

        public void AddMonster(Monster monster) => _dataService.AddMonster(monster);

        public void UpdateMonster(Monster monster) => _dataService.UpdateMonster(monster);

        public void DeleteMonster(string id) => _dataService.DeleteMonster(id);

        public List<CharacterRace> GetAllRaces() => _dataService.GetAllRaces();

        public CharacterRace GetRaceById(string id) => _dataService.GetRaceById(id);

        public void AddRace(CharacterRace race) => _dataService.AddRace(race);

        public void UpdateRace(CharacterRace race) => _dataService.UpdateRace(race);

        public void DeleteRace(string id) => _dataService.DeleteRace(id);

        public List<CharacterClass> GetAllClasses() => _dataService.GetAllClasses();

        public CharacterClass GetClassById(string id) => _dataService.GetClassById(id);

        public void AddClass(CharacterClass characterClass) => _dataService.AddClass(characterClass);

        public void UpdateClass(CharacterClass characterClass) => _dataService.UpdateClass(characterClass);

        public void DeleteClass(string id) => _dataService.DeleteClass(id);

        public void ToggleFavorite(string id, HandbookCategory category) => _dataService.ToggleFavorite(id, category);

        public List<T> GetFavoritesByCategory<T>(HandbookCategory category) where T : HandbookEntity => _dataService.GetFavoritesByCategory<T>(category);
    }
}