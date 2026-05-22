using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public enum MonsterSize
{
    Tiny,
    Small,
    Medium,
    Large,
    Huge,
    Colossal
}

public enum MonsterType
{
    Aberration,
    Beast,
    Celestial,
    Construct,
    Dragon,
    Elemental,
    Fey,
    Fiend,
    Giant,
    Humanoid,
    Monstrosity,
    Ooze,
    Plant,
    Undead
}

public enum MonsterSubtype
{
    None,
    Aarakocra,
    WaterGenasi,
    HighElf,
    Git,
    Giff,
    Gmoll,
    Gnom,
    Goblinoid,
    Grimlock,
    Grung,
    Dwarf,
    Demon,
    Derro,
    GenasiOfEarth,
    Devil,
    Zhebolyud,
    StoneGiant,
    Quaggot,
    Kenku,
    Kobold,
    Orc,
    Shifter,
    HalfDragon,
    Saurial,
    Sahuagin,
    HalfBlood,
    Hobbit,
    HalfElf,
    Tabaxi,
    Titan,
    Typhling,
    Human,
    Elf,
    YuanTi,
    AnyRace
}

public enum MonsterAlignment
{
    LawfulGood,
    NeutralGood,
    ChaoticGood,
    LawfulNeutral,
    TrueNeutral,
    ChaoticNeutral,
    LawfulEvil,
    NeutralEvil,
    ChaoticEvil,
    Unaligned
}

public enum MonsterTerrain
{
    Arctic,
    Swamp,
    Urban,
    Mountain,
    Forest,
    Coast,
    Underwater,
    Underdark,
    Desert,
    Grassland,
    Jungle,
    Hill
}

public enum MonsterSkill
{
    Acrobatics, AnimalHandling, Arcana, Athletics, Deception,
    History, Insight, Intimidation, Investigation, Medicine,
    Nature, Perception, Performance, Persuasion, Religion,
    SleightOfHand, Stealth, Survival
}

[Serializable]
public class MonsterAbilityScores
{
    [JsonProperty("strength")]
    public int Strength { get; set; }

    [JsonProperty("dexterity")]
    public int Dexterity { get; set; }

    [JsonProperty("constitution")]
    public int Constitution { get; set; }

    [JsonProperty("intelligence")]
    public int Intelligence { get; set; }

    [JsonProperty("wisdom")]
    public int Wisdom { get; set; }

    [JsonProperty("charisma")]
    public int Charisma { get; set; }
}

[Serializable]
public class MonsterSavingThrow
{
    [JsonProperty("ability")]
    public string Ability { get; set; } = string.Empty;

    [JsonProperty("bonus")]
    public int Bonus { get; set; }
}

[Serializable]
public class MonsterSkillProficiency
{
    [JsonProperty("skill")]
    public MonsterSkill Skill { get; set; }

    [JsonProperty("bonus")]
    public int Bonus { get; set; }
}

[Serializable]
public class MonsterDamageResistance
{
    [JsonProperty("type")]
    public string Type { get; set; } = string.Empty;

    [JsonProperty("description")]
    public string Description { get; set; } = string.Empty;
}

[Serializable]
public class MonsterTrait
{
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("description")]
    public string Description { get; set; } = string.Empty;
}

[Serializable]
public class MonsterAction
{
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("description")]
    public string Description { get; set; } = string.Empty;

    [JsonProperty("attackBonus")]
    public int AttackBonus { get; set; }

    [JsonProperty("damage")]
    public string Damage { get; set; } = string.Empty;
}

[Serializable]
public class MonsterLegendaryAction
{
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("description")]
    public string Description { get; set; } = string.Empty;
}

[Serializable]
public class MonsterSpellInfo
{
    [JsonProperty("spellId")]
    public string SpellId { get; set; } = string.Empty;

    [JsonProperty("spellName")]
    public string SpellName { get; set; } = string.Empty;
}

[Serializable]
public class Monster : HandbookEntity
{
    [JsonProperty("size")]
    public MonsterSize Size { get; set; } = MonsterSize.Medium;

    [JsonProperty("type")]
    public MonsterType Type { get; set; } = MonsterType.Humanoid;

    [JsonProperty("subtype")]
    public MonsterSubtype Subtype { get; set; } = MonsterSubtype.None;

    [JsonProperty("alignment")]
    public MonsterAlignment Alignment { get; set; } = MonsterAlignment.Unaligned;

    [JsonProperty("armorClass")]
    public int ArmorClass { get; set; }

    [JsonProperty("hitPoints")]
    public int HitPoints { get; set; }

    [JsonProperty("hitDice")]
    public string HitDice { get; set; } = string.Empty;

    [JsonProperty("walkSpeed")]
    public int WalkSpeed { get; set; } = 30;

    [JsonProperty("flySpeed")]
    public int FlySpeed { get; set; }

    [JsonProperty("swimSpeed")]
    public int SwimSpeed { get; set; }

    [JsonProperty("burrowSpeed")]
    public int BurrowSpeed { get; set; }

    [JsonProperty("abilityScores")]
    public MonsterAbilityScores AbilityScores { get; set; } = new MonsterAbilityScores();

    [JsonProperty("savingThrows")]
    public List<MonsterSavingThrow> SavingThrows { get; set; } = new List<MonsterSavingThrow>();

    [JsonProperty("challengeRating")]
    public float ChallengeRating { get; set; }

    [JsonProperty("skillProficiencies")]
    public List<MonsterSkillProficiency> SkillProficiencies { get; set; } = new List<MonsterSkillProficiency>();

    [JsonProperty("damageResistances")]
    public List<MonsterDamageResistance> DamageResistances { get; set; } = new List<MonsterDamageResistance>();

    [JsonProperty("damageImmunities")]
    public List<MonsterDamageResistance> DamageImmunities { get; set; } = new List<MonsterDamageResistance>();

    [JsonProperty("conditionImmunities")]
    public List<string> ConditionImmunities { get; set; } = new List<string>();

    [JsonProperty("damageVulnerabilities")]
    public List<MonsterDamageResistance> DamageVulnerabilities { get; set; } = new List<MonsterDamageResistance>();

    [JsonProperty("senses")]
    public string Senses { get; set; } = string.Empty;

    [JsonProperty("passivePerception")]
    public int PassivePerception { get; set; }

    [JsonProperty("languages")]
    public string Languages { get; set; } = string.Empty;

    [JsonProperty("terrains")]
    public List<MonsterTerrain> Terrains { get; set; } = new List<MonsterTerrain>();

    [JsonProperty("traits")]
    public List<MonsterTrait> Traits { get; set; } = new List<MonsterTrait>();

    [JsonProperty("actions")]
    public List<MonsterAction> Actions { get; set; } = new List<MonsterAction>();

    [JsonProperty("spells")]
    public List<MonsterSpellInfo> Spells { get; set; } = new List<MonsterSpellInfo>();

    [JsonProperty("reactions")]
    public List<MonsterTrait> Reactions { get; set; } = new List<MonsterTrait>();

    [JsonProperty("legendaryDescription")]
    public string LegendaryDescription { get; set; } = string.Empty;

    [JsonProperty("legendaryActions")]
    public List<MonsterLegendaryAction> LegendaryActions { get; set; } = new List<MonsterLegendaryAction>();

    [JsonProperty("lairDescription")]
    public string LairDescription { get; set; } = string.Empty;

    [JsonProperty("lairActions")]
    public string LairActions { get; set; } = string.Empty;

    [JsonProperty("regionalDescription")]
    public string RegionalDescription { get; set; } = string.Empty;

    [JsonProperty("regionalEffects")]
    public string RegionalEffects { get; set; } = string.Empty;

    public string GetSizeDisplayName()
    {
        switch (Size)
        {
            case MonsterSize.Tiny: return "Крошечный";
            case MonsterSize.Small: return "Маленький";
            case MonsterSize.Medium: return "Средний";
            case MonsterSize.Large: return "Большой";
            case MonsterSize.Huge: return "Огромный";
            case MonsterSize.Colossal: return "Громадный";
            default: return "Средний";
        }
    }

    public string GetTypeDisplayName()
    {
        switch (Type)
        {
            case MonsterType.Aberration: return "Аберрация";
            case MonsterType.Beast: return "Зверь";
            case MonsterType.Celestial: return "Небожитель";
            case MonsterType.Construct: return "Конструкт";
            case MonsterType.Dragon: return "Дракон";
            case MonsterType.Elemental: return "Элементаль";
            case MonsterType.Fey: return "Фея";
            case MonsterType.Fiend: return "Исчадие";
            case MonsterType.Giant: return "Великан";
            case MonsterType.Humanoid: return "Гуманоид";
            case MonsterType.Monstrosity: return "Чудовище";
            case MonsterType.Ooze: return "Слизь";
            case MonsterType.Plant: return "Растение";
            case MonsterType.Undead: return "Нежить";
            default: return "Гуманоид";
        }
    }

    public string GetAlignmentDisplayName()
    {
        switch (Alignment)
        {
            case MonsterAlignment.LawfulGood: return "Законопослушный добрый";
            case MonsterAlignment.NeutralGood: return "Нейтральный добрый";
            case MonsterAlignment.ChaoticGood: return "Хаотичный добрый";
            case MonsterAlignment.LawfulNeutral: return "Законопослушный нейтральный";
            case MonsterAlignment.TrueNeutral: return "Истинно нейтральный";
            case MonsterAlignment.ChaoticNeutral: return "Хаотичный нейтральный";
            case MonsterAlignment.LawfulEvil: return "Законопослушный злой";
            case MonsterAlignment.NeutralEvil: return "Нейтральный злой";
            case MonsterAlignment.ChaoticEvil: return "Хаотичный злой";
            default: return "Без мировоззрения";
        }
    }
}