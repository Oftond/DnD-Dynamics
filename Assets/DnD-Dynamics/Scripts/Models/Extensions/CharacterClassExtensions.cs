using System.Collections.Generic;
using UnityEngine;

public static class CharacterClassExtensions
{
    public static string GetDisplayName(this CharacterClass characterClass)
    {
        if (characterClass == null) return "Неизвестно";
        return characterClass.Name;
    }

    public static int GetHitDice(this CharacterClass characterClass)
    {
        return characterClass?.HitDice ?? 8;
    }

    public static CharacterAbility GetPrimaryAbility(this CharacterClass characterClass)
    {
        if (characterClass == null) return CharacterAbility.Strength;

        return characterClass.PrimaryAbility switch
        {
            "Strength" => CharacterAbility.Strength,
            "Dexterity" => CharacterAbility.Dexterity,
            "Constitution" => CharacterAbility.Constitution,
            "Intelligence" => CharacterAbility.Intelligence,
            "Wisdom" => CharacterAbility.Wisdom,
            "Charisma" => CharacterAbility.Charisma,
            _ => CharacterAbility.Strength
        };
    }

    public static List<string> GetSavingThrows(this CharacterClass characterClass)
    {
        return characterClass?.SavingThrows ?? new List<string>();
    }
}