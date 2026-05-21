using System.Collections.Generic;

namespace DnD_Dynamics.Services
{
    public interface IGameDataService
    {
        List<SkillData> LoadSkills();

        List<SpellData> LoadSpells();

        List<ItemData> LoadItems();

        SkillData GetSkillById(string id);

        SpellData GetSpellById(string id);

        ItemData GetItemById(string id);

        List<SkillData> GetSkillsByAbility(CharacterAbility ability);

        List<SpellData> GetSpellsByLevel(int level);

        List<ItemData> GetItemsByType(string type);
    }
}