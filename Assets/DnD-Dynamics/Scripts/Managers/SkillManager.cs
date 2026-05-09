using System.Collections.Generic;
using UnityEngine;

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

    public static List<SkillData> GetSkillsByAbility(CharacterAbility ability) => GameDataService.Instance.GetSkillsByAbility(ability);
}