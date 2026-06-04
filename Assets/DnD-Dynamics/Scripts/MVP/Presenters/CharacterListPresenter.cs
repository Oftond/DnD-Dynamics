using DnD_Dynamics.MVP.Model;
using DnD_Dynamics.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class CharacterListPresenter : BaseCharacterPresenter
{
    private ICharacterListView _view;

    [Inject]
    public CharacterListPresenter(CharacterModel model, IDataService dataService) : base(model, dataService)
    {
        _model.OnCharactersChanged += OnCharactersChanged;
    }

    public void SetView(ICharacterListView view)
    {
        _view = view ?? throw new ArgumentNullException(nameof(view));
    }

    public List<CharacterUIData> GetAllCharacters() => _model.GetAllCharacters();

    public async Task LoadCharactersAsync()
    {
        if (_view == null)
            return;

        _view.ShowLoading(true);
        try
        {
            await _model.LoadCharactersAsync();
        }
        catch (Exception ex)
        {
            _view.ShowError("Не удалось загрузить персонажей. Проверьте файлы сохранений.");
        }
        finally
        {
            _view.ShowLoading(false);
        }
    }

    public void SelectCharacter(string characterId)
    {
        if (string.IsNullOrEmpty(characterId) || _view == null)
            return;

        var character = _model.GetCharacter(characterId);

        if (character != null)
            _view.DisplayCharacterDetails(character);
        else
            _view.ShowError("Персонаж не найден");
    }

    private void OnCharactersChanged(List<CharacterUIData> characters) => _view?.DisplayCharacters(characters);

    public void Dispose()
    {
        if (_model != null)
            _model.OnCharactersChanged -= OnCharactersChanged;
    }
}