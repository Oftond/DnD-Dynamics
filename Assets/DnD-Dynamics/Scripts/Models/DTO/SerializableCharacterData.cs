using DnD_Dynamics.Models;
using System;
using System.Collections.Generic;

[Serializable]
public class SerializableCharacterData
{
    public string Id;
    public string Name;
    public int Level;
    public int ExperiencePoints;
    public string RaceId;
    public string ClassId;
    public CharacterStats BaseStats;
    public CharacterStats BonusStats;
    public int CurrentHp;
    public int TemporaryHp;
    public int ArmorClass;
    public int ShieldBonus;
    public bool IsShieldActive;
    public bool HasInspiration;
    public string PortraitPath;
    public string Backstory;
    public string Notes;
    public string CreatedAt;
    public string UpdatedAt;
    public List<SkillSaveData> SavedSkills = new();
    public SpellbookSaveData SavedSpellbook = new();
    public SerializableInventory SerializableInventory = new();

    public static SerializableCharacterData FromCharacter(CharacterData character)
    {
        return new SerializableCharacterData
        {
            Id = character.Id,
            Name = character.Name,
            Level = character.Level,
            ExperiencePoints = character.ExperiencePoints,
            RaceId = character.RaceId,
            ClassId = character.ClassId,
            BaseStats = character.BaseStats,
            BonusStats = character.BonusStats,
            CurrentHp = character.CurrentHp,
            TemporaryHp = character.TemporaryHp,
            ArmorClass = character.ArmorClass,
            ShieldBonus = character.ShieldBonus,
            IsShieldActive = character.IsShieldActive,
            HasInspiration = character.HasInspiration,
            PortraitPath = character.PortraitPath,
            Backstory = character.Backstory,
            Notes = character.Notes,
            CreatedAt = character.CreatedAt.ToString("O"),
            UpdatedAt = character.UpdatedAt.ToString("O"),
            SavedSkills = new List<SkillSaveData>(character.SavedSkills),
            SavedSpellbook = new SpellbookSaveData
            {
                KnownSpellIds = new List<string>(character.SavedSpellbook.KnownSpellIds),
                PreparedSpellIds = new List<string>(character.SavedSpellbook.PreparedSpellIds),
                UsedSlotsByLevel = new Dictionary<int, int>(character.SavedSpellbook.UsedSlotsByLevel)
            },
            SerializableInventory = character.SerializableInventory
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
            RaceId = RaceId,
            ClassId = ClassId,
            BaseStats = BaseStats,
            BonusStats = BonusStats,
            CurrentHp = CurrentHp,
            TemporaryHp = TemporaryHp,
            ArmorClass = ArmorClass,
            ShieldBonus = ShieldBonus,
            IsShieldActive = IsShieldActive,
            HasInspiration = HasInspiration,
            PortraitPath = PortraitPath,
            Backstory = Backstory,
            Notes = Notes,
            CreatedAt = DateTime.Parse(CreatedAt),
            UpdatedAt = DateTime.Parse(UpdatedAt),
            SavedSkills = SavedSkills,
            SavedSpellbook = SavedSpellbook,
            SerializableInventory = SerializableInventory
        };
    }
}