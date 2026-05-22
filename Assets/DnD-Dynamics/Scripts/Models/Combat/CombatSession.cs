using System;
using System.Collections.Generic;
using System.Linq;

namespace DnD_Dynamics.Models.Combat
{
    [Serializable]
    public class CombatSession
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int CurrentRound { get; set; } = 1;

        public int CurrentTurnIndex { get; set; } = 0;

        public List<Combatant> Combatants { get; set; } = new List<Combatant>();

        public Combatant CurrentCombatant => Combatants.Count > CurrentTurnIndex ? Combatants[CurrentTurnIndex] : null;

        public void SortByInitiative()
        {
            Combatants = Combatants.OrderByDescending(c => c.Initiative).ToList();
            CurrentTurnIndex = 0;
        }

        public void NextTurn()
        {
            if (Combatants.Count == 0) return;

            CurrentTurnIndex++;
            if (CurrentTurnIndex >= Combatants.Count)
            {
                CurrentTurnIndex = 0;
                CurrentRound++;
            }
        }

        public void PreviousTurn()
        {
            if (Combatants.Count == 0) return;

            CurrentTurnIndex--;
            if (CurrentTurnIndex < 0)
            {
                CurrentTurnIndex = Combatants.Count - 1;
                if (CurrentRound > 1) CurrentRound--;
            }
        }

        public void AddCombatant(Combatant combatant)
        {
            Combatants.Add(combatant);

            SortByInitiative();
        }

        public void RemoveCombatant(string id)
        {
            Combatants.RemoveAll(c => c.Id == id);

            SortByInitiative();
        }

        public Combatant GetCombatant(string id) => Combatants.FirstOrDefault(c => c.Id == id);

        public void Clear()
        {
            Combatants.Clear();
            CurrentRound = 1;
            CurrentTurnIndex = 0;
        }
    }
}