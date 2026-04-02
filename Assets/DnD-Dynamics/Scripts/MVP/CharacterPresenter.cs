using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPresenter
{
    private readonly CharacterModel _model;
    private ICharacterView _view;
    private CharacterUIData _selectedCharacter;

    public CharacterPresenter(CharacterModel model)
    {
        _model = model;

        _model.OnCharactersChanged += OnCharactersChanged;
        _model.OnCharacterUpdated += OnCharacterUpdated;
    }

    public List<CharacterUIData> GetAllCharacters()
    {
        return _model.GetAllCharacters();
    }

    public void SetView(ICharacterView view)
    {
        _view = view;
        RefreshCharacters();
    }

    public void RefreshCharacters()
    {
        var characters = _model.GetAllCharacters();
        _view?.DisplayCharacters(characters);
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

    public void CreateCharacter(string name, CharacterRace race, CharacterClass characterClass,
        int strength, int dexterity, int constitution, int intelligence, int wisdom, int charisma)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length < 2)
            {
                _view?.ShowError("Имя должно содержать минимум 2 символа");
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

            SelectCharacter(character.Id);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Ошибка создания персонажа: {ex.Message}");
            _view?.ShowError("Ошибка при создании персонажа");
        }
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
        _view?.ShowSuccess($"Персонаж повышен с {oldLevel} до {oldLevel + 1} уровня!");
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

    private void OnCharactersChanged(List<CharacterUIData> characters)
    {
        _view?.DisplayCharacters(characters);
    }

    private void OnCharacterUpdated(CharacterUIData character)
    {
        if (_selectedCharacter?.Id == character.Id)
        {
            _selectedCharacter = character;
            _view?.DisplayCharacterDetails(character);
        }
        RefreshCharacters();
    }

    public void UpdateCharacter(CharacterUIData updatedCharacter)
    {
        var character = _model.GetRawCharacter(updatedCharacter.Id);
        if (character != null)
        {
            character.Name = updatedCharacter.Name;
            character.Notes = updatedCharacter.Notes;
            character.Backstory = updatedCharacter.Backstory;
            character.Gold = updatedCharacter.Gold;
            character.Silver = updatedCharacter.Silver;
            character.Copper = updatedCharacter.Copper;

            _model.UpdateCharacter(character);
            _view?.ShowSuccess($"Персонаж {character.Name} обновлен");
        }
    }

    public CharacterUIData GetSelectedCharacter()
    {
        return _selectedCharacter;
    }

    public void ClearSelectedCharacter()
    {
        _selectedCharacter = null;
        _view?.ClearSelection();
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