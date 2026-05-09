using System;
using System.Collections.Generic;

[Serializable]
public class Spell
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Level { get; set; } = 0;
    public string School { get; set; } = string.Empty;
    public int CastingTime { get; set; } = 1;
    public string Range { get; set; } = "30 feet";
    public string Components { get; set; } = "V, S";
    public string Duration { get; set; } = "Instantaneous";
    public string Description { get; set; } = string.Empty;
    public bool IsRitual { get; set; } = false;
    public bool IsPrepared { get; set; } = false;

    public string DamageDice { get; set; } = string.Empty;
    public string DamageType { get; set; } = string.Empty;
    public string SaveAbility { get; set; } = string.Empty;

    private SpellData _data;

    public void SetData(SpellData data)
    {
        _data = data;
        Id = data.Id;
        Name = data.Name;
        Level = data.Level;
        School = data.School;
        CastingTime = data.CastingTime;
        Range = data.Range;
        Components = data.Components;
        Duration = data.Duration;
        Description = data.Description;
        IsRitual = data.IsRitual;
        DamageDice = data.DamageDice;
        DamageType = data.DamageType;
        SaveAbility = data.SaveAbility;
    }

    public string GetLevelDisplayName()
    {
        return Level switch
        {
            0 => "Заговор",
            1 => "1 круг",
            2 => "2 круг",
            3 => "3 круг",
            4 => "4 круг",
            5 => "5 круг",
            6 => "6 круг",
            7 => "7 круг",
            8 => "8 круг",
            9 => "9 круг",
            _ => "Неизвестно"
        };
    }
}

[Serializable]
public class SpellSlot
{
    public int Level { get; set; }
    public int MaxSlots { get; set; }
    public int UsedSlots { get; set; }

    public int AvailableSlots => MaxSlots - UsedSlots;

    public void UseSlot()
    {
        if (UsedSlots < MaxSlots)
        {
            UsedSlots++;
        }
    }

    public void RestoreSlot()
    {
        if (UsedSlots > 0)
        {
            UsedSlots--;
        }
    }

    public void RestoreAll()
    {
        UsedSlots = 0;
    }
}

[Serializable]
public class Spellbook
{
    public List<string> KnownSpellIds { get; set; } = new List<string>();
    public List<SpellSlot> SpellSlots { get; set; } = new List<SpellSlot>();
    public List<string> PreparedSpellIds { get; set; } = new List<string>();

    private List<SpellData> _spellsCache;

    public Spellbook()
    {
        InitializeSpellSlots();
    }

    private void InitializeSpellSlots()
    {
        for (int i = 0; i <= 9; i++)
        {
            SpellSlots.Add(new SpellSlot
            {
                Level = i,
                MaxSlots = 0,
                UsedSlots = 0
            });
        }
    }

    private List<SpellData> GetSpellsData()
    {
        if (_spellsCache == null)
        {
            _spellsCache = GameDataService.Instance.LoadSpells();
        }
        return _spellsCache;
    }

    public SpellData GetSpellDataById(string id)
    {
        var spells = GetSpellsData();
        return spells.Find(s => s.Id == id);
    }

    public List<SpellData> GetAllKnownSpells()
    {
        var spells = GetSpellsData();
        var result = new List<SpellData>();
        foreach (var id in KnownSpellIds)
        {
            var spell = spells.Find(s => s.Id == id);
            if (spell != null)
            {
                result.Add(spell);
            }
        }
        return result;
    }

    public List<SpellData> GetPreparedSpells()
    {
        var spells = GetSpellsData();
        var result = new List<SpellData>();
        foreach (var id in PreparedSpellIds)
        {
            var spell = spells.Find(s => s.Id == id);
            if (spell != null)
            {
                result.Add(spell);
            }
        }
        return result;
    }

    public void AddSpell(string spellId)
    {
        if (!KnownSpellIds.Contains(spellId))
        {
            KnownSpellIds.Add(spellId);
        }
    }

    public void RemoveSpell(string spellId)
    {
        KnownSpellIds.Remove(spellId);
        PreparedSpellIds.Remove(spellId);
    }

    public void PrepareSpell(string spellId)
    {
        if (KnownSpellIds.Contains(spellId) && !PreparedSpellIds.Contains(spellId))
        {
            PreparedSpellIds.Add(spellId);
        }
    }

    public void UnprepareSpell(string spellId)
    {
        PreparedSpellIds.Remove(spellId);
    }

    public void UseSpellSlot(int level)
    {
        var slot = SpellSlots.Find(s => s.Level == level);
        if (slot != null)
        {
            slot.UseSlot();
        }
    }

    public void RestoreAllSpellSlots()
    {
        foreach (var slot in SpellSlots)
        {
            slot.RestoreAll();
        }
    }

    public List<SpellData> GetSpellsByLevel(int level)
    {
        var allSpells = GetAllKnownSpells();
        return allSpells.FindAll(s => s.Level == level);
    }

    public void UpdateSpellSlotsForLevel(int characterLevel, CharacterClass characterClass)
    {
        var fullCasterSlots = new Dictionary<int, int[]>
        {
            { 1, new[] { 0, 2, 0, 0, 0, 0, 0, 0, 0, 0 } },
            { 2, new[] { 0, 3, 0, 0, 0, 0, 0, 0, 0, 0 } },
            { 3, new[] { 0, 4, 2, 0, 0, 0, 0, 0, 0, 0 } },
            { 4, new[] { 0, 4, 3, 0, 0, 0, 0, 0, 0, 0 } },
            { 5, new[] { 0, 4, 3, 2, 0, 0, 0, 0, 0, 0 } },
            { 6, new[] { 0, 4, 3, 3, 0, 0, 0, 0, 0, 0 } },
            { 7, new[] { 0, 4, 3, 3, 1, 0, 0, 0, 0, 0 } },
            { 8, new[] { 0, 4, 3, 3, 2, 0, 0, 0, 0, 0 } },
            { 9, new[] { 0, 4, 3, 3, 3, 1, 0, 0, 0, 0 } },
            { 10, new[] { 0, 4, 3, 3, 3, 2, 0, 0, 0, 0 } },
            { 11, new[] { 0, 4, 3, 3, 3, 2, 1, 0, 0, 0 } },
            { 12, new[] { 0, 4, 3, 3, 3, 2, 1, 0, 0, 0 } },
            { 13, new[] { 0, 4, 3, 3, 3, 2, 1, 1, 0, 0 } },
            { 14, new[] { 0, 4, 3, 3, 3, 2, 1, 1, 0, 0 } },
            { 15, new[] { 0, 4, 3, 3, 3, 2, 1, 1, 1, 0 } },
            { 16, new[] { 0, 4, 3, 3, 3, 2, 1, 1, 1, 0 } },
            { 17, new[] { 0, 4, 3, 3, 3, 2, 1, 1, 1, 1 } },
            { 18, new[] { 0, 4, 3, 3, 3, 3, 1, 1, 1, 1 } },
            { 19, new[] { 0, 4, 3, 3, 3, 3, 2, 1, 1, 1 } },
            { 20, new[] { 0, 4, 3, 3, 3, 3, 2, 2, 1, 1 } }
        };

        bool isFullCaster = characterClass switch
        {
            CharacterClass.Bard => true,
            CharacterClass.Cleric => true,
            CharacterClass.Druid => true,
            CharacterClass.Sorcerer => true,
            CharacterClass.Wizard => true,
            _ => false
        };

        if (!isFullCaster)
            characterLevel = Math.Max(0, characterLevel - 1);

        characterLevel = Math.Clamp(characterLevel, 1, 20);

        if (fullCasterSlots.TryGetValue(characterLevel, out var slots))
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (i < SpellSlots.Count)
                {
                    var maxSlots = isFullCaster ? slots[i] : (i > 0 && characterLevel >= 2 ? slots[i] / 2 : 0);
                    SpellSlots[i].MaxSlots = maxSlots;
                    SpellSlots[i].UsedSlots = Math.Min(SpellSlots[i].UsedSlots, maxSlots);
                }
            }
        }
    }
}