using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Overlays;
using UnityEngine;

public class SaveLoadService : ISaveLoadService
{
    private const string SAVE_FILE_NAME = "characters_data.dat";
    private string SavePath => Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);

    public void SaveCharacters(List<CharacterData> characters)
    {
        try
        {
            var saveData = new SaveData();

            foreach (var character in characters)
            {
                saveData.characters.Add(SerializableCharacterData.FromCharacter(character));
            }

            saveData.saveDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            foreach (var character in characters)
            {
                Debug.Log($"  Saving: ID={character.Id}, Name={character.Name}, Level={character.Level}, HP={character.CurrentHp}/{character.MaxHp}");
            }

            string json = JsonUtility.ToJson(saveData, true);

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
            var saveData = JsonUtility.FromJson<SaveData>(json);

            if (saveData?.characters == null)
                return new List<CharacterData>();

            var characters = new List<CharacterData>();
            foreach (var serializable in saveData.characters)
            {
                characters.Add(serializable.ToCharacter());
            }

            foreach (var character in characters)
            {
                Debug.Log($"  Loaded: ID={character.Id}, Name={character.Name}, Level={character.Level}, HP={character.CurrentHp}/{character.MaxHp}");
            }

            return characters;
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

    public void ClearSave()
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
            Debug.Log($"Save file deleted: {SavePath}");
        }
    }
}