using System;
using System.Threading.Tasks;

namespace DnD_Dynamics.Services
{
    public class DiceRollerService
    {
        private readonly Random _random = new Random();

        public event Action<DiceRollResult> OnRollComplete;

        public async Task<DiceRollResult> RollDiceAsync(int sides, int count = 1, int modifier = 0, bool advantage = false, bool disadvantage = false)
        {
            return await Task.Run(() =>
            {
                var result = new DiceRollResult
                {
                    Sides = sides,
                    Count = count,
                    Modifier = modifier,
                    Advantage = advantage,
                    Disadvantage = disadvantage
                };

                if (advantage && !disadvantage)
                {
                    var roll1 = _random.Next(1, sides + 1);
                    var roll2 = _random.Next(1, sides + 1);
                    result.Rolls = new[] { roll1, roll2 };
                    result.Total = Math.Max(roll1, roll2) + modifier;
                    result.RollText = $"{roll1} / {roll2} (Преимущество) => {result.Total - modifier} + {modifier} = {result.Total}";
                }
                else if (disadvantage && !advantage)
                {
                    var roll1 = _random.Next(1, sides + 1);
                    var roll2 = _random.Next(1, sides + 1);
                    result.Rolls = new[] { roll1, roll2 };
                    result.Total = Math.Min(roll1, roll2) + modifier;
                    result.RollText = $"{roll1} / {roll2} (Помеха) => {result.Total - modifier} + {modifier} = {result.Total}";
                }
                else
                {
                    var roll = _random.Next(1, sides + 1);
                    result.Rolls = new[] { roll };
                    result.Total = roll + modifier;
                    result.RollText = $"{roll} + {modifier} = {result.Total}";
                }

                OnRollComplete?.Invoke(result);
                return result;
            });
        }

        public async Task<DiceRollResult> RollD20Async(int modifier = 0, bool advantage = false, bool disadvantage = false)
        {
            return await RollDiceAsync(20, 1, modifier, advantage, disadvantage);
        }
    }

    public class DiceRollResult
    {
        public int Sides { get; set; }
        public int Count { get; set; }
        public int Modifier { get; set; }
        public bool Advantage { get; set; }
        public bool Disadvantage { get; set; }
        public int[] Rolls { get; set; }
        public int Total { get; set; }
        public string RollText { get; set; } = string.Empty;

        public bool IsCriticalHit => !Advantage && !Disadvantage && Rolls != null && Rolls.Length > 0 && Rolls[0] == 20;
        public bool IsCriticalMiss => !Advantage && !Disadvantage && Rolls != null && Rolls.Length > 0 && Rolls[0] == 1;
    }
}