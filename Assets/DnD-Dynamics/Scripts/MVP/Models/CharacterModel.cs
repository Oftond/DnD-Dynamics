using DnD_Dynamics.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;


namespace DnD_Dynamics.MVP.Model
{
    public class CharacterModel
    {
        private readonly IDataService _dataService;
        private readonly ICharacterStatCalculator _statCalculator;
        private readonly ICharacterCombatService _combatService;
        private readonly ICharacterUiMapper _uiMapper;

        private readonly List<CharacterData> _characters = new();
        private readonly Dictionary<string, CharacterRace> _races = new();
        private readonly Dictionary<string, CharacterClass> _classes = new();
        private bool _isLoaded;

        public event Action<List<CharacterUIData>> OnCharactersChanged;
        public event Action<CharacterUIData> OnCharacterUpdated;

        [Inject]
        public CharacterModel(IDataService dataService, ICharacterStatCalculator statCalculator, ICharacterCombatService combatService, ICharacterUiMapper uiMapper)
        {
            _dataService = dataService;
            _statCalculator = statCalculator;
            _combatService = combatService;
            _uiMapper = uiMapper;
        }

        public async Task LoadCharactersAsync()
        {
            if (_isLoaded) return;

            var rawCharacters = await _dataService.GetCharactersAsync();
            _characters.Clear();
            _characters.AddRange(rawCharacters ?? new List<CharacterData>());

            var races = await _dataService.GetRacesAsync();
            var classes = await _dataService.GetClassesAsync();
            _races.Clear();
            _classes.Clear();
            foreach (var r in races) _races[r.Id] = r;
            foreach (var c in classes) _classes[c.Id] = c;

            _isLoaded = true;
            NotifyCharactersChanged();
        }

        public async Task SaveAllAsync() => await _dataService.SaveCharactersAsync(_characters);

        public List<CharacterUIData> GetAllCharacters() => _characters.Select(MapToUIData).ToList();

        public CharacterUIData GetCharacter(string id)
        {
            var character = _characters.FirstOrDefault(c => c.Id == id);
            return character != null ? MapToUIData(character) : null;
        }

        public CharacterData GetRawCharacter(string id) => _characters.FirstOrDefault(c => c.Id == id);

        public async Task<CharacterData> CreateCharacterAsync(string name, CharacterRace race, CharacterClass characterClass, CharacterStats stats)
        {
            var character = new CharacterData
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                RaceId = race?.Id ?? string.Empty,
                ClassId = characterClass?.Id ?? string.Empty,
                BaseStats = stats,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CurrentHp = 0,
                ArmorClass = 10
            };

            var maxHp = _statCalculator.CalculateMaxHp(character, characterClass);
            character.CurrentHp = maxHp;
            character.ArmorClass = 10 + stats.GetModifier(CharacterAbility.Dexterity);

            _characters.Add(character);
            await SaveAllAsync();
            NotifyCharactersChanged();
            return character;
        }

        public async Task UpdateCharacterAsync(CharacterData character)
        {
            var index = _characters.FindIndex(c => c.Id == character.Id);
            if (index >= 0)
            {
                character.UpdatedAt = DateTime.Now;
                _characters[index] = character;
                await SaveAllAsync();
                NotifyCharacterUpdated(character.Id);
                NotifyCharactersChanged();
            }
        }

        public async Task ApplyDamageAsync(string characterId, int amount)
        {
            var character = GetRawCharacter(characterId);
            if (character != null)
            {
                _combatService.ApplyDamage(character, amount);
                await UpdateCharacterAsync(character);
            }
        }

        public async Task ApplyHealAsync(string characterId, int amount)
        {
            var character = GetRawCharacter(characterId);
            if (character != null)
            {
                var charClass = _classes.TryGetValue(character.ClassId, out var cls) ? cls : null;
                _combatService.ApplyHeal(character, amount, charClass);
                await UpdateCharacterAsync(character);
            }
        }

        public async Task LevelUpAsync(string characterId)
        {
            var character = GetRawCharacter(characterId);
            if (character != null && character.Level < 20)
            {
                character.Level++;
                character.UpdatedAt = DateTime.Now;

                await UpdateCharacterAsync(character);
            }
        }

        public async Task DeleteCharacterAsync(string characterId)
        {
            _characters.RemoveAll(c => c.Id == characterId);
            await SaveAllAsync();
            NotifyCharactersChanged();
        }

        private CharacterUIData MapToUIData(CharacterData character)
        {
            _races.TryGetValue(character.RaceId, out var race);
            _classes.TryGetValue(character.ClassId, out var @class);
            var totalStats = _statCalculator.CalculateTotalStats(character, race, @class);
            var maxHp = _statCalculator.CalculateMaxHp(character, @class);
            return _uiMapper.MapToUi(character, totalStats, maxHp, race, @class);
        }

        private void NotifyCharactersChanged() => OnCharactersChanged?.Invoke(GetAllCharacters());
        private void NotifyCharacterUpdated(string characterId)
        {
            var uiData = GetCharacter(characterId);
            if (uiData != null) OnCharacterUpdated?.Invoke(uiData);
        }
    }
}