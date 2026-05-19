using System;
using Newtonsoft.Json;

[Serializable]
public abstract class HandbookEntity
{
    [JsonProperty("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("nameEng")]
    public string NameEng { get; set; } = string.Empty;

    [JsonProperty("description")]
    public string Description { get; set; } = string.Empty;

    [JsonProperty("isHomebrew")]
    public bool IsHomebrew { get; set; }

    [JsonProperty("isFavorite")]
    public bool IsFavorite { get; set; }

    [JsonProperty("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [JsonProperty("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}