using System.Collections.Generic;
using System.Linq;

namespace DnD_Dynamics.Services
{
    public class HandbookFilterService : IHandbookFilterService
    {
        public List<Spell> FilterSpells(List<Spell> spells, int? level, SpellSchool? school, string requiredClassId)
        {
            var result = spells.AsEnumerable();

            if (level.HasValue)
                result = result.Where(s => (int)s.Level == level.Value);

            if (school.HasValue)
                result = result.Where(s => s.School == school.Value);

            if (!string.IsNullOrEmpty(requiredClassId))
                result = result.Where(s => s.AvailableClassIds.Contains(requiredClassId));

            return result.ToList();
        }

        public List<Item> FilterItems(List<Item> items, List<ItemRarity> rarities, List<ItemType> types)
        {
            var result = items.AsEnumerable();
            if (rarities != null && rarities.Any()) result = result.Where(i => rarities.Contains(i.Rarity));
            if (types != null && types.Any()) result = result.Where(i => types.Contains(i.Type));

            return result.ToList();
        }

        public List<Monster> FilterMonsters(List<Monster> monsters, float? minCr, float? maxCr, MonsterType? type, MonsterSize? size)
        {
            var result = monsters.AsEnumerable();
            if (minCr.HasValue) result = result.Where(m => m.ChallengeRating >= minCr.Value);
            if (maxCr.HasValue) result = result.Where(m => m.ChallengeRating <= maxCr.Value);
            if (type.HasValue) result = result.Where(m => m.Type == type.Value);
            if (size.HasValue) result = result.Where(m => m.Size == size.Value);

            return result.ToList();
        }

        public List<T> SearchByName<T>(List<T> items, string query) where T : HandbookEntity
        {
            if (string.IsNullOrWhiteSpace(query)) return items;
            query = query.ToLowerInvariant();

            return items.Where(i => i.Name.ToLowerInvariant().Contains(query) || i.NameEng.ToLowerInvariant().Contains(query)).ToList();
        }
    }
}