using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataService : IDataService
{
    private readonly string _dataPath;

    public DataService()
    {
        _dataPath = Path.Combine(Application.persistentDataPath, "GameData");
        if (!Directory.Exists(_dataPath))
        {
            Directory.CreateDirectory(_dataPath);
        }
    }

    public void Save<T>(string key, T data)
    {
        try
        {
            string filePath = GetFilePath(key);
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(filePath, json);
            Debug.Log($"Данные сохранены: {filePath}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Ошибка сохранения {key}: {ex.Message}");
        }
    }

    public T Load<T>(string key, T defaultValue = default)
    {
        try
        {
            string filePath = GetFilePath(key);
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonUtility.FromJson<T>(json);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Ошибка загрузки {key}: {ex.Message}");
        }

        return defaultValue;
    }

    public bool Exists(string key)
    {
        string filePath = GetFilePath(key);
        return File.Exists(filePath);
    }

    public void Delete(string key)
    {
        try
        {
            string filePath = GetFilePath(key);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Debug.Log($"Данные удалены: {filePath}");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Ошибка удаления {key}: {ex.Message}");
        }
    }

    public List<string> GetAllKeys()
    {
        var keys = new List<string>();
        try
        {
            if (Directory.Exists(_dataPath))
            {
                foreach (string file in Directory.GetFiles(_dataPath, "*.json"))
                {
                    keys.Add(Path.GetFileNameWithoutExtension(file));
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Ошибка получения ключей: {ex.Message}");
        }
        return keys;
    }

    private string GetFilePath(string key)
    {
        return Path.Combine(_dataPath, $"{key}.json");
    }
}