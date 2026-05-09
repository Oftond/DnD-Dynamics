using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameDataService
{
    private static GameDataService _instance;
    public static GameDataService Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameDataService();
            }
            return _instance;
        }
    }

    private List<SkillData> _skills;
    private List<SpellData> _spells;
    private List<ItemData> _items;

    public List<SkillData> LoadSkills()
    {
        if (_skills != null && _skills.Count > 0)
            return _skills;

        try
        {
            TextAsset skillDataFile = Resources.Load<TextAsset>("Data/skills");
            if (skillDataFile != null)
            {
                var skillDataList = JsonUtility.FromJson<SkillDataList>(skillDataFile.text);
                _skills = skillDataList.Skills;
                Debug.Log($"Загружено {_skills.Count} навыков из JSON");
                return _skills;
            }

            Debug.LogWarning("Файл skills.json не найден в Resources/Data");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Ошибка загрузки навыков: {ex.Message}");
        }

        _skills = GetDefaultSkills();

        return _skills;
    }

    public List<SpellData> LoadSpells()
    {
        if (_spells != null && _spells.Count > 0)
        {
            return _spells;
        }

        try
        {
            TextAsset spellDataFile = Resources.Load<TextAsset>("Data/spells");
            if (spellDataFile != null)
            {
                var spellDataList = JsonUtility.FromJson<SpellDataList>(spellDataFile.text);
                _spells = spellDataList.Spells;
                Debug.Log($"Загружено {_spells.Count} заклинаний из JSON");
                return _spells;
            }

            Debug.LogWarning("Файл spells.json не найден в Resources/Data");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Ошибка загрузки заклинаний: {ex.Message}");
        }

        _spells = GetDefaultSpells();

        return _spells;
    }

    public List<ItemData> LoadItems()
    {
        if (_items != null && _items.Count > 0)
        {
            return _items;
        }

        try
        {
            TextAsset itemDataFile = Resources.Load<TextAsset>("Data/items");
            if (itemDataFile != null)
            {
                var itemDataList = JsonUtility.FromJson<ItemDataList>(itemDataFile.text);
                _items = itemDataList.Items;
                Debug.Log($"Загружено {_items.Count} предметов из JSON");
                return _items;
            }

            Debug.LogWarning("Файл items.json не найден в Resources/Data");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Ошибка загрузки предметов: {ex.Message}");
        }

        _items = GetDefaultItems();

        return _items;
    }

    public SkillData GetSkillById(string id)
    {
        var skills = LoadSkills();

        return skills.Find(s => s.Id == id);
    }

    public SpellData GetSpellById(string id)
    {
        var spells = LoadSpells();

        return spells.Find(s => s.Id == id);
    }

    public ItemData GetItemById(string id)
    {
        var items = LoadItems();

        return items.Find(i => i.Id == id);
    }

    public List<SkillData> GetSkillsByAbility(CharacterAbility ability)
    {
        var skills = LoadSkills();

        return skills.FindAll(s => s.AssociatedAbility == ability);
    }

    public List<SpellData> GetSpellsByLevel(int level)
    {
        var spells = LoadSpells();

        return spells.FindAll(s => s.Level == level);
    }

    public List<ItemData> GetItemsByType(string type)
    {
        var items = LoadItems();

        return items.FindAll(i => i.Type == type);
    }

    #region Default Data

    private List<SkillData> GetDefaultSkills()
    {
        return new List<SkillData>
        {
            new SkillData { Id = "acrobatics", Name = "Acrobatics", AssociatedAbility = CharacterAbility.Dexterity },
            new SkillData { Id = "animal_handling", Name = "Animal Handling", AssociatedAbility = CharacterAbility.Wisdom },
            new SkillData { Id = "arcana", Name = "Arcana", AssociatedAbility = CharacterAbility.Intelligence },
            new SkillData { Id = "athletics", Name = "Athletics", AssociatedAbility = CharacterAbility.Strength },
            new SkillData { Id = "deception", Name = "Deception", AssociatedAbility = CharacterAbility.Charisma },
            new SkillData { Id = "history", Name = "History", AssociatedAbility = CharacterAbility.Intelligence },
            new SkillData { Id = "insight", Name = "Insight", AssociatedAbility = CharacterAbility.Wisdom },
            new SkillData { Id = "intimidation", Name = "Intimidation", AssociatedAbility = CharacterAbility.Charisma },
            new SkillData { Id = "investigation", Name = "Investigation", AssociatedAbility = CharacterAbility.Intelligence },
            new SkillData { Id = "medicine", Name = "Medicine", AssociatedAbility = CharacterAbility.Wisdom },
            new SkillData { Id = "nature", Name = "Nature", AssociatedAbility = CharacterAbility.Intelligence },
            new SkillData { Id = "perception", Name = "Perception", AssociatedAbility = CharacterAbility.Wisdom },
            new SkillData { Id = "performance", Name = "Performance", AssociatedAbility = CharacterAbility.Charisma },
            new SkillData { Id = "persuasion", Name = "Persuasion", AssociatedAbility = CharacterAbility.Charisma },
            new SkillData { Id = "religion", Name = "Religion", AssociatedAbility = CharacterAbility.Intelligence },
            new SkillData { Id = "sleight_of_hand", Name = "Sleight of Hand", AssociatedAbility = CharacterAbility.Dexterity },
            new SkillData { Id = "stealth", Name = "Stealth", AssociatedAbility = CharacterAbility.Dexterity },
            new SkillData { Id = "survival", Name = "Survival", AssociatedAbility = CharacterAbility.Wisdom }
        };
    }

    private List<SpellData> GetDefaultSpells()
    {
        return new List<SpellData>
        {
            new SpellData { Id = "fire_bolt", Name = "Fire Bolt", Level = 0, School = "Evocation", DamageDice = "1d10", DamageType = "Fire" },
            new SpellData { Id = "magic_missile", Name = "Magic Missile", Level = 1, School = "Evocation", DamageDice = "1d4+1", DamageType = "Force" },
            new SpellData { Id = "cure_wounds", Name = "Cure Wounds", Level = 1, School = "Evocation"},
            new SpellData { Id = "shield", Name = "Shield", Level = 1, School = "Abjuration" },
            new SpellData { Id = "detect_magic", Name = "Detect Magic", Level = 1, School = "Divination", IsRitual = true }
        };
    }

    private List<ItemData> GetDefaultItems()
    {
        return new List<ItemData>
        {
            new ItemData { Id = "longsword", Name = "Longsword", Type = "Weapon", Weight = 3, Cost = 15, DamageDice = "1d8", DamageType = "Slashing", IsVersatile = true, VersatileDamage = "1d10" },
            new ItemData { Id = "dagger", Name = "Dagger", Type = "Weapon", Weight = 1, Cost = 2, DamageDice = "1d4", DamageType = "Piercing", IsFinesse = true, IsLight = true },
            new ItemData { Id = "leather_armor", Name = "Leather Armor", Type = "Armor", Weight = 10, Cost = 10, ArmorClass = 11 },
            new ItemData { Id = "chain_mail", Name = "Chain Mail", Type = "Armor", Weight = 55, Cost = 75, ArmorClass = 16, StrengthRequirement = 13, HasStealthDisadvantage = true },
            new ItemData { Id = "shield", Name = "Shield", Type = "Shield", Weight = 6, Cost = 10, ArmorClass = 2 },
            new ItemData { Id = "health_potion", Name = "Potion of Healing", Type = "Potion", Weight = 1, Cost = 50 },
            new ItemData { Id = "backpack", Name = "Backpack", Type = "Container", Weight = 5, Cost = 2 },
            new ItemData { Id = "rope", Name = "Hempen Rope", Type = "Other", Weight = 10, Cost = 1 },
            new ItemData { Id = "torch", Name = "Torch", Type = "Other", Weight = 1, Cost = 1 },
            new ItemData { Id = "thieves_tools", Name = "Thieves' Tools", Type = "Tool", Weight = 1, Cost = 25 }
        };
    }

    #endregion
}