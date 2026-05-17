using System;
using System.Collections.Generic;

[Serializable]
public class Item
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = "Other";
    public string Rarity { get; set; } = "Common";
    public string Description { get; set; } = string.Empty;

    public int Weight { get; set; } = 0;
    public int Cost { get; set; } = 0;
    public int Quantity { get; set; } = 1;

    public string DamageDice { get; set; } = "1d4";
    public string DamageType { get; set; } = "Bludgeoning";
    public bool IsFinesse { get; set; } = false;
    public bool IsLight { get; set; } = false;
    public bool IsHeavy { get; set; } = false;
    public bool IsTwoHanded { get; set; } = false;
    public bool IsVersatile { get; set; } = false;
    public string VersatileDamage { get; set; } = "1d6";
    public int Range { get; set; } = 0;
    public int LongRange { get; set; } = 0;

    public int ArmorClass { get; set; } = 10;
    public int StrengthRequirement { get; set; } = 0;
    public bool HasStealthDisadvantage { get; set; } = false;

    public int MagicBonus { get; set; } = 0;
    public bool RequiresAttunement { get; set; } = false;
    public bool IsAttuned { get; set; } = false;

    private ItemData _data;

    public void SetData(ItemData data)
    {
        _data = data;
        Id = data.Id;
        Name = data.Name;
        Type = data.Type;
        Rarity = data.Rarity;
        Description = data.Description;
        Weight = data.Weight;
        Cost = data.Cost;
        DamageDice = data.DamageDice;
        DamageType = data.DamageType;
        IsFinesse = data.IsFinesse;
        IsLight = data.IsLight;
        IsHeavy = data.IsHeavy;
        IsTwoHanded = data.IsTwoHanded;
        IsVersatile = data.IsVersatile;
        VersatileDamage = data.VersatileDamage;
        Range = data.Range;
        LongRange = data.LongRange;
        ArmorClass = data.ArmorClass;
        StrengthRequirement = data.StrengthRequirement;
        HasStealthDisadvantage = data.HasStealthDisadvantage;
        MagicBonus = data.MagicBonus;
        RequiresAttunement = data.RequiresAttunement;
    }

    public string GetTypeDisplayName()
    {
        return Type switch
        {
            "Weapon" => "Оружие",
            "Armor" => "Броня",
            "Shield" => "Щит",
            "Potion" => "Зелье",
            "Scroll" => "Свиток",
            "Ring" => "Кольцо",
            "Wand" => "Жезл",
            "Staff" => "Посох",
            "Rod" => "Жезл силы",
            "WondrousItem" => "Чудесный предмет",
            "Amulet" => "Амулет",
            "Cloak" => "Плащ",
            "Boots" => "Обувь",
            "Gloves" => "Перчатки",
            "Helmet" => "Шлем",
            "Belt" => "Пояс",
            "Tool" => "Инструмент",
            "MaterialComponent" => "Компонент заклинаний",
            "Container" => "Контейнер",
            _ => "Прочее"
        };
    }

    public string GetRarityDisplayName()
    {
        return Rarity switch
        {
            "Common" => "Обычный",
            "Uncommon" => "Необычный",
            "Rare" => "Редкий",
            "VeryRare" => "Очень редкий",
            "Legendary" => "Легендарный",
            "Artifact" => "Артефакт",
            _ => "Неизвестно"
        };
    }

    public string GetFullDescription()
    {
        var desc = $"{Name} ({GetTypeDisplayName()}, {GetRarityDisplayName()})";
        if (MagicBonus != 0)
            desc += $" +{MagicBonus}";

        return desc;
    }
}