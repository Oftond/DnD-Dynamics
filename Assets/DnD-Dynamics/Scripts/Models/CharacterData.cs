using DnD_Dynamics.Models;
using DnD_Dynamics.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using Zenject;
using Debug = UnityEngine.Debug;

public enum CharacterAbility
{
    Strength,
    Dexterity,
    Constitution,
    Intelligence,
    Wisdom,
    Charisma
}

[Serializable]
public class SkillSaveData
{
    public string SkillId;
    public bool IsProficient;
    public bool IsExpert;
    public int CustomModifier;
}

[Serializable]
public class SpellbookSaveData
{
    public List<string> KnownSpellIds { get; set; } = new();
    public List<string> PreparedSpellIds { get; set; } = new();
    public Dictionary<int, int> UsedSlotsByLevel { get; set; } = new();
}

[Serializable]
public class CharacterData
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public int Level { get; set; } = 1;
    public int ExperiencePoints { get; set; } = 0;

    public string RaceId { get; set; } = string.Empty;
    public string ClassId { get; set; } = string.Empty;

    public CharacterStats BaseStats { get; set; } = new CharacterStats();

    public CharacterStats BonusStats { get; set; } = new CharacterStats
    {
        Strength = 0,
        Dexterity = 0,
        Constitution = 0,
        Intelligence = 0,
        Wisdom = 0,
        Charisma = 0
    };

    public int CurrentHp { get; set; }
    public int TemporaryHp { get; set; }
    public int ArmorClass { get; set; } = 10;
    public int ShieldBonus { get; set; } = 0;
    public bool IsShieldActive { get; set; } = false;
    public int ArmorClassBonus { get; set; } = 0;

    public int TotalArmorClass => ArmorClass + ArmorClassBonus + (IsShieldActive ? ShieldBonus : 0);

    public int SpeedBonus { get; set; } = 0;

    public int InitiativeBonus { get; set; } = 0;

    public bool HasInspiration { get; set; } = false;

    public string PortraitPath { get; set; } = string.Empty;
    public string Backstory { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public List<SkillSaveData> SavedSkills { get; set; } = new();
    public SpellbookSaveData SavedSpellbook { get; set; } = new();
    public CharacterAbility SpellcastingAbility { get; set; } = CharacterAbility.Intelligence;
    public SerializableInventory SerializableInventory { get; set; } = new SerializableInventory();

    public int ProficiencyBonus => Level switch
    {
        <= 4 => 2,
        <= 8 => 3,
        <= 12 => 4,
        <= 16 => 5,
        _ => 6
    };
}

[Serializable]
public class CharacterUIData
{
    public string Id;
    public string Name;
    public string ClassName;
    public string RaceName;

    public int CurrentHp;
    public int MaxHp;

    public int Level;
    public int ExperiencePoints;
    public int ExperienceToNextLevel;

    public int BaseArmorClass;
    public int ArmorClassBonus;
    public int ShieldBonus;
    public int TotalArmorClass;
    public bool IsShieldActive;

    public int BaseSpeed;
    public int SpeedBonus;
    public int TotalSpeed;

    public int InitiativeBase;
    public int InitiativeBonus;
    public int TotalInitiative;

    public bool HasInspiration;

    public int Strength;
    public int Dexterity;
    public int Constitution;
    public int Intelligence;
    public int Wisdom;
    public int Charisma;

    public int StrengthModifier;
    public int DexterityModifier;
    public int ConstitutionModifier;
    public int IntelligenceModifier;
    public int WisdomModifier;
    public int CharismaModifier;
    public int ProficiencyBonus;

    public int Gold;
    public int Silver;
    public int Copper;

    public string PortraitPath;
    public string Backstory;
    public string Notes;

    public string ClassRaceText => $"{ClassName} - {RaceName}";
    public string HpText => $"{CurrentHp} / {MaxHp}";

    public string LevelText => $"Ур. {Level}";
    public string ExperienceText => $"{ExperiencePoints} / {ExperienceToNextLevel}";

    public string SpeedText => $"{TotalSpeed} фт.";
    public string SpeedBonusText => SpeedBonus != 0 ? $" ({SpeedBonus:+0;-0;0})" : "";

    public string StrengthText => $"{Strength} ({StrengthModifier:+0;-0;0})";
    public string DexterityText => $"{Dexterity} ({DexterityModifier:+0;-0;0})";
    public string ConstitutionText => $"{Constitution} ({ConstitutionModifier:+0;-0;0})";
    public string IntelligenceText => $"{Intelligence} ({IntelligenceModifier:+0;-0;0})";
    public string WisdomText => $"{Wisdom} ({WisdomModifier:+0;-0;0})";
    public string CharismaText => $"{Charisma} ({CharismaModifier:+0;-0;0})";

    public string ArmorClassText => $"{TotalArmorClass}";

    public string InitiativeText => $"<b>{TotalInitiative:+0;-0;0}</b>\n<size=16>Инициатива</size>";
    public string ProficiencyText => $"<b>+{ProficiencyBonus}</b>\n<size=16>Бонус умения</size>";
}

[Serializable]
public class SkillUIData
{
    public string Id;
    public string Name;
    public CharacterAbility AssociatedAbility;
    public bool IsProficient;
    public bool IsExpert;
    public int TotalBonus;
    public string BonusText => $"{TotalBonus:+0;-0;0}";
}