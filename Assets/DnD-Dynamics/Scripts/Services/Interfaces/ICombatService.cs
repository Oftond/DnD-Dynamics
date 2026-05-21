using DnD_Dynamics.Models.Combat;
using System.Threading.Tasks;

namespace DnD_Dynamics.Services.Interfaces
{
    public interface ICombatService
    {
        Task<CombatSession> GetCurrentSessionAsync();

        Task SaveSessionAsync(CombatSession session);

        Task ClearSessionAsync();

        Task<bool> HasActiveSession();

        Task AddCombatantAsync(Combatant combatant);

        Task UpdateCombatantAsync(Combatant combatant);

        Task RemoveCombatantAsync(string id);

        Task ApplyDamageToCombatantAsync(string id, int amount);

        Task ApplyHealToCombatantAsync(string id, int amount);

        Task RollInitiativeForCombatantAsync(string id, int bonus = 0);

        Task RollInitiativeForAllAsync(int bonus = 0);

        Task NextTurnAsync();

        Task PreviousTurnAsync();
    }
}