using DnD_Dynamics.Services;
using System.Diagnostics;
using UnityEngine;

public class CharacterStatCalculator : ICharacterStatCalculator
{
    public CharacterStats CalculateTotalStats(CharacterData character, CharacterRace race, CharacterClass @class)
    {
        var total = character.BaseStats.Clone();

        if (race != null)
        {
            total.Strength += race.GetAbilityBonus(CharacterAbility.Strength);
            total.Dexterity += race.GetAbilityBonus(CharacterAbility.Dexterity);
            total.Constitution += race.GetAbilityBonus(CharacterAbility.Constitution);
            total.Intelligence += race.GetAbilityBonus(CharacterAbility.Intelligence);
            total.Wisdom += race.GetAbilityBonus(CharacterAbility.Wisdom);
            total.Charisma += race.GetAbilityBonus(CharacterAbility.Charisma);
        }

        total.Add(character.BonusStats);

        total.Clamp(Constants.MIN_ABILITY_SCORE, Constants.MAX_ABILITY_SCORE);

        return total;
    }

    public int CalculateMaxHp(CharacterData character, CharacterClass @class)
    {
        if (@class == null) return 10;

        var conModifier = GetModifier(character.BaseStats.Constitution);
        var hitDice = @class.GetHitDice();

        if (character.Level == 1)
            return hitDice + conModifier;

        var averageHp = (hitDice / 2) + 1;
        return hitDice + conModifier + (character.Level - 1) * (averageHp + conModifier);
    }

    public int GetProficiencyBonus(int level) => level switch
    {
        <= 4 => 2,
        <= 8 => 3,
        <= 12 => 4,
        <= 16 => 5,
        _ => 6
    };

    public int GetModifier(int abilityScore) => (abilityScore - 10) / 2;

    public int CalculateProficiencyBonus(int level) => level switch
    {
        <= 4 => 2,
        <= 8 => 3,
        <= 12 => 4,
        <= 16 => 5,
        _ => 6
    };

    public int CalculateInitiativeBonus(CharacterData character, CharacterStats totalStats) => (totalStats.Dexterity - 10) / 2;

    public int CalculateSpellSaveDC(CharacterData character, CharacterStats totalStats)
    {
        var prof = CalculateProficiencyBonus(character.Level);
        var mod = totalStats.GetModifier(character.SpellcastingAbility);

        return 8 + prof + mod;
    }

    public int CalculateSpellAttackBonus(CharacterData character, CharacterStats totalStats)
    {
        var prof = CalculateProficiencyBonus(character.Level);
        var mod = totalStats.GetModifier(character.SpellcastingAbility);

        return prof + mod;
    }
}