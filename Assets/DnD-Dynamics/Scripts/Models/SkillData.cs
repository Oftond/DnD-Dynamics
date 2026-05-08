using System;
using System.Collections.Generic;

[Serializable]
public class SkillData
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string NameRu { get; set; } = string.Empty;
    public CharacterAbility AssociatedAbility { get; set; } = CharacterAbility.Strength;
    public string Description { get; set; } = string.Empty;
    public string DescriptionRu { get; set; } = string.Empty;
}

[Serializable]
public class SkillDataList
{
    public List<SkillData> Skills { get; set; } = new List<SkillData>();
}