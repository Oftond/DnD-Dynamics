using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public List<SerializableCharacterData> characters;
    public int version;
    public string saveDate;

    public SaveData()
    {
        characters = new List<SerializableCharacterData>();
        version = 1;
        saveDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
}