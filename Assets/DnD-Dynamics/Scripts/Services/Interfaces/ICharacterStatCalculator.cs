using UnityEngine;

namespace DnD_Dynamics.Services
{
    public interface ICharacterStatCalculator
    {
        CharacterStats CalculateTotalStats(CharacterData character, CharacterRace race, CharacterClass @class);
        int CalculateMaxHp(CharacterData character, CharacterClass @class);
        int GetProficiencyBonus(int level);
        int GetModifier(int abilityScore);
        int CalculateProficiencyBonus(int level);
        int CalculateInitiativeBonus(CharacterData character, CharacterStats totalStats);
        int CalculateSpellSaveDC(CharacterData character, CharacterStats totalStats);
        int CalculateSpellAttackBonus(CharacterData character, CharacterStats totalStats);
    }
}