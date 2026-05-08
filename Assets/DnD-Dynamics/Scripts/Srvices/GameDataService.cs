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

    private List<SkillData> _skillsCache;
    private List<SpellData> _spellsCache;
    private List<ItemData> _itemsCache;

    public List<SkillData> LoadSkills()
    {
        if (_skillsCache != null && _skillsCache.Count > 0)
        {
            return _skillsCache;
        }

        try
        {
            TextAsset skillDataFile = Resources.Load<TextAsset>("Data/skills");
            if (skillDataFile != null)
            {
                var skillDataList = JsonUtility.FromJson<SkillDataList>(skillDataFile.text);
                _skillsCache = skillDataList.Skills;
                Debug.Log($"Загружено {_skillsCache.Count} навыков из JSON");
                return _skillsCache;
            }

            Debug.LogWarning("Файл skills.json не найден в Resources/Data");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Ошибка загрузки навыков: {ex.Message}");
        }

        _skillsCache = GetDefaultSkills();
        return _skillsCache;
    }

    public List<SpellData> LoadSpells()
    {
        if (_spellsCache != null && _spellsCache.Count > 0)
        {
            return _spellsCache;
        }

        try
        {
            TextAsset spellDataFile = Resources.Load<TextAsset>("Data/spells");
            if (spellDataFile != null)
            {
                var spellDataList = JsonUtility.FromJson<SpellDataList>(spellDataFile.text);
                _spellsCache = spellDataList.Spells;
                Debug.Log($"Загружено {_spellsCache.Count} заклинаний из JSON");
                return _spellsCache;
            }

            Debug.LogWarning("Файл spells.json не найден в Resources/Data");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Ошибка загрузки заклинаний: {ex.Message}");
        }

        _spellsCache = GetDefaultSpells();
        return _spellsCache;
    }

    public List<ItemData> LoadItems()
    {
        if (_itemsCache != null && _itemsCache.Count > 0)
        {
            return _itemsCache;
        }

        try
        {
            TextAsset itemDataFile = Resources.Load<TextAsset>("Data/items");
            if (itemDataFile != null)
            {
                var itemDataList = JsonUtility.FromJson<ItemDataList>(itemDataFile.text);
                _itemsCache = itemDataList.Items;
                Debug.Log($"Загружено {_itemsCache.Count} предметов из JSON");
                return _itemsCache;
            }

            Debug.LogWarning("Файл items.json не найден в Resources/Data");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Ошибка загрузки предметов: {ex.Message}");
        }

        _itemsCache = GetDefaultItems();
        return _itemsCache;
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
            new SkillData { Id = "acrobatics", Name = "Acrobatics", NameRu = "Акробатика", AssociatedAbility = CharacterAbility.Dexterity },
            new SkillData { Id = "animal_handling", Name = "Animal Handling", NameRu = "Обращение с животными", AssociatedAbility = CharacterAbility.Wisdom },
            new SkillData { Id = "arcana", Name = "Arcana", NameRu = "Магия", AssociatedAbility = CharacterAbility.Intelligence },
            new SkillData { Id = "athletics", Name = "Athletics", NameRu = "Атлетика", AssociatedAbility = CharacterAbility.Strength },
            new SkillData { Id = "deception", Name = "Deception", NameRu = "Обман", AssociatedAbility = CharacterAbility.Charisma },
            new SkillData { Id = "history", Name = "History", NameRu = "История", AssociatedAbility = CharacterAbility.Intelligence },
            new SkillData { Id = "insight", Name = "Insight", NameRu = "Проницательность", AssociatedAbility = CharacterAbility.Wisdom },
            new SkillData { Id = "intimidation", Name = "Intimidation", NameRu = "Запугивание", AssociatedAbility = CharacterAbility.Charisma },
            new SkillData { Id = "investigation", Name = "Investigation", NameRu = "Анализ", AssociatedAbility = CharacterAbility.Intelligence },
            new SkillData { Id = "medicine", Name = "Medicine", NameRu = "Медицина", AssociatedAbility = CharacterAbility.Wisdom },
            new SkillData { Id = "nature", Name = "Nature", NameRu = "Природа", AssociatedAbility = CharacterAbility.Intelligence },
            new SkillData { Id = "perception", Name = "Perception", NameRu = "Внимательность", AssociatedAbility = CharacterAbility.Wisdom },
            new SkillData { Id = "performance", Name = "Performance", NameRu = "Выступление", AssociatedAbility = CharacterAbility.Charisma },
            new SkillData { Id = "persuasion", Name = "Persuasion", NameRu = "Убеждение", AssociatedAbility = CharacterAbility.Charisma },
            new SkillData { Id = "religion", Name = "Religion", NameRu = "Религия", AssociatedAbility = CharacterAbility.Intelligence },
            new SkillData { Id = "sleight_of_hand", Name = "Sleight of Hand", NameRu = "Ловкость рук", AssociatedAbility = CharacterAbility.Dexterity },
            new SkillData { Id = "stealth", Name = "Stealth", NameRu = "Скрытность", AssociatedAbility = CharacterAbility.Dexterity },
            new SkillData { Id = "survival", Name = "Survival", NameRu = "Выживание", AssociatedAbility = CharacterAbility.Wisdom }
        };
    }

    private List<SpellData> GetDefaultSpells()
    {
        return new List<SpellData>
        {
            new SpellData { Id = "fire_bolt", Name = "Fire Bolt", NameRu = "Огненный снаряд", Level = 0, School = "Evocation", SchoolRu = "Воплощение", DamageDice = "1d10", DamageType = "Fire" },
            new SpellData { Id = "magic_missile", Name = "Magic Missile", NameRu = "Волшебная стрела", Level = 1, School = "Evocation", SchoolRu = "Воплощение", DamageDice = "1d4+1", DamageType = "Force" },
            new SpellData { Id = "cure_wounds", Name = "Cure Wounds", NameRu = "Лечение ран", Level = 1, School = "Evocation", SchoolRu = "Воплощение" },
            new SpellData { Id = "shield", Name = "Shield", NameRu = "Щит", Level = 1, School = "Abjuration", SchoolRu = "Ограждение" },
            new SpellData { Id = "detect_magic", Name = "Detect Magic", NameRu = "Обнаружение магии", Level = 1, School = "Divination", SchoolRu = "Предсказание", IsRitual = true }
        };
    }

    private List<ItemData> GetDefaultItems()
    {
        return new List<ItemData>
        {
            new ItemData { Id = "longsword", Name = "Longsword", NameRu = "Длинный меч", Type = "Weapon", Weight = 3, Cost = 15, DamageDice = "1d8", DamageType = "Slashing", IsVersatile = true, VersatileDamage = "1d10" },
            new ItemData { Id = "dagger", Name = "Dagger", NameRu = "Кинжал", Type = "Weapon", Weight = 1, Cost = 2, DamageDice = "1d4", DamageType = "Piercing", IsFinesse = true, IsLight = true },
            new ItemData { Id = "leather_armor", Name = "Leather Armor", NameRu = "Кожаная броня", Type = "Armor", Weight = 10, Cost = 10, ArmorClass = 11 },
            new ItemData { Id = "chain_mail", Name = "Chain Mail", NameRu = "Кольчуга", Type = "Armor", Weight = 55, Cost = 75, ArmorClass = 16, StrengthRequirement = 13, HasStealthDisadvantage = true },
            new ItemData { Id = "shield", Name = "Shield", NameRu = "Щит", Type = "Shield", Weight = 6, Cost = 10, ArmorClass = 2 },
            new ItemData { Id = "health_potion", Name = "Potion of Healing", NameRu = "Зелье лечения", Type = "Potion", Weight = 1, Cost = 50 },
            new ItemData { Id = "backpack", Name = "Backpack", NameRu = "Рюкзак", Type = "Container", Weight = 5, Cost = 2 },
            new ItemData { Id = "rope", Name = "Hempen Rope", NameRu = "Пеньковая веревка", Type = "Other", Weight = 10, Cost = 1 },
            new ItemData { Id = "torch", Name = "Torch", NameRu = "Факел", Type = "Other", Weight = 1, Cost = 1 },
            new ItemData { Id = "thieves_tools", Name = "Thieves' Tools", NameRu = "Воровские инструменты", Type = "Tool", Weight = 1, Cost = 25 }
        };
    }

    #endregion
}