using UnityEngine;

public interface ICharacterUiMapper
{
    CharacterUIData MapToUi(CharacterData character, CharacterStats totalStats, int maxHp, CharacterRace race = null, CharacterClass @class = null);
}