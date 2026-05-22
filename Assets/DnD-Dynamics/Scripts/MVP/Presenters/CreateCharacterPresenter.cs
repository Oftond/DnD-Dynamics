using DnD_Dynamics.Models;
using DnD_Dynamics.MVP.Model;
using DnD_Dynamics.MVP.View;
using DnD_Dynamics.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace DnD_Dynamics.MVP.Presenter
{
    public class CreateCharacterPresenter : BaseCharacterPresenter
    {
        private ICreateCharacterView _view;

        private List<CharacterRace> _races;
        private List<CharacterClass> _classes;
        private bool _isRacesLoading;
        private bool _isClassesLoading;

        [Inject]
        public CreateCharacterPresenter(CharacterModel model, IDataService dataService) : base(model, dataService) { }

        public void SetView(ICreateCharacterView view)
        {
            _view = view;
        }

        public async Task LoadRacesAsync()
        {
            if (_races != null)
            {
                UpdateRacesView();
                return;
            }

            if (_isRacesLoading) return;

            _isRacesLoading = true;
            _view?.ShowLoading(true);

            _races = await _dataService.GetRacesAsync();
            _isRacesLoading = false;
            _view?.ShowLoading(false);

            UpdateRacesView();
        }

        public async Task LoadClassesAsync()
        {
            if (_classes != null)
            {
                UpdateClassesView();
                return;
            }

            if (_isClassesLoading) return;

            _isClassesLoading = true;
            _view?.ShowLoading(true);

            _classes = await _dataService.GetClassesAsync();
            _isClassesLoading = false;
            _view?.ShowLoading(false);

            UpdateClassesView();
        }

        private void UpdateRacesView()
        {
            var raceIds = new List<string>();
            foreach (var race in _races)
                raceIds.Add(race.Id);

            _view?.SetRaces(_races, raceIds);
        }

        private void UpdateClassesView()
        {
            var classIds = new List<string>();
            foreach (var cls in _classes)
                classIds.Add(cls.Id);

            _view?.SetClasses(_classes, classIds);
        }

        public async Task<CharacterData> CreateCharacterAsync(string name, string raceId, string classId, int strength, int dexterity, int constitution, int intelligence, int wisdom, int charisma)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length < 2)
            {
                _view?.ShowError("Имя должно содержать минимум 2 символа");
                return null;
            }

            var race = _dataService.GetRaceById(raceId);
            if (race == null)
            {
                _view?.ShowError("Не выбрана раса");
                return null;
            }

            var characterClass = _dataService.GetClassById(classId);
            if (characterClass == null)
            {
                _view?.ShowError("Не выбран класс");
                return null;
            }

            var stats = new CharacterStats
            {
                Strength = Math.Clamp(strength, Constants.MIN_ABILITY_SCORE, Constants.MAX_ABILITY_SCORE),
                Dexterity = Math.Clamp(dexterity, Constants.MIN_ABILITY_SCORE, Constants.MAX_ABILITY_SCORE),
                Constitution = Math.Clamp(constitution, Constants.MIN_ABILITY_SCORE, Constants.MAX_ABILITY_SCORE),
                Intelligence = Math.Clamp(intelligence, Constants.MIN_ABILITY_SCORE, Constants.MAX_ABILITY_SCORE),
                Wisdom = Math.Clamp(wisdom, Constants.MIN_ABILITY_SCORE, Constants.MAX_ABILITY_SCORE),
                Charisma = Math.Clamp(charisma, Constants.MIN_ABILITY_SCORE, Constants.MAX_ABILITY_SCORE)
            };

            var character = await _model.CreateCharacterAsync(name, race, characterClass, stats);
            _view?.ShowSuccess($"Персонаж {name} создан!");

            return character;
        }
    }
}