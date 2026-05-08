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

    public string GetName()
    {
        return _data?.NameRu ?? "Неизвестно";
    }

    public string GetEnglishName()
    {
        return _data?.Name ?? "Unknown";
    }

    public CharacterAbility GetAssociatedAbility()
    {
        return _data?.AssociatedAbility ?? CharacterAbility.Strength;
    }

    public string GetDescription()
    {
        return _data?.DescriptionRu ?? string.Empty;
    }

    public int CalculateBonus(CharacterData character)
    {
        var ability = GetAssociatedAbility();
        var modifier = character.TotalStats.GetModifier(ability);
        var proficiencyBonus = character.ProficiencyBonus;

        if (IsExpert)
        {
            return modifier + proficiencyBonus * 2;
        }
        else if (IsProficient)
        {
            return modifier + proficiencyBonus;
        }
        else
        {
            return modifier;
        }
    }
}

public static class SkillManager
{
    private static List<SkillData> _allSkillsData;

    public static List<SkillData> GetAllSkillsData()
    {
        if (_allSkillsData == null || _allSkillsData.Count == 0)
        {
            _allSkillsData = GameDataService.Instance.LoadSkills();
        }
        return _allSkillsData;
    }

    public static List<Skill> CreateCharacterSkills()
    {
        var skills = new List<Skill>();
        var skillsData = GetAllSkillsData();

        foreach (var skillData in skillsData)
        {
            skills.Add(new Skill
            {
                Id = skillData.Id,
                IsProficient = false,
                IsExpert = false
            });

            skills[^1].SetData(skillData);
        }

        return skills;
    }

    public static SkillData GetSkillDataById(string id)
    {
        var skills = GetAllSkillsData();
        return skills.Find(s => s.Id == id);
    }

    public static List<SkillData> GetSkillsByAbility(CharacterAbility ability)
    {
        return GameDataService.Instance.GetSkillsByAbility(ability);
    }
}