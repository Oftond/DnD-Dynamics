using DnD_Dynamics.Services;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterUiMapper : ICharacterUiMapper
{
    private readonly ICharacterStatCalculator _statCalculator;

    public CharacterUiMapper(ICharacterStatCalculator statCalculator)
    {
        _statCalculator = statCalculator;
    }

    public CharacterUIData MapToUi(CharacterData character, CharacterStats totalStats, int maxHp, CharacterRace race = null, CharacterClass @class = null)
    {
        return new CharacterUIData
        {
            Id = character.Id,
            Name = character.Name,
            ClassName = @class?.GetDisplayName() ?? "Неизвестный класс",
            RaceName = race?.GetDisplayName() ?? "Неизвестная раса",
            Level = character.Level,
            CurrentHp = character.CurrentHp,
            MaxHp = maxHp,
            ArmorClass = character.TotalArmorClass,
            BaseArmorClass = character.ArmorClass,
            ShieldBonus = character.ShieldBonus,
            IsShieldActive = character.IsShieldActive,
            HasInspiration = character.HasInspiration,
            InitiativeBonus = totalStats.GetModifier(CharacterAbility.Dexterity),
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
}