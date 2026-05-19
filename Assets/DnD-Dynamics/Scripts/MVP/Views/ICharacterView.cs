using System.Collections.Generic;

namespace DnD_Dynamics.MVP.View
{
    public interface ICharacterView
    {
        void DisplayCharacters(List<CharacterUIData> characters);

        void DisplayCharacterDetails(CharacterUIData character);

        void ShowError(string message);

        void ShowSuccess(string message);

        void ShowLoading(bool show);

        void ClearSelection();
    }
}