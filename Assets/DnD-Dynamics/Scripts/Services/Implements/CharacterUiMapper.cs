using DnD_Dynamics.Services;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterUiMapper : ICharacterUiMapper
{
    private readonly ICharacterStatCalculator _statCalculator;

    public CharacterUiMapper(ICharacterStatCalculator statCalculator)
    {
        _statCalculator = statCalculator;
    }

    public CharacterUIData MapToUi(CharacterData character, CharacterStats totalStats, int maxHp, CharacterRace race = null, CharacterClass @class = null, List<Skill> skills = null)
    {
        var baseSpeed = race?.Speed ?? CharacterRace.DefaultSpeed;
        var totalSpeed = baseSpeed + character.SpeedBonus;

        var expToNext = CalculateExpForLevel(character.Level + 1);

        var initiativeBase = totalStats.GetModifier(CharacterAbility.Dexterity);
        var totalInitiative = initiativeBase + character.InitiativeBonus;

        var skillUiData = skills?.Select(s => new SkillUIData
        {
            Id = s.Id,
            Name = s.GetName(),
            AssociatedAbility = s.GetAssociatedAbility(),
            IsProficient = s.IsProficient,
            IsExpert = s.IsExpert,
            TotalBonus = s.CalculateBonus(totalStats, character.ProficiencyBonus)
        }).ToList() ?? new List<SkillUIData>();

        return new CharacterUIData
        {
            Id = character.Id,
            Name = character.Name,
            ClassName = @class?.GetDisplayName() ?? "Неизвестный класс",
            RaceName = race?.GetDisplayName() ?? "Неизвестная раса",
            CurrentHp = character.CurrentHp,
            MaxHp = maxHp,

            Level = character.Level,
            ExperiencePoints = character.ExperiencePoints,
            ExperienceToNextLevel = expToNext,

            TotalArmorClass = character.TotalArmorClass,
            BaseArmorClass = character.ArmorClass,
            ShieldBonus = character.ShieldBonus,
            IsShieldActive = character.IsShieldActive,

            BaseSpeed = baseSpeed,
            SpeedBonus = character.SpeedBonus,
            TotalSpeed = totalSpeed,

            InitiativeBase = initiativeBase,
            InitiativeBonus = character.InitiativeBonus,
            TotalInitiative = totalInitiative,

            HasInspiration = character.HasInspiration,

            Strength = totalStats.Strength,
            Dexterity = totalStats.Dexterity,
            Constitution = totalStats.Constitution,
            Intelligence = totalStats.Intelligence,
            Wisdom = totalStats.Wisdom,
            Charisma = totalStats.Charisma,

            StrengthModifier = totalStats.GetModifier(CharacterAbility.Strength),
            DexterityModifier = totalStats.GetModifier(CharacterAbility.Dexterity),
            ConstitutionModifier = totalStats.GetModifier(CharacterAbility.Constitution),
            IntelligenceModifier = totalStats.GetModifier(CharacterAbility.Intelligence),
            WisdomModifier = totalStats.GetModifier(CharacterAbility.Wisdom),
            CharismaModifier = totalStats.GetModifier(CharacterAbility.Charisma),

            ProficiencyBonus = character.ProficiencyBonus,

            PortraitPath = character.PortraitPath,
            Backstory = character.Backstory,
            Notes = character.Notes
        };
    }

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