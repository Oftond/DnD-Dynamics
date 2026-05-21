using DnD_Dynamics.MVP.Model;
using DnD_Dynamics.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class BaseCharacterPresenter
{
    protected readonly CharacterModel _model;
    protected readonly IDataService _dataService;

    protected BaseCharacterPresenter(CharacterModel model, IDataService dataService)
    {
        _model = model;
        _dataService = dataService;
    }

    public CharacterRace GetRaceById(string id) => _dataService.GetRaceById(id);

    public CharacterClass GetClassById(string id) => _dataService.GetClassById(id);

    public async Task<List<CharacterRace>> GetRacesAsync() => await _dataService.GetRacesAsync();

    public async Task<List<CharacterClass>> GetClassesAsync() => await _dataService.GetClassesAsync();
}