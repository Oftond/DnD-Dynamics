using DnD_Dynamics.Services;
using System.Collections.Generic;
using Zenject;

public class SkillManager
{
    [Inject] private IGameDataService _gameDataService;

    private List<SkillData> _allSkillsData;

    public List<SkillData> GetAllSkillsData()
    {
        if (_allSkillsData == null || _allSkillsData.Count == 0)
        {
            _allSkillsData = _gameDataService.LoadSkills();
        }

        return _allSkillsData;
    }

    public List<Skill> CreateCharacterSkills()
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

    public SkillData GetSkillDataById(string id)
    {
        var skills = GetAllSkillsData();
        return skills.Find(s => s.Id == id);
    }

    public List<SkillData> GetSkillsByAbility(CharacterAbility ability) => _gameDataService.GetSkillsByAbility(ability);
}