using DnD_Dynamics.Models;
using DnD_Dynamics.MVP.Model;
using DnD_Dynamics.MVP.View;
using DnD_Dynamics.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Zenject;

namespace DnD_Dynamics.MVP.Presenter
{
    public class CharacterPresenter
    {
        private readonly CharacterModel _model;
        private readonly IHandbookDataService _handbookDataService;
        private ICharacterView _view;
        private CharacterUIData _selectedCharacter;

        [Inject]
        public CharacterPresenter(CharacterModel model, IHandbookDataService handbookDataService)
        {
            _model = model;
            _handbookDataService = handbookDataService;

            _model.OnCharactersChanged += OnCharactersChanged;
            _model.OnCharacterUpdated += OnCharacterUpdated;
        }

        public void SetView(ICharacterView view)
        {
            _view = view;
            RefreshCharacters();
        }

        public List<CharacterUIData> GetAllCharacters()
        {
            return _model.GetAllCharacters();
        }

        public void RefreshCharacters()
        {
            var characters = _model.GetAllCharacters();
            _view?.DisplayCharacters(characters);
        }

        public void CreateCharacter(string name, CharacterRace race, CharacterClass characterClass, int strength, int dexterity, int constitution, int intelligence, int wisdom, int charisma)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length < 2)
            {
                _view?.ShowError("Имя должно содержать минимум 2 символа");
                return;
            }

            if (race == null)
            {
                _view?.ShowError("Не выбрана раса");
                return;
            }

            if (characterClass == null)
            {
                _view?.ShowError("Не выбран класс");
                return;
            }

            var stats = new CharacterStats
            {
                Strength = Math.Clamp(strength, 3, 20),
                Dexterity = Math.Clamp(dexterity, 3, 20),
                Constitution = Math.Clamp(constitution, 3, 20),
                Intelligence = Math.Clamp(intelligence, 3, 20),
                Wisdom = Math.Clamp(wisdom, 3, 20),
                Charisma = Math.Clamp(charisma, 3, 20)
            };

            var character = _model.CreateCharacter(name, race, characterClass, stats);
            _view?.ShowSuccess($"Персонаж {name} создан!");
            RefreshCharacters();
        }

        public void SelectCharacter(string characterId)
        {
            _selectedCharacter = _model.GetCharacter(characterId);
            if (_selectedCharacter != null)
            {
                _view?.DisplayCharacterDetails(_selectedCharacter);
            }
            else
            {
                _view?.ShowError("Персонаж не найден");
            }
        }

        public CharacterUIData GetSelectedCharacter()
        {
            return _selectedCharacter;
        }

        public void ApplyDamage(int amount)
        {
            if (_selectedCharacter == null)
            {
                _view?.ShowError("Персонаж не выбран");
                return;
            }

            if (amount <= 0)
            {
                _view?.ShowError("Урон должен быть положительным числом");
                return;
            }

            _model.ApplyDamage(_selectedCharacter.Id, amount);
            _view?.ShowSuccess($"Нанесено {amount} урона");
        }

        public void ApplyHeal(int amount)
        {
            if (_selectedCharacter == null)
            {
                _view?.ShowError("Персонаж не выбран");
                return;
            }

            if (amount <= 0)
            {
                _view?.ShowError("Лечение должно быть положительным числом");
                return;
            }

            _model.ApplyHeal(_selectedCharacter.Id, amount);
            _view?.ShowSuccess($"Восстановлено {amount} HP");
        }

        public void LevelUp()
        {
            if (_selectedCharacter == null)
            {
                _view?.ShowError("Персонаж не выбран");
                return;
            }

            if (_selectedCharacter.Level >= 20)
            {
                _view?.ShowError("Достигнут максимальный уровень");
                return;
            }

            var oldLevel = _selectedCharacter.Level;
            _model.LevelUp(_selectedCharacter.Id);

            _selectedCharacter = _model.GetCharacter(_selectedCharacter.Id);
            _view?.ShowSuccess($"Персонаж повышен с {oldLevel} до {_selectedCharacter?.Level} уровня!");
            RefreshCharacters();
        }

        public void DeleteCharacter()
        {
            if (_selectedCharacter == null)
            {
                _view?.ShowError("Персонаж не выбран");
                return;
            }

            _model.DeleteCharacter(_selectedCharacter.Id);
            _selectedCharacter = null;
            _view?.ClearSelection();
            _view?.ShowSuccess("Персонаж удален");
            RefreshCharacters();
        }

        public CharacterRace GetRaceById(string id) => _handbookDataService.GetRaceById(id);

        public CharacterClass GetClassById(string id) => _handbookDataService.GetClassById(id);

        public List<CharacterRace> GetAllRaces() => _handbookDataService.GetAllRaces();

        public List<CharacterClass> GetAllClasses() => _handbookDataService.GetAllClasses();

        public List<Spell> GetAllSpells() => _handbookDataService.GetAllSpells();

        public Spell GetSpellById(string id) => _handbookDataService.GetSpellById(id);

        public List<Item> GetAllItems() => _handbookDataService.GetAllItems();

        public Item GetItemById(string id) => _handbookDataService.GetItemById(id);

        public List<Monster> GetAllMonsters() => _handbookDataService.GetAllMonsters();

        public Monster GetMonsterById(string id) => _handbookDataService.GetMonsterById(id);

        private void OnCharactersChanged(List<CharacterUIData> characters) => _view?.DisplayCharacters(characters);

        private void OnCharacterUpdated(CharacterUIData character)
        {
            if (_selectedCharacter?.Id == character.Id)
            {
                _selectedCharacter = character;
                _view?.DisplayCharacterDetails(character);
            }
            RefreshCharacters();
        }

        public void Dispose()
        {
            if (_model != null)
            {
                _model.OnCharactersChanged -= OnCharactersChanged;
                _model.OnCharacterUpdated -= OnCharacterUpdated;
            }
        }
    }
}