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

[Serializable]
public class SerializableInventory
{
    public List<string> ItemIds { get; set; } = new List<string>();
    public Dictionary<string, int> ItemQuantities { get; set; } = new Dictionary<string, int>();
    public string MainHandWeaponId { get; set; }
    public string OffHandWeaponId { get; set; }
    public string ArmorId { get; set; }
    public string ShieldId { get; set; }
    public string HelmetId { get; set; }
    public string GlovesId { get; set; }
    public string BootsId { get; set; }
    public string BeltId { get; set; }
    public string CloakId { get; set; }
    public string AmuletId { get; set; }
    public string Ring1Id { get; set; }
    public string Ring2Id { get; set; }

    public static SerializableInventory FromInventory(Inventory inventory)
    {
        var serializable = new SerializableInventory
        {
            ItemIds = new List<string>(),
            ItemQuantities = new Dictionary<string, int>(),
            MainHandWeaponId = inventory.MainHandWeapon?.Id,
            OffHandWeaponId = inventory.OffHandWeapon?.Id,
            ArmorId = inventory.Armor?.Id,
            ShieldId = inventory.Shield?.Id,
            HelmetId = inventory.Helmet?.Id,
            GlovesId = inventory.Gloves?.Id,
            BootsId = inventory.Boots?.Id,
            BeltId = inventory.Belt?.Id,
            CloakId = inventory.Cloak?.Id,
            AmuletId = inventory.Amulet?.Id,
            Ring1Id = inventory.Ring1?.Id,
            Ring2Id = inventory.Ring2?.Id
        };

        foreach (var item in inventory.Items)
        {
            serializable.ItemIds.Add(item.Id);
            serializable.ItemQuantities[item.Id] = item.Quantity;
        }

        return serializable;
    }

    public Inventory ToInventory(CharacterData character)
    {
        var inventory = new Inventory(character);

        foreach (var itemId in ItemIds)
        {
            var itemData = ItemManager.GetItemDataById(itemId);
            if (itemData != null)
            {
                var item = new Item();
                item.SetData(itemData);
                item.Quantity = ItemQuantities.TryGetValue(itemId, out var qty) ? qty : 1;
                inventory.AddItem(item);
            }
        }

        if (!string.IsNullOrEmpty(MainHandWeaponId))
            inventory.MainHandWeapon = inventory.Items.Find(i => i.Id == MainHandWeaponId);
        if (!string.IsNullOrEmpty(OffHandWeaponId))
            inventory.OffHandWeapon = inventory.Items.Find(i => i.Id == OffHandWeaponId);
        if (!string.IsNullOrEmpty(ArmorId))
            inventory.Armor = inventory.Items.Find(i => i.Id == ArmorId);
        if (!string.IsNullOrEmpty(ShieldId))
            inventory.Shield = inventory.Items.Find(i => i.Id == ShieldId);
        if (!string.IsNullOrEmpty(HelmetId))
            inventory.Helmet = inventory.Items.Find(i => i.Id == HelmetId);
        if (!string.IsNullOrEmpty(GlovesId))
            inventory.Gloves = inventory.Items.Find(i => i.Id == GlovesId);
        if (!string.IsNullOrEmpty(BootsId))
            inventory.Boots = inventory.Items.Find(i => i.Id == BootsId);
        if (!string.IsNullOrEmpty(BeltId))
            inventory.Belt = inventory.Items.Find(i => i.Id == BeltId);
        if (!string.IsNullOrEmpty(CloakId))
            inventory.Cloak = inventory.Items.Find(i => i.Id == CloakId);
        if (!string.IsNullOrEmpty(AmuletId))
            inventory.Amulet = inventory.Items.Find(i => i.Id == AmuletId);
        if (!string.IsNullOrEmpty(Ring1Id))
            inventory.Ring1 = inventory.Items.Find(i => i.Id == Ring1Id);
        if (!string.IsNullOrEmpty(Ring2Id))
            inventory.Ring2 = inventory.Items.Find(i => i.Id == Ring2Id);

        return inventory;
    }
}

[Serializable]
public class Inventory
{
    public List<Item> Items { get; set; } = new List<Item>();

    public Item MainHandWeapon { get; set; }
    public Item OffHandWeapon { get; set; }
    public Item Armor { get; set; }
    public Item Shield { get; set; }
    public Item Helmet { get; set; }
    public Item Gloves { get; set; }
    public Item Boots { get; set; }
    public Item Belt { get; set; }
    public Item Cloak { get; set; }
    public Item Amulet { get; set; }
    public Item Ring1 { get; set; }
    public Item Ring2 { get; set; }

    public int TotalWeight => CalculateTotalWeight();
    public int CarryingCapacity => CalculateCarryingCapacity();

    private CharacterData _character;

    public Inventory(CharacterData character)
    {
        _character = character;
    }

    public void SetCharacter(CharacterData character)
    {
        _character = character;
    }

    public void AddItem(Item item)
    {
        var existingItem = Items.Find(i => i.Id == item.Id);
        if (existingItem != null)
        {
            existingItem.Quantity += item.Quantity;
        }
        else
        {
            Items.Add(item);
        }
    }

    public void RemoveItem(string itemId, int quantity = 1)
    {
        var item = Items.Find(i => i.Id == itemId);
        if (item != null)
        {
            if (item.Quantity > quantity)
            {
                item.Quantity -= quantity;
            }
            else
            {
                Items.Remove(item);
            }
        }
    }

    public Item FindItem(string itemId) => Items.Find(i => i.Id == itemId);

    public List<Item> FindItemsByName(string name) => Items.FindAll(i => i.Name.Contains(name, StringComparison.OrdinalIgnoreCase));

    public void EquipItem(Item item)
    {
        switch (item.Type)
        {
            case "Weapon":
                if (MainHandWeapon == null || item.IsTwoHanded)
                    MainHandWeapon = item;
                else
                    OffHandWeapon = item;
                break;
            case "Armor":
                Armor = item;
                break;
            case "Shield":
                Shield = item;
                break;
            case "Helmet":
                Helmet = item;
                break;
            case "Gloves":
                Gloves = item;
                break;
            case "Boots":
                Boots = item;
                break;
            case "Belt":
                Belt = item;
                break;
            case "Cloak":
                Cloak = item;
                break;
            case "Amulet":
                Amulet = item;
                break;
            case "Ring":
                if (Ring1 == null)
                    Ring1 = item;
                else if (Ring2 == null)
                    Ring2 = item;
                break;
        }
    }

    public void UnequipItem(Item item)
    {
        switch (item.Type)
        {
            case "Weapon":
                if (MainHandWeapon?.Id == item.Id) MainHandWeapon = null;
                else if (OffHandWeapon?.Id == item.Id) OffHandWeapon = null;
                break;
            case "Armor":
                if (Armor?.Id == item.Id) Armor = null;
                break;
            case "Shield":
                if (Shield?.Id == item.Id) Shield = null;
                break;
            case "Helmet":
                if (Helmet?.Id == item.Id) Helmet = null;
                break;
            case "Gloves":
                if (Gloves?.Id == item.Id) Gloves = null;
                break;
            case "Boots":
                if (Boots?.Id == item.Id) Boots = null;
                break;
            case "Belt":
                if (Belt?.Id == item.Id) Belt = null;
                break;
            case "Cloak":
                if (Cloak?.Id == item.Id) Cloak = null;
                break;
            case "Amulet":
                if (Amulet?.Id == item.Id) Amulet = null;
                break;
            case "Ring":
                if (Ring1?.Id == item.Id) Ring1 = null;
                else if (Ring2?.Id == item.Id) Ring2 = null;
                break;
        }
    }

    private int CalculateTotalWeight()
    {
        int totalWeight = 0;
        foreach (var item in Items)
            totalWeight += item.Weight * item.Quantity;
        return totalWeight;
    }

    private int CalculateCarryingCapacity()
    {
        if (_character == null) return 150;

        return _character.TotalStats.Strength * 15;
    }

    public bool IsEncumbered() => TotalWeight > CarryingCapacity;

    public bool CanCarryMore() => TotalWeight < CarryingCapacity;

    public int GetAvailableCapacity() => Math.Max(0, CarryingCapacity - TotalWeight);
}