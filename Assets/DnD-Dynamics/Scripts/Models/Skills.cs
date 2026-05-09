using System;
using System.Collections.Generic;

[Serializable]
public class Skill
{
    public string Id { get; set; } = string.Empty;
    public bool IsProficient { get; set; } = false;
    public bool IsExpert { get; set; } = false;

    private SkillData _data;

    public void SetData(SkillData data)
    {
        _data = data;
        Id = data.Id;
    }

    public string GetName() => _data?.Name ?? "Unknown";

    public CharacterAbility GetAssociatedAbility() => _data?.AssociatedAbility ?? CharacterAbility.Strength;

    public string GetDescription() => _data?.Description ?? string.Empty;

    public int CalculateBonus(CharacterData character)
    {
        var ability = GetAssociatedAbility();
        var modifier = character.TotalStats.GetModifier(ability);
        var proficiencyBonus = character.ProficiencyBonus;

        if (IsExpert)
            return modifier + proficiencyBonus * 2;
        else if (IsProficient)
            return modifier + proficiencyBonus;
        else
            return modifier;
    }
}