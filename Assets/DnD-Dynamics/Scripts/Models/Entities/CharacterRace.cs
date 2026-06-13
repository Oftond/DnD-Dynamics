using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable]
public class CharacterRace : HandbookEntity
{
    public const int DefaultSpeed = 30;

    [JsonProperty("speed")] public int Speed { get; set; } = DefaultSpeed;
    [JsonProperty("size")] public string Size { get; set; } = "Medium";
    [JsonProperty("abilityBonuses")] public Dictionary<string, int> AbilityBonuses { get; set; } = new();
    [JsonProperty("languages")] public List<string> Languages { get; set; } = new();
    [JsonProperty("traits")] public List<RaceTrait> Traits { get; set; } = new();

    public int GetAbilityBonus(CharacterAbility ability)
    {
        string key = ability.ToString();

        return AbilityBonuses.ContainsKey(key) ? AbilityBonuses[key] : 0;
    }

    public string GetDisplayName() => Name ?? "Неизвестно";
}

[Serializable]
public class RaceTrait
{
    [JsonProperty("name")] public string Name { get; set; } = string.Empty;
    [JsonProperty("description")] public string Description { get; set; } = string.Empty;
}