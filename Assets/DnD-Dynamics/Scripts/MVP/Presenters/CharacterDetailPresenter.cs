using DnD_Dynamics.MVP.Model;
using DnD_Dynamics.MVP.View;
using DnD_Dynamics.Services;
using System;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class CharacterDetailPresenter : BaseCharacterPresenter
{
    private ICharacterDetailView _view;
    private CharacterUIData _selectedCharacter;

    [Inject]
    public CharacterDetailPresenter(CharacterModel model, IDataService dataService) : base(model, dataService)
    {
        _model.OnCharacterUpdated += OnCharacterUpdated;
    }

    public void SetView(ICharacterDetailView view)
    {
        _view = view ?? throw new ArgumentNullException(nameof(view));
    }

    public void SelectCharacter(string characterId)
    {
        if (string.IsNullOrEmpty(characterId))
        {
            _view?.ShowError("ID персонажа пуст");
            return;
        }

        _selectedCharacter = _model.GetCharacter(characterId);

        if (_selectedCharacter != null)
            _view?.DisplayCharacterDetails(_selectedCharacter);
        else
            _view?.ShowError("Персонаж не найден");
    }

    public CharacterUIData GetSelectedCharacter() => _selectedCharacter;

    public async Task ApplyDamageAsync(int amount)
    {
        if (!ValidateCharacterSelection())
            return;

        if (amount <= 0)
        {
            _view?.ShowError("Урон должен быть > 0");
            return;
        }

        await _model.ApplyDamageAsync(_selectedCharacter.Id, amount);
        _view?.ShowSuccess($"Нанесено {amount} урона");
    }

    public async Task ApplyHealAsync(int amount)
    {
        if (!ValidateCharacterSelection())
            return;

        if (amount <= 0)
        {
            _view?.ShowError("Лечение должно быть > 0"); 
            return;
        }

        await _model.ApplyHealAsync(_selectedCharacter.Id, amount);
        _view?.ShowSuccess($"Восстановлено {amount} HP");
    }

    public async Task LevelUpAsync()
    {
        if (!ValidateCharacterSelection())
            return;

        if (_selectedCharacter.Level >= 20)
        {
            _view?.ShowError("Достигнут максимальный уровень");
            return;
        }

        var oldLevel = _selectedCharacter.Level;
        await _model.LevelUpAsync(_selectedCharacter.Id);
        _view?.ShowSuccess($"Персонаж повышен с {oldLevel} до {oldLevel + 1} уровня!");
    }

    public async Task DeleteCharacterAsync()
    {
        if (!ValidateCharacterSelection()) return;

        await _model.DeleteCharacterAsync(_selectedCharacter.Id);
        _selectedCharacter = null;
        _view?.ClearSelection();
        _view?.ShowSuccess("Персонаж удален");
    }

    private bool ValidateCharacterSelection()
    {
        if (_selectedCharacter == null)
        {
            _view?.ShowError("Персонаж не выбран");
            return false;
        }
        return true;
    }

    private void OnCharacterUpdated(CharacterUIData character)
    {
        if (_selectedCharacter != null && _selectedCharacter.Id == character.Id)
        {
            _selectedCharacter = character;
            _view?.DisplayCharacterDetails(character);
        }
    }

    public async Task UpdatePortraitPathAsync(string path)
    {
        if (!ValidateCharacterSelection()) return;

        await _model.UpdatePortraitPathAsync(_selectedCharacter.Id, path);
    }

    public void Dispose()
    {
        if (_model != null)
            _model.OnCharacterUpdated -= OnCharacterUpdated;
    }
}