using DnD_Dynamics.Services;
using System;
using UnityEngine;

public class CharacterCombatService : ICharacterCombatService
{
    public event Action<CharacterData, int> OnDamageApplied;
    public event Action<CharacterData, int> OnHealApplied;

    private readonly ICharacterStatCalculator _statCalculator;

    public CharacterCombatService(ICharacterStatCalculator statCalculator)
    {
        _statCalculator = statCalculator;
    }

    public int ApplyDamage(CharacterData character, int amount)
    {
        amount = Math.Max(0, amount);

        if (character.TemporaryHp > 0)
        {
            var tempDamage = Math.Min(character.TemporaryHp, amount);
            character.TemporaryHp -= tempDamage;
            amount -= tempDamage;
        }

        if (amount > 0)
        {
            character.CurrentHp = Math.Max(0, character.CurrentHp - amount);
            character.UpdatedAt = DateTime.Now;
            OnDamageApplied?.Invoke(character, amount);
        }

        return character.CurrentHp;
    }

    public int ApplyHeal(CharacterData character, int amount, CharacterClass characterClass)
    {
        amount = Math.Max(1, amount);
        var maxHp = _statCalculator.CalculateMaxHp(character, characterClass);
        character.CurrentHp = Math.Min(maxHp, character.CurrentHp + amount);
        character.UpdatedAt = DateTime.Now;
        OnHealApplied?.Invoke(character, amount);

        return character.CurrentHp;
    }
}