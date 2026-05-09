using System;
using System.Collections.Generic;

[Serializable]
public class SpellData
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Level { get; set; } = 0;
    public string School { get; set; } = string.Empty;
    public int CastingTime { get; set; } = 1;
    public string Range { get; set; } = "30 feet";
    public string Components { get; set; } = "V, S";
    public string Duration { get; set; } = "Instantly";
    public string Description { get; set; } = string.Empty;
    public bool IsRitual { get; set; } = false;
    public string DamageDice { get; set; } = string.Empty;
    public string DamageType { get; set; } = string.Empty;
    public string SaveAbility { get; set; } = string.Empty;
}

[Serializable]
public class SpellDataList
{
    public List<SpellData> Spells { get; set; } = new List<SpellData>();
}