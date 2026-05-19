using DnD_Dynamics.MVP.Presenters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace DnD_Dynamics.Services
{
    public interface IDataService
    {
        List<Spell> GetAllSpells();

        Spell GetSpellById(string id);

        void AddSpell(Spell spell);

        void UpdateSpell(Spell spell);

        void DeleteSpell(string id);

        List<Item> GetAllItems();

        Item GetItemById(string id);

        void AddItem(Item item);

        void UpdateItem(Item item);

        void DeleteItem(string id);

        List<Monster> GetAllMonsters();

        Monster GetMonsterById(string id);

        void AddMonster(Monster monster);

        void UpdateMonster(Monster monster);

        void DeleteMonster(string id);

        List<CharacterRace> GetAllRaces();

        CharacterRace GetRaceById(string id);

        void AddRace(CharacterRace race);

        void UpdateRace(CharacterRace race);

        void DeleteRace(string id);

        List<CharacterClass> GetAllClasses();

        CharacterClass GetClassById(string id);

        void AddClass(CharacterClass characterClass);

        void UpdateClass(CharacterClass characterClass);

        void DeleteClass(string id);

        List<SkillData> GetAllSkills();

        SkillData GetSkillById(string id);

        List<SkillData> GetSkillsByAbility(CharacterAbility ability);

        void ToggleFavorite(string id, HandbookCategory category);

        List<T> GetFavoritesByCategory<T>(HandbookCategory category) where T : HandbookEntity;

        void SaveCharacters(List<CharacterData> characters);

        List<CharacterData> LoadCharacters();

        void SaveCharacter(CharacterData character);

        void DeleteCharacter(string characterId);

        bool HasSavedCharacters();
    }
}