using System;
using UnityEngine;

[Serializable]
public class CharacterStats
{
    [SerializeField] private int strength = 10;
    [SerializeField] private int dexterity = 10;
    [SerializeField] private int constitution = 10;
    [SerializeField] private int intelligence = 10;
    [SerializeField] private int wisdom = 10;
    [SerializeField] private int charisma = 10;

    public int Strength
    {
        get => strength;
        set => strength = Math.Clamp(value, 1, 30);
    }

    public int Dexterity
    {
        get => dexterity;
        set => dexterity = Math.Clamp(value, 1, 30);
    }

    public int Constitution
    {
        get => constitution;
        set => constitution = Math.Clamp(value, 1, 30);
    }

    public int Intelligence
    {
        get => intelligence;
        set => intelligence = Math.Clamp(value, 1, 30);
    }

    public int Wisdom
    {
        get => wisdom;
        set => wisdom = Math.Clamp(value, 1, 30);
    }

    public int Charisma
    {
        get => charisma;
        set => charisma = Math.Clamp(value, 1, 30);
    }

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