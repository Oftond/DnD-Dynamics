using System.Collections.Generic;

namespace DnD_Dynamics.MVP.View
{
    public interface ICharacterDetailView : IBaseView
    {
        void DisplayCharacterDetails(CharacterUIData character);

        void ClearSelection();
    }
}