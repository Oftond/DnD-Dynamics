using DnD_Dynamics.Services;
using DnD_Dynamics.Services.Interfaces;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace DnD_Dynamics.UI.Windows
{
    public class DMToolsWindow : MonoBehaviour
    {
        [Header("Tabs")]
        [SerializeField] private Button _diceTabButton;
        [SerializeField] private Button _combatTabButton;
        [SerializeField] private Button _notesTabButton;
        [SerializeField] private GameObject _activeTabIndicator;

        [Header("Tab Content")]
        [SerializeField] private GameObject _diceTabContent;
        [SerializeField] private GameObject _combatTabContent;
        [SerializeField] private GameObject _notesTabContent;

        [Header("Dice Roller")]
        [SerializeField] private DiceRollerView _diceRollerView;

        [Header("Combat Tracker")]
        [SerializeField] private CombatTrackerView _combatTrackerView;

        [Header("Notes")]
        [SerializeField] private TMP_InputField _notesInput;
        [SerializeField] private Button _saveNotesButton;

        private string _currentTab = "dice";

        [Inject]
        public void Construct(DiceRollerService diceRollerService, ICombatService combatService)
        {
            _diceRollerView?.Initialize(diceRollerService);
            _combatTrackerView?.Initialize(combatService);
        }

        private void Start()
        {
            _diceTabButton.onClick.AddListener(() => SwitchTab("dice"));
            _combatTabButton.onClick.AddListener(() => SwitchTab("combat"));
            _notesTabButton.onClick.AddListener(() => SwitchTab("notes"));

            _saveNotesButton.onClick.AddListener(SaveNotes);
            LoadNotes();

            SwitchTab("dice");
        }

        private void SwitchTab(string tab)
        {
            _currentTab = tab;

            _diceTabContent.SetActive(tab == "dice");
            _combatTabContent.SetActive(tab == "combat");
            _notesTabContent.SetActive(tab == "notes");
        }

        private void LoadNotes()
        {
            _notesInput.text = PlayerPrefs.GetString("DMTools_Notes", "");
        }

        private void SaveNotes()
        {
            PlayerPrefs.SetString("DMTools_Notes", _notesInput.text);
            PlayerPrefs.Save();
            ShowNotification("Заметки сохранены");
        }

        private void ShowNotification(string message)
        {
            Debug.Log(message);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            _combatTrackerView?.Refresh();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}