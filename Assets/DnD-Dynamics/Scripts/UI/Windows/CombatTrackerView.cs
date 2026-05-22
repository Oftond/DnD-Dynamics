using DnD_Dynamics.Models.Combat;
using DnD_Dynamics.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DnD_Dynamics.UI.Windows
{
    public class CombatTrackerView : MonoBehaviour
    {
        [Header("Controls")]
        [SerializeField] private Button _nextTurnButton;
        [SerializeField] private Button _previousTurnButton;
        [SerializeField] private Button _rollAllInitiativeButton;
        [SerializeField] private Button _addCombatantButton;

        [Header("Current Turn")]
        [SerializeField] private TextMeshProUGUI _currentTurnText;
        [SerializeField] private TextMeshProUGUI _currentRoundText;

        [Header("Combatants List")]
        [SerializeField] private Transform _combatantsContainer;
        [SerializeField] private GameObject _combatantItemPrefab;

        [Header("Add Combatant Panel")]
        [SerializeField] private GameObject _addCombatantPanel;
        [SerializeField] private TMP_InputField _combatantNameInput;
        [SerializeField] private TMP_InputField _combatantInitiativeInput;
        [SerializeField] private TMP_InputField _combatantHpInput;
        [SerializeField] private TMP_InputField _combatantAcInput;
        [SerializeField] private Toggle _combatantIsPlayerToggle;
        [SerializeField] private Button _confirmAddButton;
        [SerializeField] private Button _cancelAddButton;

        [Header("Monster Selector")]
        [SerializeField] private GameObject _monsterSelectorPanel;
        [SerializeField] private Transform _monstersContainer;
        [SerializeField] private GameObject _monsterItemPrefab;
        [SerializeField] private TMP_InputField _monsterSearchInput;

        private ICombatService _combatService;
        private CombatSession _currentSession;
        private Dictionary<string, CombatantItemView> _combatantItems = new();

        public void Initialize(ICombatService combatService)
        {
            _combatService = combatService;

            _nextTurnButton.onClick.AddListener(OnNextTurn);
            _previousTurnButton.onClick.AddListener(OnPreviousTurn);
            _rollAllInitiativeButton.onClick.AddListener(OnRollAllInitiative);
            _addCombatantButton.onClick.AddListener(() => _addCombatantPanel.SetActive(true));
            _confirmAddButton.onClick.AddListener(OnAddCombatant);
            _cancelAddButton.onClick.AddListener(() => _addCombatantPanel.SetActive(false));
        }

        public async void Refresh()
        {
            _currentSession = await _combatService.GetCurrentSessionAsync();
            UpdateUI();
        }

        private void UpdateUI()
        {
            UpdateCombatantsList();
            UpdateTurnDisplay();
        }

        private void UpdateCombatantsList()
        {
            foreach (var item in _combatantItems.Values)
            {
                if (item != null)
                    Destroy(item.gameObject);
            }
            _combatantItems.Clear();

            foreach (var combatant in _currentSession.Combatants)
            {
                var itemObj = Instantiate(_combatantItemPrefab, _combatantsContainer);
                var itemView = itemObj.GetComponent<CombatantItemView>();
                itemView.Setup(combatant, _currentSession.CurrentCombatant?.Id == combatant.Id);
                itemView.OnDamage += (id) => OnDamageCombatant(id);
                itemView.OnHeal += (id) => OnHealCombatant(id);
                itemView.OnRemove += (id) => OnRemoveCombatant(id);
                itemView.OnRollInitiative += (id) => OnRollInitiative(id);
                _combatantItems[combatant.Id] = itemView;
            }
        }

        private void UpdateTurnDisplay()
        {
            var current = _currentSession.CurrentCombatant;
            _currentTurnText.text = current != null ? $"Текущий ход: {current.Name}" : "Нет участников";
            _currentRoundText.text = $"Раунд {_currentSession.CurrentRound}";

            _nextTurnButton.interactable = _currentSession.Combatants.Count > 0;
            _previousTurnButton.interactable = _currentSession.Combatants.Count > 0;
        }

        private async void OnNextTurn()
        {
            await _combatService.NextTurnAsync();

            Refresh();
        }

        private async void OnPreviousTurn()
        {
            await _combatService.PreviousTurnAsync();

            Refresh();
        }

        private async void OnRollAllInitiative()
        {
            await _combatService.RollInitiativeForAllAsync();

            Refresh();
        }

        private async void OnAddCombatant()
        {
            var combatant = new Combatant
            {
                Name = _combatantNameInput.text,
                Initiative = int.TryParse(_combatantInitiativeInput.text, out int init) ? init : 0,
                MaxHp = int.TryParse(_combatantHpInput.text, out int hp) ? hp : 10,
                CurrentHp = int.TryParse(_combatantHpInput.text, out hp) ? hp : 10,
                ArmorClass = int.TryParse(_combatantAcInput.text, out int ac) ? ac : 10,
                IsPlayer = _combatantIsPlayerToggle.isOn,
                IsMonster = !_combatantIsPlayerToggle.isOn
            };

            await _combatService.AddCombatantAsync(combatant);

            _combatantNameInput.text = "";
            _combatantInitiativeInput.text = "";
            _combatantHpInput.text = "";
            _combatantAcInput.text = "";
            _addCombatantPanel.SetActive(false);

            Refresh();
        }

        private async void OnDamageCombatant(string id)
        {
            var amount = int.TryParse(_combatantInitiativeInput.text, out int dmg) ? dmg : 5;
            await _combatService.ApplyDamageToCombatantAsync(id, amount);

            Refresh();
        }

        private async void OnHealCombatant(string id)
        {
            var amount = int.TryParse(_combatantInitiativeInput.text, out int heal) ? heal : 5;
            await _combatService.ApplyHealToCombatantAsync(id, amount);

            Refresh();
        }

        private async void OnRemoveCombatant(string id)
        {
            await _combatService.RemoveCombatantAsync(id);

            Refresh();
        }

        private async void OnRollInitiative(string id)
        {
            await _combatService.RollInitiativeForCombatantAsync(id);

            Refresh();
        }
    }
}