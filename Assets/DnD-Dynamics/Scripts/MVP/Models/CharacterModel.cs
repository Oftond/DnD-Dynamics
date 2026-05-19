using DnD_Dynamics.Models;
using DnD_Dynamics.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Zenject;

namespace DnD_Dynamics.MVP.Model
{
    public class CharacterModel
    {
        private readonly IDataService _dataService;
        private readonly IHandbookDataService _handbookDataService;
        private List<CharacterData> _characters;

        public event Action<List<CharacterUIData>> OnCharactersChanged;
        public event Action<CharacterUIData> OnCharacterUpdated;

        [Inject]
        public CharacterModel(IDataService dataService, IHandbookDataService handbookDataService)
        {
            _dataService = dataService;
            _handbookDataService = handbookDataService;

            LoadCharacters();
        }

        public void LoadCharacters()
        {
            _characters = _dataService.LoadCharacters();
            if (_characters == null)
                _characters = new List<CharacterData>();

            foreach (var character in _characters)
                character.InitializeSpellbook(_handbookDataService);

            NotifyCharactersChanged();
        }

        private void SaveAll()
        {
            _dataService.SaveCharacters(_characters);
        }

        public List<CharacterUIData> GetAllCharacters() => _characters.Select(c => c.GetUIData()).ToList();

        public CharacterUIData GetCharacter(string id)
        {
            var character = _characters.FirstOrDefault(c => c.Id == id);
            return character?.GetUIData();
        }

        public CharacterData GetRawCharacter(string id)
        {
            return _characters.FirstOrDefault(c => c.Id == id);
        }

        public CharacterData CreateCharacter(string name, CharacterRace race, CharacterClass characterClass, CharacterStats stats)
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

            character.InitializeSpellbook(_handbookDataService);

            _characters.Add(character);
            SaveAll();
            NotifyCharactersChanged();

            return character;
        }

        public void UpdateCharacter(CharacterData character)
        {
            var index = _characters.FindIndex(c => c.Id == character.Id);

            if (index >= 0)
            {
                character.UpdatedAt = DateTime.Now;
                _characters[index] = character;
                SaveAll();
                NotifyCharacterUpdated(character.Id);
                NotifyCharactersChanged();
            }
        }

        public void ApplyDamage(string characterId, int amount)
        {
            var character = GetRawCharacter(characterId);
            if (character != null)
            {
                character.ApplyDamage(amount);
                UpdateCharacter(character);
            }
        }

        public void ApplyHeal(string characterId, int amount)
        {
            var character = GetRawCharacter(characterId);
            if (character != null)
            {
                character.ApplyHeal(amount);
                UpdateCharacter(character);
            }
        }

        public void LevelUp(string characterId)
        {
            var character = GetRawCharacter(characterId);
            if (character != null && character.Level < 20)
            {
                character.LevelUp();
                UpdateCharacter(character);
            }
        }

        public void DeleteCharacter(string characterId)
        {
            _characters.RemoveAll(c => c.Id == characterId);
            SaveAll();
            NotifyCharactersChanged();
        }

        private void NotifyCharactersChanged()
        {
            OnCharactersChanged?.Invoke(GetAllCharacters());
        }

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