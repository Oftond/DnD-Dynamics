using System;

[Serializable]
public enum CharacterRace
{
    Human = 1,
    Elf = 2,
    Dwarf = 3,
    Halfling = 4,
    Gnome = 5,
    HalfElf = 6,
    HalfOrc = 7,
    Tiefling = 8,
    Dragonborn = 9
}

public static class CharacterRaceExtensions
{
    public static string GetDisplayName(this CharacterRace race)
    {
        return race switch
        {
            CharacterRace.Human => "Человек",
            CharacterRace.Elf => "Эльф",
            CharacterRace.Dwarf => "Дворф",
            CharacterRace.Halfling => "Полурослик",
            CharacterRace.Gnome => "Гном",
            CharacterRace.HalfElf => "Полуэльф",
            CharacterRace.HalfOrc => "Полуорк",
            CharacterRace.Tiefling => "Тифлинг",
            CharacterRace.Dragonborn => "Драконорожденный",
            _ => "Неизвестно"
        };
    }

    public static int GetStrengthBonus(this CharacterRace race)
    {
        return race switch
        {
            CharacterRace.Dwarf => 2,
            CharacterRace.HalfOrc => 2,
            CharacterRace.Dragonborn => 2,
            _ => 0
        };
    }

    public static int GetDexterityBonus(this CharacterRace race)
    {
        return race switch
        {
            CharacterRace.Elf => 2,
            CharacterRace.Halfling => 2,
            _ => 0
        };
    }

    public static int GetConstitutionBonus(this CharacterRace race)
    {
        return race switch
        {
            CharacterRace.Dwarf => 2,
            CharacterRace.Gnome => 1,
            _ => 0
        };
    }

    public static int GetIntelligenceBonus(this CharacterRace race)
    {
        return race switch
        {
            CharacterRace.Gnome => 2,
            CharacterRace.Human => 1,
            _ => 0
        };
    }

    public static int GetWisdomBonus(this CharacterRace race)
    {
        return race switch
        {
            CharacterRace.HalfElf => 1,
            CharacterRace.Human => 1,
            _ => 0
        };
    }

    public static int GetCharismaBonus(this CharacterRace race)
    {
        return race switch
        {
            CharacterRace.Tiefling => 2,
            CharacterRace.HalfElf => 2,
            CharacterRace.Dragonborn => 1,
            _ => 0
        };
    }
}