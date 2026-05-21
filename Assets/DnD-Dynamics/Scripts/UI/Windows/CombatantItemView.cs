// UI/Views/CombatantItemView.cs
using DnD_Dynamics.Models.Combat;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DnD_Dynamics.UI.Windows
{
    public class CombatantItemView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _initiativeText;
        [SerializeField] private TextMeshProUGUI _hpText;
        [SerializeField] private Slider _hpSlider;
        [SerializeField] private Button _damageButton;
        [SerializeField] private Button _healButton;
        [SerializeField] private Button _removeButton;
        [SerializeField] private Button _initiativeButton;
        [SerializeField] private GameObject _activeTurnIndicator;

        private string _combatantId;

        public event Action<string> OnDamage;
        public event Action<string> OnHeal;
        public event Action<string> OnRemove;
        public event Action<string> OnRollInitiative;

        public void Setup(Combatant combatant, bool isActiveTurn)
        {
            _combatantId = combatant.Id;

            _nameText.text = combatant.Name;
            _initiativeText.text = combatant.Initiative.ToString();
            _hpText.text = combatant.HpText;
            _hpSlider.value = (float)combatant.CurrentHp / combatant.MaxHp;

            _activeTurnIndicator.SetActive(isActiveTurn);

            _damageButton.onClick.AddListener(() => OnDamage?.Invoke(_combatantId));
            _healButton.onClick.AddListener(() => OnHeal?.Invoke(_combatantId));
            _removeButton.onClick.AddListener(() => OnRemove?.Invoke(_combatantId));
            _initiativeButton.onClick.AddListener(() => OnRollInitiative?.Invoke(_combatantId));
        }

        public void UpdateHp(int currentHp, int maxHp)
        {
            _hpText.text = $"{currentHp}/{maxHp}";
            _hpSlider.value = (float)currentHp / maxHp;
        }

        public void SetActiveTurn(bool isActive) => _activeTurnIndicator.SetActive(isActive);
    }
}