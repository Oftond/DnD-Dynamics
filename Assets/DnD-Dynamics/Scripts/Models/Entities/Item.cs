using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public enum ItemRarity
{
    Common,
    Uncommon,
    Rare,
    VeryRare,
    Legendary,
    Artifact
}

public enum ItemType
{
    Weapon,
    Armor,
    Wand,
    Rod,
    Staff,
    Ring,
    WondrousItem,
    Potion,
    Other
}

[Serializable]
public class ItemSpecialFeature
{
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("description")]
    public string Description { get; set; } = string.Empty;
}

[Serializable]
public class ItemSpellInfo
{
    [JsonProperty("spellId")]
    public string SpellId { get; set; } = string.Empty;

    [JsonProperty("spellName")]
    public string SpellName { get; set; } = string.Empty;
}

[Serializable]
public class Item : HandbookEntity
{
    [JsonProperty("itemType")]
    public ItemType Type { get; set; } = ItemType.Other;

    [JsonProperty("rarity")]
    public ItemRarity Rarity { get; set; } = ItemRarity.Common;

    [JsonProperty("requiresAttunement")]
    public bool RequiresAttunement { get; set; }

    [JsonProperty("attunementDescription")]
    public string AttunementDescription { get; set; } = string.Empty;

    [JsonProperty("weight")]
    public int Weight { get; set; }

    [JsonProperty("cost")]
    public int Cost { get; set; }

    [JsonProperty("damageDice")]
    public string DamageDice { get; set; } = "1d4";

    [JsonProperty("damageType")]
    public string DamageType { get; set; } = "Bludgeoning";

    [JsonProperty("isFinesse")]
    public bool IsFinesse { get; set; }

    [JsonProperty("isLight")]
    public bool IsLight { get; set; }

    [JsonProperty("isHeavy")]
    public bool IsHeavy { get; set; }

    [JsonProperty("isTwoHanded")]
    public bool IsTwoHanded { get; set; }

    [JsonProperty("isVersatile")]
    public bool IsVersatile { get; set; }

    [JsonProperty("versatileDamage")]
    public string VersatileDamage { get; set; } = "1d6";

    [JsonProperty("range")]
    public int Range { get; set; }

    [JsonProperty("longRange")]
    public int LongRange { get; set; }

    [JsonProperty("armorClass")]
    public int ArmorClass { get; set; }

    [JsonProperty("strengthRequirement")]
    public int StrengthRequirement { get; set; }

    [JsonProperty("hasStealthDisadvantage")]
    public bool HasStealthDisadvantage { get; set; }

    [JsonProperty("magicBonus")]
    public int MagicBonus { get; set; }

    [JsonProperty("isAttuned")]
    public bool IsAttuned { get; set; }

    [JsonProperty("specialFeatures")]
    public List<ItemSpecialFeature> SpecialFeatures { get; set; } = new List<ItemSpecialFeature>();

    [JsonProperty("containedSpells")]
    public List<ItemSpellInfo> ContainedSpells { get; set; } = new List<ItemSpellInfo>();

    public Item ShallowCopy() => (Item)MemberwiseClone();

    public string GetRarityDisplayName()
    {
        switch (Rarity)
        {
            case ItemRarity.Common: return "Обычный";
            case ItemRarity.Uncommon: return "Необычный";
            case ItemRarity.Rare: return "Редкий";
            case ItemRarity.VeryRare: return "Очень редкий";
            case ItemRarity.Legendary: return "Легендарный";
            case ItemRarity.Artifact: return "Артефакт";
            default: return "Неизвестно";
        }
    }

    public string GetTypeDisplayName()
    {
        switch (Type)
        {
            case ItemType.Weapon: return "Оружие";
            case ItemType.Armor: return "Доспех";
            case ItemType.Wand: return "Волшебная палочка";
            case ItemType.Rod: return "Жезл";
            case ItemType.Staff: return "Посох";
            case ItemType.Ring: return "Кольцо";
            case ItemType.WondrousItem: return "Чудесный предмет";
            case ItemType.Potion: return "Зелье";
            default: return "Другое";
        }
    }
}