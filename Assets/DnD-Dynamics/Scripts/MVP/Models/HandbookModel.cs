using DnD_Dynamics.MVP.Presenters;
using DnD_Dynamics.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DnD_Dynamics.Models
{
    public class HandbookModel
    {
        private readonly IDataService _dataService;

        private List<Spell> _spells;
        private List<Item> _items;
        private List<Monster> _monsters;
        private List<CharacterRace> _races;
        private List<CharacterClass> _classes;

        private bool _isSpellsLoading = false;
        private bool _isItemsLoading = false;
        private bool _isMonstersLoading = false;
        private bool _isRacesLoading = false;
        private bool _isClassesLoading = false;

        public event Action<List<Spell>> OnSpellsChanged;
        public event Action<List<Item>> OnItemsChanged;
        public event Action<List<Monster>> OnMonstersChanged;
        public event Action<List<CharacterRace>> OnRacesChanged;
        public event Action<List<CharacterClass>> OnClassesChanged;

        public HandbookModel(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<List<Spell>> GetSpellsAsync()
        {
            if (_spells != null)
                return _spells;

            if (_isSpellsLoading)
                return _spells;

            _isSpellsLoading = true;

            _spells = await _dataService.GetSpellsAsync();
            OnSpellsChanged?.Invoke(_spells);

            _isSpellsLoading = false;
            return _spells;
        }

        public List<Spell> GetSpells() => GetSpellsAsync().ConfigureAwait(false).GetAwaiter().GetResult();

        public async Task<List<Item>> GetItemsAsync()
        {
            if (_items != null)
                return _items;

            if (_isItemsLoading)
                return _items;

            _isItemsLoading = true;

            _items = await _dataService.GetItemsAsync();
            OnItemsChanged?.Invoke(_items);

            _isItemsLoading = false;
            return _items;
        }

        public List<Item> GetItems() => GetItemsAsync().ConfigureAwait(false).GetAwaiter().GetResult();

        public async Task<List<Monster>> GetMonstersAsync()
        {
            if (_monsters != null)
                return _monsters;

            if (_isMonstersLoading)
                return _monsters;

            _isMonstersLoading = true;

            _monsters = await _dataService.GetMonstersAsync();
            OnMonstersChanged?.Invoke(_monsters);

            _isMonstersLoading = false;
            return _monsters;
        }

        public List<Monster> GetMonsters() => GetMonstersAsync().ConfigureAwait(false).GetAwaiter().GetResult();

        public async Task<List<CharacterRace>> GetRacesAsync()
        {
            if (_races != null)
                return _races;

            if (_isRacesLoading)
                return _races;

            _isRacesLoading = true;

            _races = await _dataService.GetRacesAsync();
            OnRacesChanged?.Invoke(_races);

            _isRacesLoading = false;
            return _races;
        }

        public List<CharacterRace> GetRaces() => GetRacesAsync().ConfigureAwait(false).GetAwaiter().GetResult();

        private async Task<List<CharacterClass>> GetClassesAsync()
        {
            if (_classes != null)
                return _classes;

            if (_isClassesLoading)
                return _classes;

            _isClassesLoading = true;

            _classes = await _dataService.GetClassesAsync();
            OnClassesChanged?.Invoke(_classes);

            _isClassesLoading = false;
            return _classes;
        }

        public List<CharacterClass> GetClasses() => GetClassesAsync().ConfigureAwait(false).GetAwaiter().GetResult();

        public async Task<List<HandbookEntity>> GetItemsByCategoryAsync(HandbookCategory category)
        {
            return category switch
            {
                HandbookCategory.Spells => (await GetSpellsAsync()).Cast<HandbookEntity>().ToList(),
                HandbookCategory.Items => (await GetItemsAsync()).Cast<HandbookEntity>().ToList(),
                HandbookCategory.Monsters => (await GetMonstersAsync()).Cast<HandbookEntity>().ToList(),
                HandbookCategory.Races => (await GetRacesAsync()).Cast<HandbookEntity>().ToList(),
                HandbookCategory.Classes => (await GetClassesAsync()).Cast<HandbookEntity>().ToList(),
                _ => new List<HandbookEntity>()
            };
        }

        public List<HandbookEntity> GetItemsByCategory(HandbookCategory category) => GetItemsByCategoryAsync(category).ConfigureAwait(false).GetAwaiter().GetResult();

        public async Task AddSpellAsync(Spell spell)
        {
            spell.IsHomebrew = true;
            await _dataService.AddSpellAsync(spell);

            if (_spells != null)
            {
                _spells.Add(spell);
                OnSpellsChanged?.Invoke(_spells);
            }
        }

        public void AddSpell(Spell spell) => AddSpellAsync(spell).ConfigureAwait(false).GetAwaiter().GetResult();

        public async Task AddItemAsync(Item item)
        {
            item.IsHomebrew = true;
            await _dataService.AddItemAsync(item);

            if (_items != null)
            {
                _items.Add(item);
                OnItemsChanged?.Invoke(_items);
            }
        }

        public void AddItem(Item item) => AddItemAsync(item).ConfigureAwait(false).GetAwaiter().GetResult();

        public async Task AddMonsterAsync(Monster monster)
        {
            monster.IsHomebrew = true;
            await _dataService.AddMonsterAsync(monster);

            if (_monsters != null)
            {
                _monsters.Add(monster);
                OnMonstersChanged?.Invoke(_monsters);
            }
        }

        public void AddMonster(Monster monster) => AddMonsterAsync(monster).ConfigureAwait(false).GetAwaiter().GetResult();

        public async Task AddRaceAsync(CharacterRace race)
        {
            race.IsHomebrew = true;
            await _dataService.AddRaceAsync(race);

            if (_races != null)
            {
                _races.Add(race);
                OnRacesChanged?.Invoke(_races);
            }
        }

        public void AddRace(CharacterRace race) => AddRaceAsync(race).ConfigureAwait(false).GetAwaiter().GetResult();

        public async Task AddClassAsync(CharacterClass charClass)
        {
            charClass.IsHomebrew = true;
            await _dataService.AddClassAsync(charClass);

            if (_classes != null)
            {
                _classes.Add(charClass);
                OnClassesChanged?.Invoke(_classes);
            }
        }

        public void AddClass(CharacterClass charClass) => AddClassAsync(charClass).ConfigureAwait(false).GetAwaiter().GetResult();

        public async Task DeleteSpellAsync(string id)
        {
            await _dataService.DeleteSpellAsync(id);

            if (_spells != null)
            {
                _spells.RemoveAll(s => s.Id == id);
                OnSpellsChanged?.Invoke(_spells);
            }
        }

        public void DeleteSpell(string id) => DeleteSpellAsync(id).ConfigureAwait(false).GetAwaiter().GetResult();

        public async Task DeleteItemAsync(string id)
        {
            await _dataService.DeleteItemAsync(id);

            if (_items != null)
            {
                _items.RemoveAll(i => i.Id == id);
                OnItemsChanged?.Invoke(_items);
            }
        }

        public void DeleteItem(string id) => DeleteItemAsync(id).ConfigureAwait(false).GetAwaiter().GetResult();

        public async Task DeleteMonsterAsync(string id)
        {
            await _dataService.DeleteMonsterAsync(id);

            if (_monsters != null)
            {
                _monsters.RemoveAll(m => m.Id == id);
                OnMonstersChanged?.Invoke(_monsters);
            }
        }

        public void DeleteMonster(string id) => DeleteMonsterAsync(id).ConfigureAwait(false).GetAwaiter().GetResult();

        public async Task DeleteRaceAsync(string id)
        {
            await _dataService.DeleteRaceAsync(id);

            if (_races != null)
            {
                _races.RemoveAll(r => r.Id == id);
                OnRacesChanged?.Invoke(_races);
            }
        }

        public void DeleteRace(string id) => DeleteRaceAsync(id).ConfigureAwait(false).GetAwaiter().GetResult();

        public async Task DeleteClassAsync(string id)
        {
            await _dataService.DeleteClassAsync(id);

            if (_classes != null)
            {
                _classes.RemoveAll(c => c.Id == id);
                OnClassesChanged?.Invoke(_classes);
            }
        }

        public void DeleteClass(string id) => DeleteClassAsync(id).ConfigureAwait(false).GetAwaiter().GetResult();

        public async Task ToggleFavoriteAsync(string id, HandbookCategory category)
        {
            await _dataService.ToggleFavoriteAsync(id, category);

            HandbookEntity target = category switch
            {
                HandbookCategory.Spells => _spells?.FirstOrDefault(x => x.Id == id),
                HandbookCategory.Items => _items?.FirstOrDefault(x => x.Id == id),
                HandbookCategory.Monsters => _monsters?.FirstOrDefault(x => x.Id == id),
                HandbookCategory.Races => _races?.FirstOrDefault(x => x.Id == id),
                HandbookCategory.Classes => _classes?.FirstOrDefault(x => x.Id == id),
                _ => null
            };

            if (target != null)
            {
                target.IsFavorite = !target.IsFavorite;

                switch (category)
                {
                    case HandbookCategory.Spells: OnSpellsChanged?.Invoke(_spells); break;
                    case HandbookCategory.Items: OnItemsChanged?.Invoke(_items); break;
                    case HandbookCategory.Monsters: OnMonstersChanged?.Invoke(_monsters); break;
                    case HandbookCategory.Races: OnRacesChanged?.Invoke(_races); break;
                    case HandbookCategory.Classes: OnClassesChanged?.Invoke(_classes); break;
                }
            }
        }

        public void ToggleFavorite(string id, HandbookCategory category) => ToggleFavoriteAsync(id, category).ConfigureAwait(false).GetAwaiter().GetResult();
    }
}