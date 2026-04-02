using System;
using System.Collections.Generic;

[Serializable]
public class SerializableCharacterData
{
    public string Id;
    public string Name;
    public int Level;
    public int ExperiencePoints;

    public CharacterRace Race;
    public CharacterClass Class;

    public CharacterStats BaseStats;
    public CharacterStats BonusStats;

    public int CurrentHp;
    public int TemporaryHp;
    public int ArmorClass;

    public int Gold;
    public int Silver;
    public int Copper;

    public string PortraitPath;
    public string Backstory;
    public string Notes;

    public string CreatedAt;
    public string UpdatedAt;

    public static SerializableCharacterData FromCharacter(CharacterData character)
    {
        return new SerializableCharacterData
        {
            Id = character.Id,
            Name = character.Name,
            Level = character.Level,
            ExperiencePoints = character.ExperiencePoints,
            Race = character.Race,
            Class = character.Class,
            BaseStats = character.BaseStats,
            BonusStats = character.BonusStats,
            CurrentHp = character.CurrentHp,
            TemporaryHp = character.TemporaryHp,
            ArmorClass = character.ArmorClass,
            Gold = character.Gold,
            Silver = character.Silver,
            Copper = character.Copper,
            PortraitPath = character.PortraitPath,
            Backstory = character.Backstory,
            Notes = character.Notes,
            CreatedAt = character.CreatedAt.ToString("O"),
            UpdatedAt = character.UpdatedAt.ToString("O")
        };
    }

    public CharacterData ToCharacter()
    {
        return new CharacterData
        {
            Id = Id,
            Name = Name,
            Level = Level,
            ExperiencePoints = ExperiencePoints,
            Race = Race,
            Class = Class,
            BaseStats = BaseStats,
            BonusStats = BonusStats,
            CurrentHp = CurrentHp,
            TemporaryHp = TemporaryHp,
            ArmorClass = ArmorClass,
            Gold = Gold,
            Silver = Silver,
            Copper = Copper,
            PortraitPath = PortraitPath,
            Backstory = Backstory,
            Notes = Notes,
            CreatedAt = DateTime.Parse(CreatedAt),
            UpdatedAt = DateTime.Parse(UpdatedAt)
        };
    }
}