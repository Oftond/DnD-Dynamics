using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public enum CasterType
{
    None,
    FullCaster,
    HalfCaster,
    ThirdCaster
}

[Serializable]
public class CharacterClass : HandbookEntity
{
    [JsonProperty("hitDice")] public int HitDice { get; set; } = 8;
    [JsonProperty("primaryAbility")] public string PrimaryAbility { get; set; } = "Strength";
    [JsonProperty("savingThrows")] public List<string> SavingThrows { get; set; } = new();
    [JsonProperty("features")] public List<ClassFeature> Features { get; set; } = new();
    [JsonProperty("casterType")] public CasterType CasterType { get; set; } = CasterType.None;

    public int GetHitDice() => HitDice;
    public string GetPrimaryAbility() => PrimaryAbility;

    public string GetDisplayName() => Name ?? "Неизвестно";
}

[Serializable]
public class ClassFeature
{
    [JsonProperty("level")] public int Level { get; set; }
    [JsonProperty("name")] public string Name { get; set; } = string.Empty;
    [JsonProperty("description")] public string Description { get; set; } = string.Empty;
}