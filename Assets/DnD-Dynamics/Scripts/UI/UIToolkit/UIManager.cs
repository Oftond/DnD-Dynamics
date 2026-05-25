using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

namespace DnD_Dynamics.UI
{
    /// <summary>
    /// Главный контроллер UI приложения на базе UI Toolkit
    /// Управляет навигацией между окнами и их инициализацией
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private VisualTreeAsset mainMenuLayout;
        [SerializeField] private VisualTreeAsset characterCreatorLayout;
        [SerializeField] private VisualTreeAsset handbookLayout;
        [SerializeField] private VisualTreeAsset dmToolsLayout;
        [SerializeField] private VisualTreeAsset combatTrackerLayout;
        [SerializeField] private StyleSheet commonStyles;

        private UIDocument _currentDocument;
        private VisualElement _root;
        
        // Windows cache
        private VisualElement _mainMenu;
        private VisualElement _characterCreator;
        private VisualElement _handbook;
        private VisualElement _dmTools;
        private VisualElement _combatTracker;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            // Создаем документ если нет
            if (_currentDocument == null)
            {
                GameObject uiObj = new GameObject("UI_Document");
                uiObj.transform.SetParent(transform);
                _currentDocument = uiObj.AddComponent<UIDocument>();
            }

            ShowMainMenu();
        }

        #region Navigation

        public void ShowMainMenu()
        {
            if (mainMenuLayout == null)
            {
                Debug.LogError("MainMenu layout not assigned!");
                return;
            }

            _root = mainMenuLayout.Instantiate();
            _currentDocument.visualTreeAsset = mainMenuLayout;
            _currentDocument.enabled = true;
            
            // Force reload
            _currentDocument.enabled = false;
            _currentDocument.enabled = true;

            BindMainMenuEvents();
        }

        public void ShowCharacterCreator()
        {
            if (characterCreatorLayout == null)
            {
                Debug.LogError("CharacterCreator layout not assigned!");
                return;
            }

            _currentDocument.visualTreeAsset = characterCreatorLayout;
            _currentDocument.enabled = false;
            _currentDocument.enabled = true;

            BindCharacterCreatorEvents();
        }

        public void ShowHandbook()
        {
            if (handbookLayout == null)
            {
                Debug.LogError("Handbook layout not assigned!");
                return;
            }

            _currentDocument.visualTreeAsset = handbookLayout;
            _currentDocument.enabled = false;
            _currentDocument.enabled = true;

            BindHandbookEvents();
        }

        public void ShowDMTools()
        {
            if (dmToolsLayout == null)
            {
                Debug.LogError("DMTools layout not assigned!");
                return;
            }

            _currentDocument.visualTreeAsset = dmToolsLayout;
            _currentDocument.enabled = false;
            _currentDocument.enabled = true;

            BindDMToolsEvents();
        }

        public void ShowCombatTracker()
        {
            if (combatTrackerLayout == null)
            {
                Debug.LogError("CombatTracker layout not assigned!");
                return;
            }

            _currentDocument.visualTreeAsset = combatTrackerLayout;
            _currentDocument.enabled = false;
            _currentDocument.enabled = true;

            BindCombatTrackerEvents();
        }

        #endregion

        #region Event Bindings

        private void BindMainMenuEvents()
        {
            var root = _currentDocument.rootVisualElement;
            
            var btnNewGame = root.Q<Button>("BtnNewGame");
            if (btnNewGame != null)
                btnNewGame.clicked += ShowCharacterCreator;

            var btnHandbook = root.Q<Button>("BtnHandbook");
            if (btnHandbook != null)
                btnHandbook.clicked += ShowHandbook;

            var btnCombat = root.Q<Button>("BtnCombat");
            if (btnCombat != null)
                btnCombat.clicked += ShowCombatTracker;

            var btnDMTools = root.Q<Button>("BtnDMTools");
            if (btnDMTools != null)
                btnDMTools.clicked += ShowDMTools;

            var btnExit = root.Q<Button>("BtnExit");
            if (btnExit != null)
                btnExit.clicked += () => Application.Quit();
        }

        private void BindCharacterCreatorEvents()
        {
            var root = _currentDocument.rootVisualElement;

            var btnCancel = root.Q<Button>("BtnCancel");
            if (btnCancel != null)
                btnCancel.clicked += ShowMainMenu;

            var btnSave = root.Q<Button>("BtnSaveCharacter");
            if (btnSave != null)
                btnSave.clicked += OnSaveCharacter;

            var btnRollStats = root.Q<Button>("BtnRollStats");
            if (btnRollStats != null)
                btnRollStats.clicked += RollStats;
        }

        private void BindHandbookEvents()
        {
            var root = _currentDocument.rootVisualElement;

            var btnAddNew = root.Q<Button>("BtnAddNew");
            if (btnAddNew != null)
                btnAddNew.clicked += OnAddNewItem;

            var inputSearch = root.Q<TextField>("InputSearch");
            if (inputSearch != null)
                inputSearch.RegisterValueChangedCallback(evt => OnSearchChanged(evt.newValue));

            var dropCategory = root.Q<DropdownField>("DropCategory");
            if (dropCategory != null)
                dropCategory.RegisterValueChangedCallback(evt => OnCategoryChanged(evt.newValue));
        }

        private void BindDMToolsEvents()
        {
            var root = _currentDocument.rootVisualElement;

            // Dice buttons
            RegisterDiceButton(root, "DiceD4", 4);
            RegisterDiceButton(root, "DiceD6", 6);
            RegisterDiceButton(root, "DiceD8", 8);
            RegisterDiceButton(root, "DiceD10", 10);
            RegisterDiceButton(root, "DiceD12", 12);
            RegisterDiceButton(root, "DiceD20", 20);
            RegisterDiceButton(root, "DiceD100", 100);

            var btnGenNPC = root.Q<Button>("BtnGenNPC");
            if (btnGenNPC != null)
                btnGenNPC.clicked += GenerateNPCName;

            var btnNewNote = root.Q<Button>("BtnNewNote");
            if (btnNewNote != null)
                btnNewNote.clicked += CreateNewNote;
        }

        private void BindCombatTrackerEvents()
        {
            var root = _currentDocument.rootVisualElement;

            var btnStart = root.Q<Button>("BtnStartCombat");
            if (btnStart != null)
                btnStart.clicked += StartCombat;

            var btnNextTurn = root.Q<Button>("BtnNextTurn");
            if (btnNextTurn != null)
                btnNextTurn.clicked += NextTurn;

            var btnEnd = root.Q<Button>("BtnEndCombat");
            if (btnEnd != null)
                btnEnd.clicked += EndCombat;

            var btnAddMonster = root.Q<Button>("BtnAddMonster");
            if (btnAddMonster != null)
                btnAddMonster.clicked += AddMonsterFromBestiary;

            var btnAddPC = root.Q<Button>("BtnAddPC");
            if (btnAddPC != null)
                btnAddPC.clicked += AddPlayerCharacter;
        }

        #endregion

        #region Handlers

        private void OnSaveCharacter()
        {
            Debug.Log("Saving character...");
            // TODO: Integrate with CharacterPresenter
        }

        private void RollStats()
        {
            Debug.Log("Rolling stats...");
            var root = _currentDocument.rootVisualElement;
            
            // Simulate rolling
            var stats = new string[] { "StatSTR", "StatDEX", "StatCON", "StatINT", "StatWIS", "StatCHA" };
            foreach (var stat in stats)
            {
                var field = root.Q<IntegerField>(stat);
                if (field != null)
                {
                    int roll1 = Random.Range(1, 7);
                    int roll2 = Random.Range(1, 7);
                    int roll3 = Random.Range(1, 7);
                    int total = Mathf.Max(Mathf.Max(roll1, roll2), roll3);
                    field.value = total;
                }
            }
        }

        private void OnAddNewItem()
        {
            Debug.Log("Adding new item to handbook...");
        }

        private void OnSearchChanged(string query)
        {
            Debug.Log($"Searching: {query}");
        }

        private void OnCategoryChanged(string category)
        {
            Debug.Log($"Filtering by: {category}");
        }

        private void RegisterDiceButton(VisualElement root, string buttonName, int sides)
        {
            var btn = root.Q<Button>(buttonName);
            if (btn != null)
            {
                btn.clicked += () => RollDice(sides);
            }
        }

        private void RollDice(int sides)
        {
            int result = Random.Range(1, sides + 1);
            Debug.Log($"Rolled d{sides}: {result}");
            
            var root = _currentDocument.rootVisualElement;
            var lbl = root.Q<Label>("LblLastRoll");
            if (lbl != null)
                lbl.text = $"Последний бросок: d{sides} = {result}";
        }

        private void GenerateNPCName()
        {
            string[] firstNames = { "Арагорн", "Гимли", "Леголас", "Гэндальф", "Фродо", "Сэм", "Боромир", "Теоден" };
            string[] lastNames = { "из Гондора", "Сын Глоина", "из Лихолесья", "Серый", "Торбинс", "Гэмджи", "из Рохана", "Эадриг" };
            
            string name = $"{firstNames[Random.Range(0, firstNames.Length)]} {lastNames[Random.Range(0, lastNames.Length)]}";
            
            var root = _currentDocument.rootVisualElement;
            var lbl = root.Q<Label>("LblNPCName");
            if (lbl != null)
                lbl.text = name;
        }

        private void CreateNewNote()
        {
            Debug.Log("Creating new note...");
        }

        private void StartCombat()
        {
            Debug.Log("Starting combat...");
        }

        private void NextTurn()
        {
            Debug.Log("Next turn...");
        }

        private void EndCombat()
        {
            Debug.Log("Ending combat...");
        }

        private void AddMonsterFromBestiary()
        {
            Debug.Log("Adding monster from bestiary...");
        }

        private void AddPlayerCharacter()
        {
            Debug.Log("Adding player character to combat...");
        }

        #endregion
    }
}
