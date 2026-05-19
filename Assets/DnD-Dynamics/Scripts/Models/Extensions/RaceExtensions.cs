using System.Diagnostics;
using UnityEngine;

public static class RaceExtensions
{
    public static string GetDisplayName(this CharacterRace race)
    {
        if (race == null) return "Неизвестно";
        return race.Name;
    }

    public static int GetAbilityBonus(this CharacterRace race, CharacterAbility ability)
    {
        if (race?.AbilityBonuses == null) return 0;

        string key = ability.ToString();
        return race.AbilityBonuses.ContainsKey(key) ? race.AbilityBonuses[key] : 0;
    }
}