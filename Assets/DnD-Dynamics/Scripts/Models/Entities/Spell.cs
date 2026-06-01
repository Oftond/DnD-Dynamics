using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public enum SpellLevel
{
    Cantrip,
    First,
    Second,
    Third,
    Fourth,
    Fifth,
    Sixth,
    Seventh,
    Eighth,
    Ninth
}

public enum SpellSchool
{
    Abjuration,
    Conjuration,
    Divination,
    Enchantment,
    Evocation,
    Illusion,
    Necromancy,
    Transmutation
}

public enum CastingTimeType
{
    Action,
    BonusAction,
    Reaction,
    Minute,
    TenMinutes,
    Hour
}

public enum DurationType
{
    Instantaneous,
    Round,
    ConcentrationUpToMinute,
    UntilDispelled,
    UpToMinute,
    UpToHour
}

[Flags]
public enum SpellComponents
{
    None = 0,
    Verbal = 1 << 0,
    Somatic = 1 << 1,
    Material = 1 << 2
}

[Serializable]
public class SpellEffect
{
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("description")]
    public string Description { get; set; } = string.Empty;
}

[Serializable]
public class Spell : HandbookEntity
{
    [JsonProperty("level")]
    public SpellLevel Level { get; set; } = SpellLevel.Cantrip;

    [JsonProperty("school")]
    public SpellSchool School { get; set; } = SpellSchool.Evocation;

    [JsonProperty("castingTimeType")]
    public CastingTimeType CastingTimeType { get; set; } = CastingTimeType.Action;

    [JsonProperty("isRitual")]
    public bool IsRitual { get; set; }

    [JsonProperty("durationType")]
    public DurationType DurationType { get; set; } = DurationType.Instantaneous;

    [JsonProperty("components")]
    public SpellComponents Components { get; set; } = SpellComponents.Verbal | SpellComponents.Somatic;

    [JsonProperty("materials")]
    public string Materials { get; set; } = string.Empty;

    [JsonProperty("range")]
    public string Range { get; set; } = "30 feet";

    [JsonProperty("availableClassIds")]
    public List<string> AvailableClassIds { get; set; } = new List<string>();

    [JsonIgnore]
    private List<CharacterClass> _availableClasses { get; set; } = new List<CharacterClass>();

    [JsonIgnore]
    public List<CharacterClass> AvailableClasses
    {
        get => _availableClasses;
        set
        {
            _availableClasses = value;

            if (value != null)
            {
                AvailableClassIds = new List<string>();

                foreach (var cls in value)
                {
                    if (cls != null)
                        AvailableClassIds.Add(cls.Id);
                }
            }
        }
    }

    [JsonProperty("fullDescription")]
    public string FullDescription { get; set; } = string.Empty;

    [JsonProperty("effects")]
    public List<SpellEffect> Effects { get; set; } = new List<SpellEffect>();

    [JsonProperty("higherLevels")]
    public string HigherLevels { get; set; } = string.Empty;

    public void SyncAvailableClasses(Dictionary<string, CharacterClass> characterClasses)
    {
        _availableClasses = new List<CharacterClass>();

        foreach (var id in AvailableClassIds)
        {
            if (characterClasses.TryGetValue(id, out var cls))
                _availableClasses.Add(cls);
        }
    }

    public string GetLevelDisplayName()
    {
        switch (Level)
        {
            case SpellLevel.Cantrip: return "Заговор";
            case SpellLevel.First: return "1 круг";
            case SpellLevel.Second: return "2 круг";
            case SpellLevel.Third: return "3 круг";
            case SpellLevel.Fourth: return "4 круг";
            case SpellLevel.Fifth: return "5 круг";
            case SpellLevel.Sixth: return "6 круг";
            case SpellLevel.Seventh: return "7 круг";
            case SpellLevel.Eighth: return "8 круг";
            case SpellLevel.Ninth: return "9 круг";
            default: return "Неизвестно";
        }
    }

    public string GetSchoolDisplayName()
    {
        switch (School)
        {
            case SpellSchool.Abjuration: return "Ограждение";
            case SpellSchool.Conjuration: return "Призыв";
            case SpellSchool.Divination: return "Прорицание";
            case SpellSchool.Enchantment: return "Очарование";
            case SpellSchool.Evocation: return "Воплощение";
            case SpellSchool.Illusion: return "Иллюзия";
            case SpellSchool.Necromancy: return "Некромантия";
            case SpellSchool.Transmutation: return "Преобразование";
            default: return "Неизвестно";
        }
    }

    public string GetCastingTimeDisplayName()
    {
        switch (CastingTimeType)
        {
            case CastingTimeType.Action: return "1 действие";
            case CastingTimeType.BonusAction: return "1 бонусное действие";
            case CastingTimeType.Reaction: return "1 реакция";
            case CastingTimeType.Minute: return "1 минута";
            case CastingTimeType.TenMinutes: return "10 минут";
            case CastingTimeType.Hour: return "1 час";
            default: return "1 действие";
        }
    }

    public string GetDurationDisplayName()
    {
        switch (DurationType)
        {
            case DurationType.Round: return "1 раунд";
            case DurationType.ConcentrationUpToMinute: return "Концентрация до 1 минуты";
            case DurationType.UntilDispelled: return "Пока не будет рассеяно";
            case DurationType.UpToMinute: return "Вплоть до 1 минуты";
            case DurationType.UpToHour: return "Вплоть до 1 часа";
            default: return "Мгновенная";
        }
    }

    public string GetComponentsDisplayString()
    {
        var parts = new List<string>();

        if ((Components & SpellComponents.Verbal) != 0) parts.Add("В");
        if ((Components & SpellComponents.Somatic) != 0) parts.Add("С");
        if ((Components & SpellComponents.Material) != 0) parts.Add("М");

        return string.Join(", ", parts);
    }
}