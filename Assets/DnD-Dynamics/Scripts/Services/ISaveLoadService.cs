using System.Collections.Generic;

public interface ISaveLoadService
{
    void SaveCharacters(List<CharacterData> characters);

    List<CharacterData> LoadCharacters();

    void SaveCharacter(CharacterData character);

    void DeleteCharacter(string characterId);

    bool HasSavedData();
}