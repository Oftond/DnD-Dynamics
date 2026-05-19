using DnD_Dynamics.MVP.Presenters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace DnD_Dynamics.Services
{
    public class DataService : IDataService
    {
        private readonly string _persistentPath;

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

        private bool _isUserDataLoaded = false;
        private bool _isBaseDataLoaded = false;

        private const string SPELLS_KEY = "spells";
        private const string ITEMS_KEY = "items";
        private const string MONSTERS_KEY = "monsters";
        private const string RACES_KEY = "races";
        private const string CLASSES_KEY = "classes";
        private const string CHARACTERS_KEY = "characters";
        private const string FAVORITES_KEY = "favorites";

        public DataService()
        {
            _persistentPath = Path.Combine(Application.persistentDataPath, "GameData");

            if (!Directory.Exists(_persistentPath))
                Directory.CreateDirectory(_persistentPath);

            LoadAllBaseData();
            LoadAllUserData();
        }

        private void LoadAllBaseData()
        {
            if (_isBaseDataLoaded) return;

            _baseSpells = LoadFromResources<List<Spell>>("Data/spells") ?? new();
            _baseItems = LoadFromResources<List<Item>>("Data/items") ?? new();
            _baseMonsters = LoadFromResources<List<Monster>>("Data/monsters") ?? new();
            _baseRaces = LoadFromResources<List<CharacterRace>>("Data/races") ?? new();
            _baseClasses = LoadFromResources<List<CharacterClass>>("Data/classes") ?? new();
            _baseSkills = LoadFromResources<List<SkillData>>("Data/skills") ?? new();

            Debug.Log($"Базовые данные: {_baseSpells.Count} заклинаний, {_baseItems.Count} предметов, {_baseMonsters.Count} монстров, {_baseRaces.Count} рас, {_baseClasses.Count} классов, {_baseSkills.Count} навыков");

            BuildClassDictionary();
            SyncSpellsWithClasses();

            _isBaseDataLoaded = true;
        }

        private void LoadAllUserData()
        {
            if (_isUserDataLoaded) return;

            _userSpells = LoadFromPersistent<List<Spell>>(SPELLS_KEY) ?? new();
            _userItems = LoadFromPersistent<List<Item>>(ITEMS_KEY) ?? new();
            _userMonsters = LoadFromPersistent<List<Monster>>(MONSTERS_KEY) ?? new();
            _userRaces = LoadFromPersistent<List<CharacterRace>>(RACES_KEY) ?? new();
            _userClasses = LoadFromPersistent<List<CharacterClass>>(CLASSES_KEY) ?? new();
            _userCharacters = LoadFromPersistent<List<CharacterData>>(CHARACTERS_KEY) ?? new();
            _favorites = LoadFromPersistent<HashSet<string>>(FAVORITES_KEY) ?? new();

            Debug.Log($"Пользовательские данные: {_userSpells.Count} заклинаний, {_userItems.Count} предметов, {_userMonsters.Count} монстров, {_userRaces.Count} рас, {_userClasses.Count} классов, {_userCharacters.Count} персонажей, {_favorites.Count} избранных");

            _isUserDataLoaded = true;
        }

        private T LoadFromResources<T>(string path) where T : class
        {
            try
            {
                TextAsset asset = Resources.Load<TextAsset>(path);

                if (asset != null)
                {
                    return JsonConvert.DeserializeObject<T>(asset.text);
                }
                Debug.LogWarning($"Файл {path}.json не найден в Resources");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Ошибка загрузки {path}: {ex.Message}");
            }
            return null;
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

        private void SaveToPersistent<T>(string key, T data)
        {
            try
            {
                string filePath = Path.Combine(_persistentPath, $"{key}.json");
                string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText(filePath, json);
                Debug.Log($"Сохранено {key}: {filePath}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Ошибка сохранения {key}: {ex.Message}");
            }
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

        private List<Spell> GetAllSpellsCombined()
        {
            var result = new List<Spell>();
            var userSpellDict = _userSpells.ToDictionary(s => s.Id, s => s);

            foreach (var spell in _baseSpells)
            {
                if (userSpellDict.TryGetValue(spell.Id, out var userSpell) && !userSpell.IsHomebrew)
                    result.Add(userSpell);
                else
                    result.Add(spell);
            }
            result.AddRange(_userSpells.Where(s => s.IsHomebrew));
            return result;
        }

        private List<Item> GetAllItemsCombined()
        {
            var result = new List<Item>();
            var userItemDict = _userItems.ToDictionary(i => i.Id, i => i);

            foreach (var item in _baseItems)
            {
                if (userItemDict.TryGetValue(item.Id, out var userItem) && !userItem.IsHomebrew)
                    result.Add(userItem);
                else
                    result.Add(item);
            }
            result.AddRange(_userItems.Where(i => i.IsHomebrew));
            return result;
        }

        private List<Monster> GetAllMonstersCombined()
        {
            var result = new List<Monster>();
            var userMonsterDict = _userMonsters.ToDictionary(m => m.Id, m => m);

            foreach (var monster in _baseMonsters)
            {
                if (userMonsterDict.TryGetValue(monster.Id, out var userMonster) && !userMonster.IsHomebrew)
                    result.Add(userMonster);
                else
                    result.Add(monster);
            }
            result.AddRange(_userMonsters.Where(m => m.IsHomebrew));
            return result;
        }

        private List<CharacterRace> GetAllRacesCombined()
        {
            var result = new List<CharacterRace>();
            var userRaceDict = _userRaces.ToDictionary(r => r.Id, r => r);

            foreach (var race in _baseRaces)
            {
                if (userRaceDict.TryGetValue(race.Id, out var userRace) && !userRace.IsHomebrew)
                    result.Add(userRace);
                else
                    result.Add(race);
            }
            result.AddRange(_userRaces.Where(r => r.IsHomebrew));
            return result;
        }

        private List<CharacterClass> GetAllClassesCombined()
        {
            var result = new List<CharacterClass>();
            var userClassDict = _userClasses.ToDictionary(c => c.Id, c => c);

            foreach (var cls in _baseClasses)
            {
                if (userClassDict.TryGetValue(cls.Id, out var userClass) && !userClass.IsHomebrew)
                    result.Add(userClass);
                else
                    result.Add(cls);
            }
            result.AddRange(_userClasses.Where(c => c.IsHomebrew));
            return result;
        }

        public List<Spell> GetAllSpells() => GetAllSpellsCombined();

        public Spell GetSpellById(string id) => GetAllSpellsCombined().FirstOrDefault(s => s.Id == id);

        public void AddSpell(Spell spell)
        {
            spell.IsHomebrew = true;
            _userSpells.Add(spell);
            SaveToPersistent(SPELLS_KEY, _userSpells);
        }

        public void UpdateSpell(Spell spell)
        {
            var index = _userSpells.FindIndex(s => s.Id == spell.Id);
            if (index != -1) _userSpells[index] = spell;
            else _userSpells.Add(spell);
            SaveToPersistent(SPELLS_KEY, _userSpells);
        }

        public void DeleteSpell(string id)
        {
            _userSpells.RemoveAll(s => s.Id == id);
            SaveToPersistent(SPELLS_KEY, _userSpells);
        }

        public List<Item> GetAllItems() => GetAllItemsCombined();

        public Item GetItemById(string id) => GetAllItemsCombined().FirstOrDefault(i => i.Id == id);

        public void AddItem(Item item)
        {
            item.IsHomebrew = true;
            _userItems.Add(item);
            SaveToPersistent(ITEMS_KEY, _userItems);
        }

        public void UpdateItem(Item item)
        {
            var index = _userItems.FindIndex(i => i.Id == item.Id);
            if (index != -1) _userItems[index] = item;
            else _userItems.Add(item);
            SaveToPersistent(ITEMS_KEY, _userItems);
        }

        public void DeleteItem(string id)
        {
            _userItems.RemoveAll(i => i.Id == id);
            SaveToPersistent(ITEMS_KEY, _userItems);
        }

        public List<Monster> GetAllMonsters() => GetAllMonstersCombined();

        public Monster GetMonsterById(string id) => GetAllMonstersCombined().FirstOrDefault(m => m.Id == id);

        public void AddMonster(Monster monster)
        {
            monster.IsHomebrew = true;
            _userMonsters.Add(monster);
            SaveToPersistent(MONSTERS_KEY, _userMonsters);
        }

        public void UpdateMonster(Monster monster)
        {
            var index = _userMonsters.FindIndex(m => m.Id == monster.Id);
            if (index != -1) _userMonsters[index] = monster;
            else _userMonsters.Add(monster);
            SaveToPersistent(MONSTERS_KEY, _userMonsters);
        }

        public void DeleteMonster(string id)
        {
            _userMonsters.RemoveAll(m => m.Id == id);
            SaveToPersistent(MONSTERS_KEY, _userMonsters);
        }

        public List<CharacterRace> GetAllRaces() => GetAllRacesCombined();

        public CharacterRace GetRaceById(string id) => GetAllRacesCombined().FirstOrDefault(r => r.Id == id);

        public void AddRace(CharacterRace race)
        {
            race.IsHomebrew = true;
            _userRaces.Add(race);
            SaveToPersistent(RACES_KEY, _userRaces);
            BuildClassDictionary();
            SyncSpellsWithClasses();
        }

        public void UpdateRace(CharacterRace race)
        {
            var index = _userRaces.FindIndex(r => r.Id == race.Id);
            if (index != -1) _userRaces[index] = race;
            else _userRaces.Add(race);
            SaveToPersistent(RACES_KEY, _userRaces);
            BuildClassDictionary();
            SyncSpellsWithClasses();
        }

        public void DeleteRace(string id)
        {
            _userRaces.RemoveAll(r => r.Id == id);
            SaveToPersistent(RACES_KEY, _userRaces);
            BuildClassDictionary();
            SyncSpellsWithClasses();
        }

        public List<CharacterClass> GetAllClasses() => GetAllClassesCombined();

        public CharacterClass GetClassById(string id) => GetAllClassesCombined().FirstOrDefault(c => c.Id == id);

        public void AddClass(CharacterClass characterClass)
        {
            characterClass.IsHomebrew = true;
            _userClasses.Add(characterClass);
            SaveToPersistent(CLASSES_KEY, _userClasses);
            BuildClassDictionary();
            SyncSpellsWithClasses();
        }

        public void UpdateClass(CharacterClass characterClass)
        {
            var index = _userClasses.FindIndex(c => c.Id == characterClass.Id);
            if (index != -1) _userClasses[index] = characterClass;
            else _userClasses.Add(characterClass);
            SaveToPersistent(CLASSES_KEY, _userClasses);
            BuildClassDictionary();
            SyncSpellsWithClasses();
        }

        public void DeleteClass(string id)
        {
            _userClasses.RemoveAll(c => c.Id == id);
            SaveToPersistent(CLASSES_KEY, _userClasses);
            BuildClassDictionary();
            SyncSpellsWithClasses();
        }

        public List<SkillData> GetAllSkills() => _baseSkills;

        public SkillData GetSkillById(string id) => _baseSkills.FirstOrDefault(s => s.Id == id);

        public List<SkillData> GetSkillsByAbility(CharacterAbility ability)
        {
            return _baseSkills.FindAll(s => s.AssociatedAbility == ability);
        }

        public void ToggleFavorite(string id, HandbookCategory category)
        {
            if (_favorites.Contains(id))
                _favorites.Remove(id);
            else
                _favorites.Add(id);

            SaveToPersistent(FAVORITES_KEY, _favorites);

            switch (category)
            {
                case HandbookCategory.Spells:
                    var spell = GetSpellById(id);
                    if (spell != null) spell.IsFavorite = _favorites.Contains(id);
                    break;
                case HandbookCategory.Items:
                    var item = GetItemById(id);
                    if (item != null) item.IsFavorite = _favorites.Contains(id);
                    break;
                case HandbookCategory.Monsters:
                    var monster = GetMonsterById(id);
                    if (monster != null) monster.IsFavorite = _favorites.Contains(id);
                    break;
                case HandbookCategory.Races:
                    var race = GetRaceById(id);
                    if (race != null) race.IsFavorite = _favorites.Contains(id);
                    break;
                case HandbookCategory.Classes:
                    var cls = GetClassById(id);
                    if (cls != null) cls.IsFavorite = _favorites.Contains(id);
                    break;
            }
        }

        public List<T> GetFavoritesByCategory<T>(HandbookCategory category) where T : HandbookEntity
        {
            var allItems = category switch
            {
                HandbookCategory.Spells => GetAllSpells().Cast<HandbookEntity>().ToList(),
                HandbookCategory.Items => GetAllItems().Cast<HandbookEntity>().ToList(),
                HandbookCategory.Monsters => GetAllMonsters().Cast<HandbookEntity>().ToList(),
                HandbookCategory.Races => GetAllRaces().Cast<HandbookEntity>().ToList(),
                HandbookCategory.Classes => GetAllClasses().Cast<HandbookEntity>().ToList(),
                _ => new List<HandbookEntity>()
            };

            return allItems.Where(x => _favorites.Contains(x.Id)).Cast<T>().ToList();
        }

        public void SaveCharacters(List<CharacterData> characters)
        {
            _userCharacters = characters;
            SaveToPersistent(CHARACTERS_KEY, _userCharacters);
        }

        public List<CharacterData> LoadCharacters()
        {
            if (_userCharacters == null)
                _userCharacters = LoadFromPersistent<List<CharacterData>>(CHARACTERS_KEY) ?? new List<CharacterData>();
            return _userCharacters;
        }

        public void SaveCharacter(CharacterData character)
        {
            LoadCharacters();
            var existingIndex = _userCharacters.FindIndex(c => c.Id == character.Id);
            if (existingIndex >= 0)
                _userCharacters[existingIndex] = character;
            else
                _userCharacters.Add(character);
            SaveToPersistent(CHARACTERS_KEY, _userCharacters);
        }

        public void DeleteCharacter(string characterId)
        {
            LoadCharacters();
            _userCharacters.RemoveAll(c => c.Id == characterId);
            SaveToPersistent(CHARACTERS_KEY, _userCharacters);
        }

        public bool HasSavedCharacters() => File.Exists(Path.Combine(_persistentPath, $"{CHARACTERS_KEY}.json"));
    }
}