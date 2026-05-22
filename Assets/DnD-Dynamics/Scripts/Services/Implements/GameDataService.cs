using System;
using System.Collections.Generic;
using UnityEngine;

namespace DnD_Dynamics.Services
{
    public class GameDataService : IGameDataService
    {
        private List<SkillData> _skills;
        private List<SpellData> _spells;
        private List<ItemData> _items;

        public GameDataService()
        {
            LoadAllData();
        }

        private void LoadAllData()
        {
            LoadSkills();
            LoadSpells();
            LoadItems();
        }

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

            return _skills;
        }

        public List<SpellData> LoadSpells()
        {
            if (_spells != null && _spells.Count > 0)
                return _spells;

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

            return _spells;
        }

        public List<ItemData> LoadItems()
        {
            if (_items != null && _items.Count > 0)
                return _items;

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
    }
}