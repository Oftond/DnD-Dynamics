using DnD_Dynamics.MVP.Presenters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace DnD_Dynamics.Services
{
    public interface IDataService
    {
        Task<List<Spell>> GetSpellsAsync();

        Spell GetSpellById(string id);

        Task AddSpellAsync(Spell spell);

        Task UpdateSpellAsync(Spell spell);

        Task DeleteSpellAsync(string id);

        Task<List<Item>> GetItemsAsync();

        Item GetItemById(string id);

        Task AddItemAsync(Item item);

        Task UpdateItemAsync(Item item);

        Task DeleteItemAsync(string id);

        Task<List<Monster>> GetMonstersAsync();

        Monster GetMonsterById(string id);

        Task AddMonsterAsync(Monster monster);

        Task UpdateMonsterAsync(Monster monster);

        Task DeleteMonsterAsync(string id);

        Task<List<CharacterRace>> GetRacesAsync();

        CharacterRace GetRaceById(string id);

        Task AddRaceAsync(CharacterRace race);

        Task UpdateRaceAsync(CharacterRace race);

        Task DeleteRaceAsync(string id);

        Task<List<CharacterClass>> GetClassesAsync();

        CharacterClass GetClassById(string id);

        Task AddClassAsync(CharacterClass characterClass);

        Task UpdateClassAsync(CharacterClass characterClass);

        Task DeleteClassAsync(string id);

        Task<List<SkillData>> GetSkillsAsync();

        SkillData GetSkillById(string id);

        List<SkillData> GetSkillsByAbility(CharacterAbility ability);

        Task ToggleFavoriteAsync(string id, HandbookCategory category);

        Task<List<T>> GetFavoritesByCategoryAsync<T>(HandbookCategory category) where T : HandbookEntity;

        Task SaveCharactersAsync(List<CharacterData> characters);

        Task<List<CharacterData>> GetCharactersAsync();

        Task DeleteCharacter(string characterId);

        bool HasSavedCharacters();

        Task PreloadCategoryAsync(HandbookCategory category);

        Task PreloadAllAsync();

        bool IsSpellsLoaded { get; }
        bool IsItemsLoaded { get; }
        bool IsMonstersLoaded { get; }
        bool IsRacesLoaded { get; }
        bool IsClassesLoaded { get; }

        event Action OnSpellsLoaded;
        event Action OnItemsLoaded;
        event Action OnMonstersLoaded;
        event Action OnRacesLoaded;
        event Action OnClassesLoaded;
    }
}