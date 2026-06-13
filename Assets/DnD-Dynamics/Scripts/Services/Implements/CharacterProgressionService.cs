using System;
using UnityEngine;

public class CharacterProgressionService : ICharacterProgressionService
{
    public event Action<(CharacterData character, int oldLevel, int newLevel)> OnLevelUp;

    public bool TryAddExperience(CharacterData character, int amount)
    {
        if (character == null)
        {
            Debug.LogError("[CharacterProgressionService] Персонаж null");
            return false;
        }

        character.ExperiencePoints += Math.Max(0, amount);
        bool leveledUp = false;

        var expForNext = ExperienceTable.GetExperienceForLevel(character.Level + 1);
        while (character.ExperiencePoints >= expForNext && character.Level < 20)
        {
            character.Level++;
            character.UpdatedAt = DateTime.Now;
            OnLevelUp?.Invoke((character, character.Level - 1, character.Level));
            leveledUp = true;
            expForNext = ExperienceTable.GetExperienceForLevel(character.Level + 1);
        }

        return leveledUp;
    }

    public bool CanLevelUp(CharacterData character)
    {
        if (character == null)
            return false;

        return character.ExperiencePoints >= ExperienceTable.GetExperienceForLevel(character.Level + 1) && character.Level < 20;
    }
}