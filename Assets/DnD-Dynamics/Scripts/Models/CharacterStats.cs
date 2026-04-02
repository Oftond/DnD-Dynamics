using System;

[Serializable]
public class CharacterStats
{
    public int Strength { get; set; } = 10;
    public int Dexterity { get; set; } = 10;
    public int Constitution { get; set; } = 10;
    public int Intelligence { get; set; } = 10;
    public int Wisdom { get; set; } = 10;
    public int Charisma { get; set; } = 10;

    public int GetAbility(CharacterAbility ability)
    {
        return ability switch
        {
            CharacterAbility.Strength => Strength,
            CharacterAbility.Dexterity => Dexterity,
            CharacterAbility.Constitution => Constitution,
            CharacterAbility.Intelligence => Intelligence,
            CharacterAbility.Wisdom => Wisdom,
            CharacterAbility.Charisma => Charisma,
            _ => 10
        };
    }

    public void SetAbility(CharacterAbility ability, int value)
    {
        value = Math.Clamp(value, 1, 20);

        switch (ability)
        {
            case CharacterAbility.Strength:
                Strength = value;
                break;
            case CharacterAbility.Dexterity:
                Dexterity = value;
                break;
            case CharacterAbility.Constitution:
                Constitution = value;
                break;
            case CharacterAbility.Intelligence:
                Intelligence = value;
                break;
            case CharacterAbility.Wisdom:
                Wisdom = value;
                break;
            case CharacterAbility.Charisma:
                Charisma = value;
                break;
        }
    }

    public static int CalculateModifier(int score) => (score - 10) / 2;

    public int GetModifier(CharacterAbility ability) => CalculateModifier(GetAbility(ability));

    public CharacterStats Clone()
    {
        return new CharacterStats
        {
            Strength = Strength,
            Dexterity = Dexterity,
            Constitution = Constitution,
            Intelligence = Intelligence,
            Wisdom = Wisdom,
            Charisma = Charisma
        };
    }
}