using DnD_Dynamics.Models;
using DnD_Dynamics.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace DnD_Dynamics.MVP.Model
{
    public class CharacterModel
    {
        private readonly IDataService _dataService;
        private List<CharacterData> _characters = new();

        public event Action<List<CharacterUIData>> OnCharactersChanged;
        public event Action<CharacterUIData> OnCharacterUpdated;

        [Inject]
        public CharacterModel(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task LoadCharactersAsync()
        {
            _characters = await _dataService.LoadCharactersAsync();

            if (_characters == null)
                _characters = new List<CharacterData>();

            foreach (var character in _characters)
                character.InitializeSpellbook(_dataService);

            NotifyCharactersChanged();
        }

        public async Task SaveAllAsync() => await _dataService.SaveCharactersAsync(_characters);

        public List<CharacterUIData> GetAllCharacters() => _characters.Select(c => c.GetUIData()).ToList();

        public CharacterUIData GetCharacter(string id)
        {
            var character = _characters.FirstOrDefault(c => c.Id == id);

            return character?.GetUIData();
        }

        public CharacterData GetRawCharacter(string id) => _characters.FirstOrDefault(c => c.Id == id);

        public async Task<CharacterData> CreateCharacterAsync(string name, CharacterRace race, CharacterClass characterClass, CharacterStats stats)
        {
            var character = new CharacterData
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                Race = race,
                Class = characterClass,
                BaseStats = stats,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            character.CurrentHp = character.MaxHp;
            character.ArmorClass = 10 + character.TotalStats.GetModifier(CharacterAbility.Dexterity);

            character.InitializeSpellbook(_dataService);

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
                character.ApplyDamage(amount);

                await UpdateCharacterAsync(character);
            }
        }

        public async Task ApplyHealAsync(string characterId, int amount)
        {
            var character = GetRawCharacter(characterId);

            if (character != null)
            {
                character.ApplyHeal(amount);
                await UpdateCharacterAsync(character);
            }
        }

        public async Task LevelUpAsync(string characterId)
        {
            var character = GetRawCharacter(characterId);

            if (character != null && character.Level < 20)
            {
                character.LevelUp();
                await UpdateCharacterAsync(character);
            }
        }

        public async Task DeleteCharacterAsync(string characterId)
        {
            _characters.RemoveAll(c => c.Id == characterId);

            await SaveAllAsync();

            NotifyCharactersChanged();
        }

        private void NotifyCharactersChanged() => OnCharactersChanged?.Invoke(GetAllCharacters());

        private void NotifyCharacterUpdated(string characterId)
        {
            var character = GetCharacter(characterId);

            if (character != null)
            {
                OnCharacterUpdated?.Invoke(character);
            }
        }
    }
}