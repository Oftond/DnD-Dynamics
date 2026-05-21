using DnD_Dynamics.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DnD_Dynamics.UI.Windows
{
    public class DiceRollerView : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button _d4Button;
        [SerializeField] private Button _d6Button;
        [SerializeField] private Button _d8Button;
        [SerializeField] private Button _d10Button;
        [SerializeField] private Button _d12Button;
        [SerializeField] private Button _d20Button;

        [Header("Controls")]
        [SerializeField] private TMP_InputField _modifierInput;
        [SerializeField] private Toggle _advantageToggle;
        [SerializeField] private Toggle _disadvantageToggle;

        [Header("Result")]
        [SerializeField] private TextMeshProUGUI _resultText;
        [SerializeField] private TextMeshProUGUI _rollDetailsText;
        [SerializeField] private GameObject _criticalHitEffect;
        [SerializeField] private GameObject _criticalMissEffect;

        private DiceRollerService _diceRollerService;

        public void Initialize(DiceRollerService diceRollerService)
        {
            _diceRollerService = diceRollerService;

            _d4Button.onClick.AddListener(() => RollDice(4));
            _d6Button.onClick.AddListener(() => RollDice(6));
            _d8Button.onClick.AddListener(() => RollDice(8));
            _d10Button.onClick.AddListener(() => RollDice(10));
            _d12Button.onClick.AddListener(() => RollDice(12));
            _d20Button.onClick.AddListener(() => RollDice(20));

            // Логика для переключения преимущества/помехи
            _advantageToggle.onValueChanged.AddListener(isOn =>
            {
                if (isOn && _disadvantageToggle.isOn)
                    _disadvantageToggle.isOn = false;
            });

            _disadvantageToggle.onValueChanged.AddListener(isOn =>
            {
                if (isOn && _advantageToggle.isOn)
                    _advantageToggle.isOn = false;
            });

            _modifierInput.text = "0";
        }

        private async void RollDice(int sides)
        {
            int modifier = int.TryParse(_modifierInput.text, out int m) ? m : 0;
            bool advantage = _advantageToggle.isOn;
            bool disadvantage = _disadvantageToggle.isOn;

            var result = await _diceRollerService.RollDiceAsync(sides, 1, modifier, advantage, disadvantage);

            _resultText.text = result.Total.ToString();
            _rollDetailsText.text = result.RollText;

            _criticalHitEffect?.SetActive(result.IsCriticalHit);
            _criticalMissEffect?.SetActive(result.IsCriticalMiss);

            if (_criticalHitEffect != null && result.IsCriticalHit)
                Invoke(nameof(HideEffects), 2f);
            if (_criticalMissEffect != null && result.IsCriticalMiss)
                Invoke(nameof(HideEffects), 2f);
        }

        private void HideEffects()
        {
            if (_criticalHitEffect != null) _criticalHitEffect.SetActive(false);
            if (_criticalMissEffect != null) _criticalMissEffect.SetActive(false);
        }
    }
}