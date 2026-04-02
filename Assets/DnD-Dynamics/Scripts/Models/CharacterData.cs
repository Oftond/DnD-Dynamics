using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

[Serializable]
public class CharacterData
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public int Level { get; set; } = 1;
    public int ExperiencePoints { get; set; } = 0;

    public CharacterRace Race { get; set; } = CharacterRace.Human;
    public CharacterClass Class { get; set; } = CharacterClass.Fighter;

    public CharacterStats BaseStats { get; set; } = new CharacterStats();
    public CharacterStats BonusStats { get; set; } = new CharacterStats();

    public int CurrentHp { get; set; }
    public int TemporaryHp { get; set; }
    public int ArmorClass { get; set; }

    public int Gold { get; set; }
    public int Silver { get; set; }
    public int Copper { get; set; }

    public string PortraitPath { get; set; } = string.Empty;
    public string Backstory { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public CharacterStats TotalStats => CalculateTotalStats();

    public int MaxHp => CalculateMaxHp();

    public int ProficiencyBonus => Level switch
    {
        <= 4 => 2,
        <= 8 => 3,
        <= 12 => 4,
        <= 16 => 5,
        _ => 6
    };

    public int InitiativeBonus => TotalStats.GetModifier(CharacterAbility.Dexterity);

    private CharacterStats CalculateTotalStats()
    {
        var total = BaseStats.Clone();

        total.Strength += Race.GetStrengthBonus();
        total.Dexterity += Race.GetDexterityBonus();
        total.Constitution += Race.GetConstitutionBonus();
        total.Intelligence += Race.GetIntelligenceBonus();
        total.Wisdom += Race.GetWisdomBonus();
        total.Charisma += Race.GetCharismaBonus();

        total.Strength += BonusStats.Strength;
        total.Dexterity += BonusStats.Dexterity;
        total.Constitution += BonusStats.Constitution;
        total.Intelligence += BonusStats.Intelligence;
        total.Wisdom += BonusStats.Wisdom;
        total.Charisma += BonusStats.Charisma;

        total.Strength = Math.Clamp(total.Strength, 1, 30);
        total.Dexterity = Math.Clamp(total.Dexterity, 1, 30);
        total.Constitution = Math.Clamp(total.Constitution, 1, 30);
        total.Intelligence = Math.Clamp(total.Intelligence, 1, 30);
        total.Wisdom = Math.Clamp(total.Wisdom, 1, 30);
        total.Charisma = Math.Clamp(total.Charisma, 1, 30);

        return total;
    }

    private int CalculateMaxHp()
    {
        var conModifier = TotalStats.GetModifier(CharacterAbility.Constitution);
        var hitDice = Class.GetHitDice();

        if (Level == 1)
        {
            return hitDice + conModifier;
        }

        var averageHp = (hitDice / 2) + 1;
        return hitDice + conModifier + (Level - 1) * (averageHp + conModifier);
    }

    public int ApplyDamage(int amount)
    {
        amount = Math.Max(1, amount);

        if (TemporaryHp > 0)
        {
            var tempDamage = Math.Min(TemporaryHp, amount);
            TemporaryHp -= tempDamage;
            amount -= tempDamage;
        }

        if (amount > 0)
        {
            Debug.Log($"ТЕКУЩЕЕ ЗДОРОВЬЕ: {CurrentHp}");
            CurrentHp = Math.Max(0, CurrentHp - amount);
            Debug.Log($"ТЕКУЩЕЕ ЗДОРОВЬЕ: {CurrentHp}");
        }

        UpdatedAt = DateTime.Now;
        return CurrentHp;
    }

    public int ApplyHeal(int amount)
    {
        amount = Math.Max(1, amount);
        CurrentHp = Math.Min(MaxHp, CurrentHp + amount);
        UpdatedAt = DateTime.Now;
        return CurrentHp;
    }

    public void LevelUp()
    {
        if (Level >= 20) return;

        Level++;

        var oldMaxHp = MaxHp;
        CurrentHp = MaxHp;

        UpdatedAt = DateTime.Now;
    }

    public void AddExperience(int amount)
    {
        ExperiencePoints += amount;

        var expForNextLevel = CalculateExpForLevel(Level + 1);
        while (ExperiencePoints >= expForNextLevel && Level < 20)
        {
            LevelUp();
            expForNextLevel = CalculateExpForLevel(Level + 1);
        }
    }

    private static int CalculateExpForLevel(int level)
    {
        return level switch
        {
            2 => 300,
            3 => 900,
            4 => 2700,
            5 => 6500,
            6 => 14000,
            7 => 23000,
            8 => 34000,
            9 => 48000,
            10 => 64000,
            11 => 85000,
            12 => 100000,
            13 => 120000,
            14 => 140000,
            15 => 165000,
            16 => 195000,
            17 => 225000,
            18 => 265000,
            19 => 305000,
            20 => 355000,
            _ => int.MaxValue
        };
    }

    public CharacterUIData GetUIData()
    {
        return new CharacterUIData
        {
            Id = Id,
            Name = Name,
            ClassName = Class.GetDisplayName(),
            RaceName = Race.GetDisplayName(),
            Level = Level,
            CurrentHp = CurrentHp,
            MaxHp = MaxHp,
            ArmorClass = ArmorClass,
            InitiativeBonus = InitiativeBonus,
            Strength = TotalStats.Strength,
            Dexterity = TotalStats.Dexterity,
            Constitution = TotalStats.Constitution,
            Intelligence = TotalStats.Intelligence,
            Wisdom = TotalStats.Wisdom,
            Charisma = TotalStats.Charisma,
            StrengthModifier = TotalStats.GetModifier(CharacterAbility.Strength),
            DexterityModifier = TotalStats.GetModifier(CharacterAbility.Dexterity),
            ConstitutionModifier = TotalStats.GetModifier(CharacterAbility.Constitution),
            IntelligenceModifier = TotalStats.GetModifier(CharacterAbility.Intelligence),
            WisdomModifier = TotalStats.GetModifier(CharacterAbility.Wisdom),
            CharismaModifier = TotalStats.GetModifier(CharacterAbility.Charisma),
            ProficiencyBonus = ProficiencyBonus,
            Gold = Gold,
            Silver = Silver,
            Copper = Copper,
            PortraitPath = PortraitPath,
            Backstory = Backstory,
            Notes = Notes
        };
    }
}

[Serializable]
public class CharacterUIData
{
    public string Id;
    public string Name;
    public string ClassName;
    public string RaceName;
    public int Level;
    public int CurrentHp;
    public int MaxHp;
    public int ArmorClass;

    public int InitiativeBonus;

    public int Strength;
    public int Dexterity;
    public int Constitution;
    public int Intelligence;
    public int Wisdom;
    public int Charisma;

    public int StrengthModifier;
    public int DexterityModifier;
    public int ConstitutionModifier;
    public int IntelligenceModifier;
    public int WisdomModifier;
    public int CharismaModifier;

    public int ProficiencyBonus;

    public int Gold;
    public int Silver;
    public int Copper;

    public string PortraitPath;
    public string Backstory;
    public string Notes;

    public string HpText => $"{CurrentHp}/{MaxHp}";
    public string LevelText => $"Ур. {Level}";
    public string ClassRaceText => $"{ClassName} - {RaceName}";

    public string StrengthText => $"{Strength} ({StrengthModifier:+0;-0;0})";
    public string DexterityText => $"{Dexterity} ({DexterityModifier:+0;-0;0})";
    public string ConstitutionText => $"{Constitution} ({ConstitutionModifier:+0;-0;0})";
    public string IntelligenceText => $"{Intelligence} ({IntelligenceModifier:+0;-0;0})";
    public string WisdomText => $"{Wisdom} ({WisdomModifier:+0;-0;0})";
    public string CharismaText => $"{Charisma} ({CharismaModifier:+0;-0;0})";

    public string ArmorClassText => $"КД: {ArmorClass}";
    public string InitiativeText => $"Инициатива: {InitiativeBonus:+0;-0;0}";
    public string ProficiencyText => $"Бонус умения: +{ProficiencyBonus}";
}