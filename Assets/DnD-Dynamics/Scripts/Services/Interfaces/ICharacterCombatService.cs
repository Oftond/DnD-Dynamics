using System;
using UnityEngine;

namespace DnD_Dynamics.Services
{
    public interface ICharacterCombatService
    {
        event Action<CharacterData, int> OnDamageApplied;
        event Action<CharacterData, int> OnHealApplied;

        int ApplyDamage(CharacterData character, int amount);
        int ApplyHeal(CharacterData character, int amount, CharacterClass characterClass);
    }
}