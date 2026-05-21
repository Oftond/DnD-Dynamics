using DnD_Dynamics.MVP.Presenters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace DnD_Dynamics.Services
{
    public class DataService : IDataService
    {
        private readonly string _persistentPath;

        private readonly Lazy<Task<List<Spell>>> _lazySpells;
        private readonly Lazy<Task<List<Item>>> _lazyItems;
        private readonly Lazy<Task<List<Monster>>> _lazyMonsters;
        private readonly Lazy<Task<List<CharacterRace>>> _lazyRaces;
        private readonly Lazy<Task<List<CharacterClass>>> _lazyClasses;
        private readonly Lazy<Task<List<SkillData>>> _lazySkills;

        private List<Spell> _userSpells = new();
        private List<Item> _userItems = new();
        private List<Monster> _userMonsters = new();
        private List<CharacterRace> _userRaces = new();
        private List<CharacterClass> _userClasses = new();
        private List<CharacterData> _userCharacters = new();
        private HashSet<string> _favorites = new();

        private List<Spell> _baseSpells = new();
        private List<Item> _baseItems = new();
        private List<Monster> _baseMonsters = new();
        private List<CharacterRace> _baseRaces = new();
        private List<CharacterClass> _baseClasses = new();
        private List<SkillData> _baseSkills = new();

        private Dictionary<string, CharacterClass> _classDict = new();
        private Dictionary<string, Task> _loadingTasks = new();

        private bool _isSpellsLoaded = false;
        private bool _isItemsLoaded = false;
        private bool _isMonstersLoaded = false;
        private bool _isRacesLoaded = false;
        private bool _isClassesLoaded = false;

        private const string SPELLS_KEY = "spells";
        private const string ITEMS_KEY = "items";
        private const string MONSTERS_KEY = "monsters";
        private const string RACES_KEY = "races";
        private const string CLASSES_KEY = "classes";
        private const string CHARACTERS_KEY = "characters";
        private const string FAVORITES_KEY = "favorites";

        public event Action OnSpellsLoaded;
        public event Action OnItemsLoaded;
        public event Action OnMonstersLoaded;
        public event Action OnRacesLoaded;
        public event Action OnClassesLoaded;

        public bool IsSpellsLoaded => _isSpellsLoaded;
        public bool IsItemsLoaded => _isItemsLoaded;
        public bool IsMonstersLoaded => _isMonstersLoaded;
        public bool IsRacesLoaded => _isRacesLoaded;
        public bool IsClassesLoaded => _isClassesLoaded;

        public DataService()
        {
            _persistentPath = Path.Combine(Application.persistentDataPath, "GameData");

            if (!Directory.Exists(_persistentPath))
                Directory.CreateDirectory(_persistentPath);

            _lazySpells = new Lazy<Task<List<Spell>>>(() => LoadAllSpellsAsync());
            _lazyItems = new Lazy<Task<List<Item>>>(() => LoadAllItemsAsync());
            _lazyMonsters = new Lazy<Task<List<Monster>>>(() => LoadAllMonstersAsync());
            _lazyRaces = new Lazy<Task<List<CharacterRace>>>(() => LoadRacesAsync());
            _lazyClasses = new Lazy<Task<List<CharacterClass>>>(() => LoadClassesAsync());
            _lazySkills = new Lazy<Task<List<SkillData>>>(() => LoadSkillsAsync());

            LoadUserData();
        }

        private T LoadFromPersistent<T>(string key, T defaultValue = default)
        {
            try
            {
                string filePath = Path.Combine(_persistentPath, $"{key}.json");
                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePath);
                    return JsonConvert.DeserializeObject<T>(json);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Ошибка загрузки {key}: {ex.Message}");
            }
            return defaultValue;
        }

        private async Task SaveToPersistentAsync<T>(string key, T data)
        {
            await Task.Run(() =>
            {
                try
                {
                    string filePath = Path.Combine(_persistentPath, $"{key}.json");
                    string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                    File.WriteAllText(filePath, json);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Ошибка сохранения {key}: {ex.Message}");
                }
            });
        }

        private async Task<T> LoadFromResourcesAsync<T>(string path) where T : class
        {
            var tcs = new TaskCompletionSource<T>();

            ResourceRequest request = Resources.LoadAsync<TextAsset>(path);
            request.completed += (op) =>
            {
                try
                {
                    var asset = request.asset as TextAsset;
                    if (asset != null)
                    {
                        var result = JsonConvert.DeserializeObject<T>(asset.text);
                        tcs.SetResult(result);
                    }
                    else
                    {
                        Debug.LogWarning($"Файл {path}.json не найден в Resources");
                        tcs.SetResult(null);
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Ошибка загрузки {path}: {ex.Message}");
                    tcs.SetException(ex);
                }
            };

            return await tcs.Task;
        }

        private void BuildClassDictionary()
        {
            _classDict = new Dictionary<string, CharacterClass>();

            foreach (var c in _baseClasses)
                if (!_classDict.ContainsKey(c.Id))
                    _classDict[c.Id] = c;

            foreach (var c in _userClasses)
                _classDict[c.Id] = c;
        }

        private void SyncSpellsWithClasses()
        {
            foreach (var spell in _baseSpells)
                spell.SyncAvailableClasses(_classDict);
            foreach (var spell in _userSpells)
                spell.SyncAvailableClasses(_classDict);
        }

        private void LoadUserData()
        {
            _userSpells = LoadFromPersistent<List<Spell>>(SPELLS_KEY) ?? new List<Spell>();
            _userItems = LoadFromPersistent<List<Item>>(ITEMS_KEY) ?? new List<Item>();
            _userMonsters = LoadFromPersistent<List<Monster>>(MONSTERS_KEY) ?? new List<Monster>();
            _userRaces = LoadFromPersistent<List<CharacterRace>>(RACES_KEY) ?? new List<CharacterRace>();
            _userClasses = LoadFromPersistent<List<CharacterClass>>(CLASSES_KEY) ?? new List<CharacterClass>();
            _userCharacters = LoadFromPersistent<List<CharacterData>>(CHARACTERS_KEY) ?? new List<CharacterData>();
            _favorites = LoadFromPersistent<HashSet<string>>(FAVORITES_KEY) ?? new HashSet<string>();
        }

        private async Task<List<Spell>> LoadAllSpellsAsync()
        {
            if (_baseSpells != null && _baseSpells.Count > 0)
                return _baseSpells;

            string loadingKey = "spells";
            if (_loadingTasks.ContainsKey(loadingKey))
                await _loadingTasks[loadingKey];

            var tcs = new TaskCompletionSource<bool>();
            _loadingTasks[loadingKey] = tcs.Task;

            await Task.Run(async () =>
            {
                var baseSpells = await LoadFromResourcesAsync<List<Spell>>("Data/spells") ?? new List<Spell>();
                var userSpells = LoadFromPersistent<List<Spell>>(SPELLS_KEY) ?? new List<Spell>();

                var unitedSpells = UniteSpells(baseSpells, userSpells);

                await Task.Run(() =>
                {
                    _baseSpells = unitedSpells;
                    _isSpellsLoaded = true;
                    OnSpellsLoaded?.Invoke();
                });
                tcs.SetResult(true);
            });

            return _baseSpells;
        }

        private async Task<List<Item>> LoadAllItemsAsync()
        {
            if (_baseItems != null && _baseItems.Count > 0)
                return _baseItems;

            string loadingKey = "items";
            if (_loadingTasks.ContainsKey(loadingKey))
                await _loadingTasks[loadingKey];

            var tcs = new TaskCompletionSource<bool>();
            _loadingTasks[loadingKey] = tcs.Task;

            await Task.Run(async () =>
            {
                var baseItems = await LoadFromResourcesAsync<List<Item>>("Data/items") ?? new List<Item>();
                var userItems = LoadFromPersistent<List<Item>>(ITEMS_KEY) ?? new List<Item>();

                var unitedItems = UniteItems(baseItems, userItems);

                await Task.Run(() =>
                {
                    _baseItems = unitedItems;
                    _isItemsLoaded = true;
                    OnItemsLoaded?.Invoke();
                });
                tcs.SetResult(true);
            });

            return _baseItems;
        }

        private async Task<List<Monster>> LoadAllMonstersAsync()
        {
            if (_baseMonsters != null && _baseMonsters.Count > 0)
                return _baseMonsters;

            string loadingKey = "monsters";
            if (_loadingTasks.ContainsKey(loadingKey))
                await _loadingTasks[loadingKey];

            var tcs = new TaskCompletionSource<bool>();
            _loadingTasks[loadingKey] = tcs.Task;

            await Task.Run(async () =>
            {
                var baseMonsters = await LoadFromResourcesAsync<List<Monster>>("Data/monsters") ?? new List<Monster>();
                var userMonsters = LoadFromPersistent<List<Monster>>(MONSTERS_KEY) ?? new List<Monster>();

                var unitedMonsters = UniteMonsters(baseMonsters, userMonsters);

                await Task.Run(() =>
                {
                    _baseMonsters = unitedMonsters;
                    _isMonstersLoaded = true;
                    OnMonstersLoaded?.Invoke();
                });
                tcs.SetResult(true);
            });

            return _baseMonsters;
        }

        private async Task<List<CharacterRace>> LoadRacesAsync()
        {
            if (_baseRaces != null && _baseRaces.Count > 0)
                return _baseRaces;

            string loadingKey = "races";
            if (_loadingTasks.ContainsKey(loadingKey))
                await _loadingTasks[loadingKey];

            var tcs = new TaskCompletionSource<bool>();
            _loadingTasks[loadingKey] = tcs.Task;

            try
            {
                var baseRaces = await LoadFromResourcesAsync<List<CharacterRace>>("Data/races") ?? new List<CharacterRace>();

                var userRaces = await Task.Run(() => LoadFromPersistent<List<CharacterRace>>(RACES_KEY) ?? new List<CharacterRace>());

                var unitedRaces = UniteRaces(baseRaces, userRaces);

                _baseRaces = unitedRaces;
                _isRacesLoaded = true;
                OnRacesLoaded?.Invoke();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Ошибка загрузки рас: {ex.Message}");
                _baseRaces = new List<CharacterRace>();
            }
            finally
            {
                tcs.SetResult(true);
            }

            return _baseRaces;
        }

        private async Task<List<CharacterClass>> LoadClassesAsync()
        {
            if (_baseClasses != null && _baseClasses.Count > 0)
                return _baseClasses;

            string loadingKey = "classes";
            if (_loadingTasks.ContainsKey(loadingKey))
                await _loadingTasks[loadingKey];

            var tcs = new TaskCompletionSource<bool>();
            _loadingTasks[loadingKey] = tcs.Task;

            try
            {
                var baseClasses = await LoadFromResourcesAsync<List<CharacterClass>>("Data/classes") ?? new List<CharacterClass>();

                var userClasses = await Task.Run(() => LoadFromPersistent<List<CharacterClass>>(CLASSES_KEY) ?? new List<CharacterClass>());

                var unitedClasses = UniteClasses(baseClasses, userClasses);

                _baseClasses = unitedClasses;
                _isClassesLoaded = true;
                OnClassesLoaded?.Invoke();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Ошибка загрузки классов: {ex.Message}");
                _baseClasses = new List<CharacterClass>();
            }
            finally
            {
                tcs.SetResult(true);
            }

            return _baseClasses;
        }

        private async Task<List<SkillData>> LoadSkillsAsync()
        {
            if (_baseSkills != null && _baseSkills.Count > 0)
                return _baseSkills;

            _baseSkills = await LoadFromResourcesAsync<List<SkillData>>("Data/skills") ?? new List<SkillData>();

            return _baseSkills;
        }

        private List<Spell> UniteSpells(List<Spell> baseSpells, List<Spell> userSpells)
        {
            var result = new List<Spell>();
            var userDict = userSpells.ToDictionary(s => s.Id, s => s);

            foreach (var spell in baseSpells ?? new List<Spell>())
            {
                if (userDict.TryGetValue(spell.Id, out var userSpell) && !userSpell.IsHomebrew)
                    result.Add(userSpell);
                else
                    result.Add(spell);
            }
            result.AddRange(userSpells.Where(s => s.IsHomebrew));

            return result;
        }

        private List<Item> UniteItems(List<Item> baseItems, List<Item> userItems)
        {
            var result = new List<Item>();
            var userDict = userItems.ToDictionary(i => i.Id, i => i);

            foreach (var item in baseItems ?? new List<Item>())
            {
                if (userDict.TryGetValue(item.Id, out var userItem) && !userItem.IsHomebrew)
                    result.Add(userItem);
                else
                    result.Add(item);
            }
            result.AddRange(userItems.Where(i => i.IsHomebrew));

            return result;
        }

        private List<Monster> UniteMonsters(List<Monster> baseMonsters, List<Monster> userMonsters)
        {
            var result = new List<Monster>();
            var userDict = userMonsters.ToDictionary(m => m.Id, m => m);

            foreach (var monster in baseMonsters ?? new List<Monster>())
            {
                if (userDict.TryGetValue(monster.Id, out var userMonster) && !userMonster.IsHomebrew)
                    result.Add(userMonster);
                else
                    result.Add(monster);
            }
            result.AddRange(userMonsters.Where(m => m.IsHomebrew));

            return result;
        }

        private List<CharacterRace> UniteRaces(List<CharacterRace> baseRaces, List<CharacterRace> userRaces)
        {
            var result = new List<CharacterRace>();
            var userDict = userRaces.ToDictionary(r => r.Id, r => r);

            foreach (var race in baseRaces ?? new List<CharacterRace>())
            {
                if (userDict.TryGetValue(race.Id, out var userRace) && !userRace.IsHomebrew)
                    result.Add(userRace);
                else
                    result.Add(race);
            }
            result.AddRange(userRaces.Where(r => r.IsHomebrew));

            return result;
        }

        private List<CharacterClass> UniteClasses(List<CharacterClass> baseClasses, List<CharacterClass> userClasses)
        {
            var result = new List<CharacterClass>();
            var userDict = userClasses.ToDictionary(c => c.Id, c => c);

            foreach (var cls in baseClasses ?? new List<CharacterClass>())
            {
                if (userDict.TryGetValue(cls.Id, out var userClass) && !userClass.IsHomebrew)
                    result.Add(userClass);
                else
                    result.Add(cls);
            }
            result.AddRange(userClasses.Where(c => c.IsHomebrew));

            return result;
        }

        public Task<List<Spell>> GetSpellsAsync() => _lazySpells.Value;

        public Spell GetSpellById(string id) => _baseSpells.FirstOrDefault(s => s.Id == id);

        public async Task AddSpellAsync(Spell spell)
        {
            spell.IsHomebrew = true;
            _userSpells.Add(spell);

            await SaveToPersistentAsync(SPELLS_KEY, _userSpells);

            _baseSpells = UniteSpells(_baseSpells, _userSpells);
        }

        public async Task UpdateSpellAsync(Spell spell)
        {
            var index = _userSpells.FindIndex(s => s.Id == spell.Id);

            if (index != -1)
                _userSpells[index] = spell;
            else
                _userSpells.Add(spell);

            await SaveToPersistentAsync(SPELLS_KEY, _userSpells);

            _baseSpells = UniteSpells(_baseSpells, _userSpells);
        }

        public async Task DeleteSpellAsync(string id)
        {
            _userSpells.RemoveAll(s => s.Id == id);
            await SaveToPersistentAsync(SPELLS_KEY, _userSpells);
            _baseSpells = UniteSpells(_baseSpells, _userSpells);
        }

        public Task<List<Item>> GetItemsAsync() => _lazyItems.Value;

        public Item GetItemById(string id) => _baseItems.FirstOrDefault(i => i.Id == id);

        public async Task AddItemAsync(Item item)
        {
            item.IsHomebrew = true;
            _userItems.Add(item);

            await SaveToPersistentAsync(ITEMS_KEY, _userItems);

            _baseItems = UniteItems(_baseItems, _userItems);
        }

        public async Task UpdateItemAsync(Item item)
        {
            var index = _userItems.FindIndex(i => i.Id == item.Id);

            if (index != -1)
                _userItems[index] = item;
            else
                _userItems.Add(item);

            await SaveToPersistentAsync(ITEMS_KEY, _userItems);

            _baseItems = UniteItems(_baseItems, _userItems);
        }

        public async Task DeleteItemAsync(string id)
        {
            _userItems.RemoveAll(i => i.Id == id);

            await SaveToPersistentAsync(ITEMS_KEY, _userItems);

            _baseItems = UniteItems(_baseItems, _userItems);
        }

        public Task<List<Monster>> GetMonstersAsync() => _lazyMonsters.Value;

        public Monster GetMonsterById(string id) => _baseMonsters.FirstOrDefault(m => m.Id == id);

        public async Task AddMonsterAsync(Monster monster)
        {
            monster.IsHomebrew = true;
            _userMonsters.Add(monster);
            await SaveToPersistentAsync(MONSTERS_KEY, _userMonsters);
            _baseMonsters = UniteMonsters(_baseMonsters, _userMonsters);
        }

        public async Task UpdateMonsterAsync(Monster monster)
        {
            var index = _userMonsters.FindIndex(m => m.Id == monster.Id);

            if (index != -1)
                _userMonsters[index] = monster;
            else
                _userMonsters.Add(monster);

            await SaveToPersistentAsync(MONSTERS_KEY, _userMonsters);

            _baseMonsters = UniteMonsters(_baseMonsters, _userMonsters);
        }

        public async Task DeleteMonsterAsync(string id)
        {
            _userMonsters.RemoveAll(m => m.Id == id);

            await SaveToPersistentAsync(MONSTERS_KEY, _userMonsters);

            _baseMonsters = UniteMonsters(_baseMonsters, _userMonsters);
        }

        public Task<List<CharacterRace>> GetRacesAsync() => _lazyRaces.Value;

        public CharacterRace GetRaceById(string id) => _baseRaces.FirstOrDefault(r => r.Id == id);

        public async Task AddRaceAsync(CharacterRace race)
        {
            race.IsHomebrew = true;
            _userRaces.Add(race);
            await SaveToPersistentAsync(RACES_KEY, _userRaces);
            _baseRaces = UniteRaces(_baseRaces, _userRaces);
            BuildClassDictionary();
            SyncSpellsWithClasses();
        }

        public async Task UpdateRaceAsync(CharacterRace race)
        {
            var index = _userRaces.FindIndex(r => r.Id == race.Id);
            if (index != -1) _userRaces[index] = race;
            else _userRaces.Add(race);
            await SaveToPersistentAsync(RACES_KEY, _userRaces);
            _baseRaces = UniteRaces(_baseRaces, _userRaces);
            BuildClassDictionary();
            SyncSpellsWithClasses();
        }

        public async Task DeleteRaceAsync(string id)
        {
            _userRaces.RemoveAll(r => r.Id == id);
            await SaveToPersistentAsync(RACES_KEY, _userRaces);
            _baseRaces = UniteRaces(_baseRaces, _userRaces);
            BuildClassDictionary();
            SyncSpellsWithClasses();
        }

        public Task<List<CharacterClass>> GetClassesAsync() => _lazyClasses.Value;

        public CharacterClass GetClassById(string id) => _baseClasses.FirstOrDefault(c => c.Id == id);

        public async Task AddClassAsync(CharacterClass characterClass)
        {
            characterClass.IsHomebrew = true;
            _userClasses.Add(characterClass);
            await SaveToPersistentAsync(CLASSES_KEY, _userClasses);
            _baseClasses = UniteClasses(_baseClasses, _userClasses);
            BuildClassDictionary();
            SyncSpellsWithClasses();
        }

        public async Task UpdateClassAsync(CharacterClass characterClass)
        {
            var index = _userClasses.FindIndex(c => c.Id == characterClass.Id);
            if (index != -1) _userClasses[index] = characterClass;
            else _userClasses.Add(characterClass);
            await SaveToPersistentAsync(CLASSES_KEY, _userClasses);
            _baseClasses = UniteClasses(_baseClasses, _userClasses);
            BuildClassDictionary();
            SyncSpellsWithClasses();
        }

        public async Task DeleteClassAsync(string id)
        {
            _userClasses.RemoveAll(c => c.Id == id);
            await SaveToPersistentAsync(CLASSES_KEY, _userClasses);
            _baseClasses = UniteClasses(_baseClasses, _userClasses);
            BuildClassDictionary();
            SyncSpellsWithClasses();
        }

        public Task<List<SkillData>> GetSkillsAsync() => _lazySkills.Value;

        public SkillData GetSkillById(string id) => _baseSkills.FirstOrDefault(s => s.Id == id);

        public List<SkillData> GetSkillsByAbility(CharacterAbility ability) => _baseSkills.FindAll(s => s.AssociatedAbility == ability);

        public async Task ToggleFavoriteAsync(string id, HandbookCategory category)
        {
            if (_favorites.Contains(id))
                _favorites.Remove(id);
            else
                _favorites.Add(id);

            await SaveToPersistentAsync(FAVORITES_KEY, _favorites);

            UpdateItemFavoriteStatus(id, category);
        }

        private void UpdateItemFavoriteStatus(string id, HandbookCategory category)
        {
            HandbookEntity item = category switch
            {
                HandbookCategory.Spells => _baseSpells.FirstOrDefault(x => x.Id == id),
                HandbookCategory.Items => _baseItems.FirstOrDefault(x => x.Id == id),
                HandbookCategory.Monsters => _baseMonsters.FirstOrDefault(x => x.Id == id),
                HandbookCategory.Races => _baseRaces.FirstOrDefault(x => x.Id == id),
                HandbookCategory.Classes => _baseClasses.FirstOrDefault(x => x.Id == id),
                _ => null
            };

            if (item != null)
                item.IsFavorite = _favorites.Contains(id);
        }

        public async Task<List<T>> GetFavoritesByCategoryAsync<T>(HandbookCategory category) where T : HandbookEntity
        {
            var items = category switch
            {
                HandbookCategory.Spells => (await GetSpellsAsync()).Cast<HandbookEntity>().ToList(),
                HandbookCategory.Items => (await GetItemsAsync()).Cast<HandbookEntity>().ToList(),
                HandbookCategory.Monsters => (await GetMonstersAsync()).Cast<HandbookEntity>().ToList(),
                HandbookCategory.Races => (await GetRacesAsync()).Cast<HandbookEntity>().ToList(),
                HandbookCategory.Classes => (await GetClassesAsync()).Cast<HandbookEntity>().ToList(),
                _ => new List<HandbookEntity>()
            };

            return items.Where(x => _favorites.Contains(x.Id)).Cast<T>().ToList();
        }

        public async Task SaveCharactersAsync(List<CharacterData> characters)
        {
            _userCharacters = characters;

            await SaveToPersistentAsync(CHARACTERS_KEY, _userCharacters);
        }

        public async Task<List<CharacterData>> LoadCharactersAsync()
        {
            return await Task.Run(() => LoadFromPersistent<List<CharacterData>>(CHARACTERS_KEY) ?? new List<CharacterData>());
        }

        public async Task DeleteCharacter(string characterId)
        {
            await LoadCharactersAsync();

            _userCharacters.RemoveAll(c => c.Id == characterId);
            await SaveToPersistentAsync(CHARACTERS_KEY, _userCharacters).ConfigureAwait(false);
        }

        public bool HasSavedCharacters() => File.Exists(Path.Combine(_persistentPath, $"{CHARACTERS_KEY}.json"));

        public async Task PreloadCategoryAsync(HandbookCategory category)
        {
            switch (category)
            {
                case HandbookCategory.Spells:
                    await GetSpellsAsync();
                    break;
                case HandbookCategory.Items:
                    await GetItemsAsync();
                    break;
                case HandbookCategory.Monsters:
                    await GetMonstersAsync();
                    break;
                case HandbookCategory.Races:
                    await GetRacesAsync();
                    break;
                case HandbookCategory.Classes:
                    await GetClassesAsync();
                    break;
            }
        }

        public async Task PreloadAllAsync()
        {
            await Task.WhenAll(
                GetSpellsAsync(),
                GetItemsAsync(),
                GetMonstersAsync(),
                GetRacesAsync(),
                GetClassesAsync(),
                GetSkillsAsync()
            );
        }
    }
}