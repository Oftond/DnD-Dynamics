using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadService : ISaveLoadService
{
    private const string SAVE_FILE_NAME = "characters_data.dat";
    private string SavePath => Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);

    public void SaveCharacters(List<CharacterData> characters)
    {
        try
        {
            var json = JsonUtility.ToJson(new Wrapper<List<CharacterData>> { Items = characters }, true);
            File.WriteAllText(SavePath, json);
            Debug.Log($"Сохранено {characters.Count} персонажей в {SavePath}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Ошибка сохранения: {ex.Message}");
        }
    }

    public List<CharacterData> LoadCharacters()
    {
        try
        {
            if (!File.Exists(SavePath))
            {
                Debug.Log("Файл сохранения не найден");
                return new List<CharacterData>();
            }

            var json = File.ReadAllText(SavePath);
            var wrapper = JsonUtility.FromJson<Wrapper<List<CharacterData>>>(json);

            Debug.Log($"Загружено {wrapper?.Items?.Count ?? 0} персонажей");
            return wrapper?.Items ?? new List<CharacterData>();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Ошибка загрузки: {ex.Message}");
            return new List<CharacterData>();
        }
    }

    public void SaveCharacter(CharacterData character)
    {
        var characters = LoadCharacters();
        var existingIndex = characters.FindIndex(c => c.Id == character.Id);

        if (existingIndex >= 0)
        {
            characters[existingIndex] = character;
        }
        else
        {
            characters.Add(character);
        }

        SaveCharacters(characters);
    }

    public void DeleteCharacter(string characterId)
    {
        var characters = LoadCharacters();
        characters.RemoveAll(c => c.Id == characterId);
        SaveCharacters(characters);
    }

    public bool HasSavedData()
    {
        return File.Exists(SavePath);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T Items;
    }
}