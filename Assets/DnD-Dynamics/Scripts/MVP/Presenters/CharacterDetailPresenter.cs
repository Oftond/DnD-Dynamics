using DnD_Dynamics.MVP.Model;
using DnD_Dynamics.MVP.View;
using DnD_Dynamics.Services;
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
        _view = view;
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

    public CharacterUIData GetSelectedCharacter() => _selectedCharacter;

    public async Task ApplyDamageAsync(int amount)
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

        await _model.ApplyDamageAsync(_selectedCharacter.Id, amount);
        _view?.ShowSuccess($"Нанесено {amount} урона");
        UpdateSelectedCharacter();
    }

    public async Task ApplyHealAsync(int amount)
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

        await _model.ApplyHealAsync(_selectedCharacter.Id, amount);
        _view?.ShowSuccess($"Восстановлено {amount} HP");

        UpdateSelectedCharacter();
    }

    public async Task LevelUpAsync()
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
        await _model.LevelUpAsync(_selectedCharacter.Id);
        UpdateSelectedCharacter();
        _view?.ShowSuccess($"Персонаж повышен с {oldLevel} до {_selectedCharacter?.Level} уровня!");
    }

    public async Task DeleteCharacterAsync()
    {
        if (_selectedCharacter == null)
        {
            _view?.ShowError("Персонаж не выбран");
            return;
        }

        await _model.DeleteCharacterAsync(_selectedCharacter.Id);
        _selectedCharacter = null;
        _view?.ClearSelection();

        _view?.ShowSuccess("Персонаж удален");
    }

    private void UpdateSelectedCharacter()
    {
        if (_selectedCharacter != null)
        {
            _selectedCharacter = _model.GetCharacter(_selectedCharacter.Id);
            _view?.DisplayCharacterDetails(_selectedCharacter);
        }
    }

    private void OnCharacterUpdated(CharacterUIData character)
    {
        if (_selectedCharacter?.Id == character.Id)
        {
            _selectedCharacter = character;
            _view?.DisplayCharacterDetails(character);
        }
    }

    public void Dispose()
    {
        if (_model != null)
            _model.OnCharacterUpdated -= OnCharacterUpdated;
    }
}