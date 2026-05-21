using DnD_Dynamics.Models.Combat;
using DnD_Dynamics.Services.Interfaces;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using Random = System.Random;

namespace DnD_Dynamics.Services
{
    public class CombatService : ICombatService
    {
        private readonly string _savePath;
        private CombatSession _currentSession;
        private readonly Random _random = new Random();

        private const string COMBAT_SAVE_KEY = "combat_session";

        public CombatService()
        {
            _savePath = Path.Combine(Application.persistentDataPath, "GameData", $"{COMBAT_SAVE_KEY}.json");
            LoadSessionAsync().ConfigureAwait(false);
        }

        private async Task LoadSessionAsync()
        {
            if (File.Exists(_savePath))
            {
                var json = await File.ReadAllTextAsync(_savePath);
                _currentSession = JsonConvert.DeserializeObject<CombatSession>(json);
            }

            if (_currentSession == null)
                _currentSession = new CombatSession();
        }

        private async Task SaveSessionAsync()
        {
            var json = JsonConvert.SerializeObject(_currentSession, Formatting.Indented);

            await File.WriteAllTextAsync(_savePath, json);
        }

        public Task<CombatSession> GetCurrentSessionAsync() => Task.FromResult(_currentSession);

        public Task SaveSessionAsync(CombatSession session)
        {
            _currentSession = session;

            return SaveSessionAsync();
        }

        public Task ClearSessionAsync()
        {
            _currentSession = new CombatSession();

            return SaveSessionAsync();
        }

        public Task<bool> HasActiveSession() => Task.FromResult(_currentSession.Combatants.Count > 0);

        public async Task AddCombatantAsync(Combatant combatant)
        {
            _currentSession.AddCombatant(combatant);

            await SaveSessionAsync();
        }

        public async Task UpdateCombatantAsync(Combatant combatant)
        {
            var existing = _currentSession.GetCombatant(combatant.Id);
            if (existing != null)
            {
                existing.Name = combatant.Name;
                existing.Initiative = combatant.Initiative;
                existing.CurrentHp = combatant.CurrentHp;
                existing.MaxHp = combatant.MaxHp;
                existing.ArmorClass = combatant.ArmorClass;
                await SaveSessionAsync();
            }
        }

        public async Task RemoveCombatantAsync(string id)
        {
            _currentSession.RemoveCombatant(id);

            await SaveSessionAsync();
        }

        public async Task ApplyDamageToCombatantAsync(string id, int amount)
        {
            var combatant = _currentSession.GetCombatant(id);

            if (combatant != null)
            {
                combatant.ApplyDamage(amount);

                await SaveSessionAsync();
            }
        }

        public async Task ApplyHealToCombatantAsync(string id, int amount)
        {
            var combatant = _currentSession.GetCombatant(id);
            if (combatant != null)
            {
                combatant.ApplyHeal(amount);

                await SaveSessionAsync();
            }
        }

        public async Task RollInitiativeForCombatantAsync(string id, int bonus = 0)
        {
            var combatant = _currentSession.GetCombatant(id);

            if (combatant != null)
            {
                combatant.Initiative = _random.Next(1, 21) + bonus;
                _currentSession.SortByInitiative();

                await SaveSessionAsync();
            }
        }

        public async Task RollInitiativeForAllAsync(int bonus = 0)
        {
            foreach (var combatant in _currentSession.Combatants)
            {
                combatant.Initiative = _random.Next(1, 21) + bonus;
            }
            _currentSession.SortByInitiative();
            await SaveSessionAsync();
        }

        public async Task NextTurnAsync()
        {
            _currentSession.NextTurn();
            await SaveSessionAsync();
        }

        public async Task PreviousTurnAsync()
        {
            _currentSession.PreviousTurn();
            await SaveSessionAsync();
        }
    }
}