using System;

namespace DnD_Dynamics.Models.Combat
{
    [Serializable]
    public class Combatant
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; } = string.Empty;

        public int Initiative { get; set; }

        public int CurrentHp { get; set; }

        public int MaxHp { get; set; }

        public int ArmorClass { get; set; }

        public bool IsPlayer { get; set; }

        public bool IsMonster { get; set; }

        public string MonsterId { get; set; }

        public string CharacterId { get; set; }

        public int HpPercentage => MaxHp > 0 ? (int)((float)CurrentHp / MaxHp * 100) : 0;

        public string HpText => $"{CurrentHp}/{MaxHp}";

        public bool IsAlive => CurrentHp > 0;

        public void ApplyDamage(int amount)
        {
            CurrentHp = Math.Max(0, CurrentHp - amount);
        }

        public void ApplyHeal(int amount)
        {
            CurrentHp = Math.Min(MaxHp, CurrentHp + amount);
        }
    }
}