using DnD_Dynamics.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace DnD_Dynamics.Models
{
    [Serializable]
    public class SpellSlot
    {
        public int Level;
        public int MaxSlots;
        public int UsedSlots;

        public int AvailableSlots => MaxSlots - UsedSlots;

        public void UseSlot()
        {
            if (UsedSlots < MaxSlots)
                UsedSlots++;
        }

        public void RestoreSlot()
        {
            if (UsedSlots > 0)
                UsedSlots--;
        }

        public void RestoreAll() => UsedSlots = 0;

        public SpellSlot Clone()
        {
            return new SpellSlot
            {
                Level = Level,
                MaxSlots = MaxSlots,
                UsedSlots = UsedSlots
            };
        }
    }

    [Serializable]
    public class Spellbook
    {
        public List<string> KnownSpellIds = new List<string>();
        public List<string> PreparedSpellIds = new List<string>();
        public List<SpellSlot> SpellSlots = new List<SpellSlot>();

        [NonSerialized] private List<Spell> _spellsCache;

        [NonSerialized] private IHandbookDataService _handbookDataService;

        public Spellbook() { }

        [Inject]
        public Spellbook(IHandbookDataService handbookDataService)
        {
            _handbookDataService = handbookDataService;
            InitializeSpellSlots();
        }

        public void Initialize(IHandbookDataService handbookDataService)
        {
            _handbookDataService = handbookDataService;

            if (SpellSlots.Count == 0)
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

        private List<Spell> GetSpellsData()
        {
            if (_spellsCache == null && _handbookDataService != null)
            {
                _spellsCache = _handbookDataService.GetAllSpells();
            }

            return _spellsCache ?? new List<Spell>();
        }

        public Spell GetSpellById(string id)
        {
            var spells = GetSpellsData();

            return spells.Find(s => s.Id == id);
        }

        public List<Spell> GetAllKnownSpells()
        {
            var allSpells = GetSpellsData();
            var result = new List<Spell>();

            foreach (var id in KnownSpellIds)
            {
                var spell = allSpells.Find(s => s.Id == id);
                if (spell != null)
                    result.Add(spell);
            }

            return result;
        }

        public List<Spell> GetPreparedSpells()
        {
            var allSpells = GetSpellsData();
            var result = new List<Spell>();

            foreach (var id in PreparedSpellIds)
            {
                var spell = allSpells.Find(s => s.Id == id);
                if (spell != null)
                    result.Add(spell);
            }

            return result;
        }

        public List<Spell> GetSpellsByLevel(int level) => GetAllKnownSpells().FindAll(s => (int)s.Level == level);

        public void AddSpell(string spellId)
        {
            if (!KnownSpellIds.Contains(spellId))
                KnownSpellIds.Add(spellId);
        }

        public void RemoveSpell(string spellId)
        {
            KnownSpellIds.Remove(spellId);
            PreparedSpellIds.Remove(spellId);
        }

        public void PrepareSpell(string spellId)
        {
            if (KnownSpellIds.Contains(spellId) && !PreparedSpellIds.Contains(spellId))
                PreparedSpellIds.Add(spellId);
        }

        public void UnprepareSpell(string spellId) => PreparedSpellIds.Remove(spellId);

        public void UseSpellSlot(int level)
        {
            var slot = SpellSlots.Find(s => s.Level == level);
            slot?.UseSlot();
        }

        public void RestoreSpellSlot(int level)
        {
            var slot = SpellSlots.Find(s => s.Level == level);
            slot?.RestoreSlot();
        }

        public void RestoreAllSpellSlots()
        {
            foreach (var slot in SpellSlots)
                slot.RestoreAll();
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

            if (characterClass == null)
                return;

            bool isFullCaster = IsFullCaster(characterClass);
            bool isHalfCaster = IsHalfCaster(characterClass);
            bool isThirdCaster = IsThirdCaster(characterClass);

            int casterLevel = characterLevel;

            if (isHalfCaster)
                casterLevel = (int)Math.Ceiling(characterLevel / 2.0);
            else if (isThirdCaster)
                casterLevel = (int)Math.Ceiling(characterLevel / 3.0);

            casterLevel = Math.Clamp(casterLevel, 1, 20);

            if (fullCasterSlots.TryGetValue(casterLevel, out var slots))
            {
                for (int i = 0; i < slots.Length; i++)
                {
                    if (i < SpellSlots.Count)
                    {
                        SpellSlots[i].MaxSlots = slots[i];
                        SpellSlots[i].UsedSlots = Math.Min(SpellSlots[i].UsedSlots, slots[i]);
                    }
                }
            }
        }

        private bool IsFullCaster(CharacterClass characterClass) => characterClass?.CasterType == CasterType.FullCaster;

        private bool IsHalfCaster(CharacterClass characterClass) => characterClass?.CasterType == CasterType.HalfCaster;

        private bool IsThirdCaster(CharacterClass characterClass) => characterClass?.CasterType == CasterType.ThirdCaster;

        public bool CanPrepareSpells(int characterLevel, int spellcastingModifier)
        {
            int maxPrepared = characterLevel + spellcastingModifier;

            return PreparedSpellIds.Count < maxPrepared;
        }

        public int GetMaxPreparedSpells(int characterLevel, int spellcastingModifier) => Math.Max(1, characterLevel + spellcastingModifier);

        public Spellbook Clone()
        {
            var clone = new Spellbook();
            clone.KnownSpellIds = new List<string>(KnownSpellIds);
            clone.PreparedSpellIds = new List<string>(PreparedSpellIds);

            for (int i = 0; i < SpellSlots.Count && i < clone.SpellSlots.Count; i++)
            {
                clone.SpellSlots[i] = SpellSlots[i].Clone();
            }

            return clone;
        }
    }
}