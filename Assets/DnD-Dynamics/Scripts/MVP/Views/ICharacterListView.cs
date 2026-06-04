using System.Collections.Generic;

public interface ICharacterListView : IBaseView
{
    void DisplayCharacters(List<CharacterUIData> characters);

    void DisplayCharacterDetails(CharacterUIData character);

    void ClearSelection();
}