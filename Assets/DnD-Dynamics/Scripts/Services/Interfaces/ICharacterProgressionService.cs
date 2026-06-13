using System;
using UnityEngine;

public interface ICharacterProgressionService
{
    event Action<(CharacterData character, int oldLevel, int newLevel)> OnLevelUp;

    bool TryAddExperience(CharacterData character, int amount);
    bool CanLevelUp(CharacterData character);
}