using System.Collections.Generic;
using UnityEngine;

public interface ICreateCharacterView : IBaseView
{
    void SetRaces(List<CharacterRace> races, List<string> raceIds);

    void SetClasses(List<CharacterClass> classes, List<string> classIds);

    void ClearError();
}