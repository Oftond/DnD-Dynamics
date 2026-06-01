using System;
using UnityEngine;

public class CharacterProgressionService : ICharacterProgressionService
{
    public event Action<CharacterData, int> OnLevelUp;

    public bool AddExperience(CharacterData character, int amount)
    {
        character.ExperiencePoints += amount;
        bool leveledUp = false;

        var expForNext = CalculateExpForLevel(character.Level + 1);
        while (character.ExperiencePoints >= expForNext && character.Level < 20)
        {
            character.Level++;
            character.UpdatedAt = DateTime.Now;
            OnLevelUp?.Invoke(character, character.Level);
            leveledUp = true;
            expForNext = CalculateExpForLevel(character.Level + 1);
        }

        return leveledUp;
    }

    public bool CanLevelUp(CharacterData character) => character.ExperiencePoints >= CalculateExpForLevel(character.Level + 1) && character.Level < 20;

    private int CalculateExpForLevel(int level)
    {
        return level switch
        {
            2 => 300,
            3 => 900,
            4 => 2700,
            5 => 6500,
            6 => 14000,
            7 => 23000,
            8 => 34000,
            9 => 48000,
            10 => 64000,
            11 => 85000,
            12 => 100000,
            13 => 120000,
            14 => 140000,
            15 => 165000,
            16 => 195000,
            17 => 225000,
            18 => 265000,
            19 => 305000,
            20 => 355000,
            _ => int.MaxValue
        };
    }
}