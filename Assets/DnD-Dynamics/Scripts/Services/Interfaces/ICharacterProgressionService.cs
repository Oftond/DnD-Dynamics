using System;
using UnityEngine;

public interface ICharacterProgressionService
{
    event Action<CharacterData, int> OnLevelUp;

    bool AddExperience(CharacterData character, int amount);
    bool CanLevelUp(CharacterData character);
}