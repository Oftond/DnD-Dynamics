using DnD_Dynamics.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        [NonSerialized] public List<string> _knownSpellIds = new();
        [NonSerialized] public List<string> _preparedSpellIds = new();
        [NonSerialized] public List<SpellSlot> _spellSlots = new();
        [NonSerialized] private List<Spell> _allSpells = new();

        [NonSerialized] private Task<List<Spell>> _loadingTask;
        [NonSerialized] private IDataService _dataService;

        public List<string> KnownSpellIds => _knownSpellIds;
        public List<string> PreparedSpellIds => _preparedSpellIds;
        public List<SpellSlot> SpellSlots => _spellSlots;

        public Spellbook() { }

        [Inject]
        public Spellbook(IDataService dataService)
        {
            _dataService = dataService;
            InitializeSpellSlots();
        }

        public void Initialize(IDataService dataService)
        {
            _dataService = dataService;

            if (_spellSlots.Count == 0)
                InitializeSpellSlots();
        }

        private void InitializeSpellSlots()
        {
            for (int i = 0; i <= 9; i++)
            {
                _spellSlots.Add(new SpellSlot
                {
                    Level = i,
                    MaxSlots = 0,
                    UsedSlots = 0
                });
            }
        }

        private async Task<List<Spell>> GetSpellsAsync()
        {
            if (_allSpells != null)
                return _allSpells;

            if (_loadingTask != null)
                return await _loadingTask;

            _loadingTask = Task.Run(async () =>
            {
                if (_dataService != null)
                    return await _dataService.GetSpellsAsync();
                return new List<Spell>();
            });

            _allSpells = await _loadingTask;

            return _allSpells;
        }

        private List<Spell> GetSpellsData() => _dataService.GetSpellsAsync().ConfigureAwait(false).GetAwaiter().GetResult();

        public async Task<Spell> GetSpellByIdAsync(string id)
        {
            var spells = await GetSpellsAsync();

            return spells.Find(s => s.Id == id);
        }

        public Spell GetSpellById(string id) => GetSpellByIdAsync(id).ConfigureAwait(false).GetAwaiter().GetResult();

        public async Task<List<Spell>> GetAllKnownSpellsAsync()
        {
            var allSpells = await GetSpellsAsync();
            var result = new List<Spell>();

            foreach (var id in _knownSpellIds)
            {
                var spell = allSpells.Find(s => s.Id == id);

                if (spell != null)
                    result.Add(spell);
            }

            return result;
        }

        public async Task<List<Spell>> GetPreparedSpellsAsync()
        {
            var allSpells = await GetSpellsAsync();
            var result = new List<Spell>();

            foreach (var id in _preparedSpellIds)
            {
                var spell = allSpells.Find(s => s.Id == id);
                if (spell != null)
                    result.Add(spell);
            }

            return result;
        }

        public List<Spell> GetPreparedSpells() => GetPreparedSpellsAsync().ConfigureAwait(false).GetAwaiter().GetResult();

        public async Task<List<Spell>> GetSpellsByLevelAsync(int level) => (await GetAllKnownSpellsAsync()).FindAll(s => (int)s.Level == level);

        public List<Spell> GetSpellsByLevel(int level) => GetSpellsByLevelAsync(level).ConfigureAwait(false).GetAwaiter().GetResult();

        public void AddSpell(string spellId)
        {
            if (!_knownSpellIds.Contains(spellId))
                _knownSpellIds.Add(spellId);
        }

        public void RemoveSpell(string spellId)
        {
            _knownSpellIds.Remove(spellId);
            _preparedSpellIds.Remove(spellId);
        }

        public void PrepareSpell(string spellId)
        {
            if (_knownSpellIds.Contains(spellId) && !_preparedSpellIds.Contains(spellId))
                _preparedSpellIds.Add(spellId);
        }

        public void UnprepareSpell(string spellId) => _preparedSpellIds.Remove(spellId);

        public void UseSpellSlot(int level)
        {
            var slot = _spellSlots.Find(s => s.Level == level);
            slot?.UseSlot();
        }

        public void RestoreSpellSlot(int level)
        {
            var slot = _spellSlots.Find(s => s.Level == level);
            slot?.RestoreSlot();
        }

        public void RestoreAllSpellSlots()
        {
            foreach (var slot in _spellSlots)
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
                    if (i < _spellSlots.Count)
                    {
                        _spellSlots[i].MaxSlots = slots[i];
                        _spellSlots[i].UsedSlots = Math.Min(_spellSlots[i].UsedSlots, slots[i]);
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

            return _preparedSpellIds.Count < maxPrepared;
        }

        public int GetMaxPreparedSpells(int characterLevel, int spellcastingModifier) => Math.Max(1, characterLevel + spellcastingModifier);
    }
}