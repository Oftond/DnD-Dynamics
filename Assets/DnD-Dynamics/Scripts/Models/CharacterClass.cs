using System;

namespace DnDCharacterManager.Core.Models.Enums
{
    [Serializable]
    public enum CharacterClass
    {
        Barbarian = 1,
        Bard = 2,
        Cleric = 3,
        Druid = 4,
        Fighter = 5,
        Monk = 6,
        Paladin = 7,
        Ranger = 8,
        Rogue = 9,
        Sorcerer = 10,
        Warlock = 11,
        Wizard = 12
    }

    public static class CharacterClassExtensions
    {
        public static string GetDisplayName(this CharacterClass characterClass)
        {
            return characterClass switch
            {
                CharacterClass.Barbarian => "Варвар",
                CharacterClass.Bard => "Бард",
                CharacterClass.Cleric => "Жрец",
                CharacterClass.Druid => "Друид",
                CharacterClass.Fighter => "Воин",
                CharacterClass.Monk => "Монах",
                CharacterClass.Paladin => "Паладин",
                CharacterClass.Ranger => "Следопыт",
                CharacterClass.Rogue => "Плут",
                CharacterClass.Sorcerer => "Чародей",
                CharacterClass.Warlock => "Колдун",
                CharacterClass.Wizard => "Волшебник",
                _ => "Неизвестно"
            };
        }

        public static int GetHitDice(this CharacterClass characterClass)
        {
            return characterClass switch
            {
                CharacterClass.Barbarian => 12,
                CharacterClass.Fighter => 10,
                CharacterClass.Paladin => 10,
                CharacterClass.Ranger => 10,
                CharacterClass.Bard => 8,
                CharacterClass.Cleric => 8,
                CharacterClass.Druid => 8,
                CharacterClass.Monk => 8,
                CharacterClass.Rogue => 8,
                CharacterClass.Warlock => 8,
                CharacterClass.Sorcerer => 6,
                CharacterClass.Wizard => 6,
                _ => 8
            };
        }

        public static CharacterAbility GetPrimaryAbility(this CharacterClass characterClass)
        {
            return characterClass switch
            {
                CharacterClass.Barbarian => CharacterAbility.Strength,
                CharacterClass.Bard => CharacterAbility.Charisma,
                CharacterClass.Cleric => CharacterAbility.Wisdom,
                CharacterClass.Druid => CharacterAbility.Wisdom,
                CharacterClass.Fighter => CharacterAbility.Strength,
                CharacterClass.Monk => CharacterAbility.Dexterity,
                CharacterClass.Paladin => CharacterAbility.Strength,
                CharacterClass.Ranger => CharacterAbility.Dexterity,
                CharacterClass.Rogue => CharacterAbility.Dexterity,
                CharacterClass.Sorcerer => CharacterAbility.Charisma,
                CharacterClass.Warlock => CharacterAbility.Charisma,
                CharacterClass.Wizard => CharacterAbility.Intelligence,
                _ => CharacterAbility.Strength
            };
        }
    }

    public enum CharacterAbility
    {
        Strength,
        Dexterity,
        Constitution,
        Intelligence,
        Wisdom,
        Charisma
    }
}