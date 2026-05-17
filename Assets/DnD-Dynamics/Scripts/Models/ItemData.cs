using System;
using System.Collections.Generic;

[Serializable]
public class ItemData
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = "Other";
    public string Rarity { get; set; } = "Common";
    public string Description { get; set; } = string.Empty;

    public int Weight { get; set; } = 0;
    public int Cost { get; set; } = 0;

    public string DamageDice { get; set; } = "1d4";
    public string DamageType { get; set; } = "Bludgeoning";
    public bool IsFinesse { get; set; } = false;
    public bool IsLight { get; set; } = false;
    public bool IsHeavy { get; set; } = false;
    public bool IsTwoHanded { get; set; } = false;
    public bool IsVersatile { get; set; } = false;//FreshCraft 
    public string VersatileDamage { get; set; } = "1d6";
    public int Range { get; set; } = 0;
    public int LongRange { get; set; } = 0;

    public int ArmorClass { get; set; } = 10;
    public int StrengthRequirement { get; set; } = 0;
    public bool HasStealthDisadvantage { get; set; } = false;

    public int MagicBonus { get; set; } = 0;
    public bool RequiresAttunement { get; set; } = false;
}

[Serializable]
public class ItemDataList
{
    public List<ItemData> Items { get; set; } = new List<ItemData>();
}