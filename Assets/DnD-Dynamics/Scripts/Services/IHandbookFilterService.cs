using System.Collections.Generic;

namespace DnD_Dynamics.Services
{
    public interface IHandbookFilterService
    {
        List<Spell> FilterSpells(List<Spell> spells, int? level, SpellSchool? school, string requiredClassId);

        List<Item> FilterItems(List<Item> items, List<ItemRarity> rarities, List<ItemType> types);

        List<Monster> FilterMonsters(List<Monster> monsters, float? minCr, float? maxCr, MonsterType? type, MonsterSize? size);

        List<T> SearchByName<T>(List<T> items, string query) where T : HandbookEntity;
    }
}