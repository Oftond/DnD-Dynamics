using DnD_Dynamics.MVP.Model;
using DnD_Dynamics.Services;
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
        _view = view;
    }

    public List<CharacterUIData> GetAllCharacters() => _model.GetAllCharacters();

    public async Task LoadCharactersAsync()
    {
        _view?.ShowLoading(true);
        await _model.LoadCharactersAsync();
        _view?.ShowLoading(false);

        _view?.DisplayCharacters(_model.GetAllCharacters());
    }

    public void SelectCharacter(string characterId)
    {
        var character = _model.GetCharacter(characterId);

        if (character != null)
            _view?.DisplayCharacterDetails(character);
        else
            _view?.ShowError("Персонаж не найден");
    }

    private void OnCharactersChanged(List<CharacterUIData> characters) => _view?.DisplayCharacters(characters);

    public void Dispose()
    {
        if (_model != null)
            _model.OnCharactersChanged -= OnCharactersChanged;
    }
}