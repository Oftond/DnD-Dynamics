using System.Collections.Generic;
using UnityEngine;

public static class SpellManager
{
    private static List<SpellData> _allSpellsData;

    public static List<SpellData> GetAllSpellsData()
    {
        if (_allSpellsData == null || _allSpellsData.Count == 0)
        {
            _allSpellsData = GameDataService.Instance.LoadSpells();
        }
        return _allSpellsData;
    }

    public static SpellData GetSpellDataById(string id)
    {
        var spells = GetAllSpellsData();
        return spells.Find(s => s.Id == id);
    }

    public static List<SpellData> GetSpellsByLevel(int level) => GameDataService.Instance.GetSpellsByLevel(level);

    public static List<SpellData> GetSpellsBySchool(string school)
    {
        var spells = GetAllSpellsData();
        return spells.FindAll(s => s.School == school);
    }
}