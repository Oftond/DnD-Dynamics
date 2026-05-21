using UnityEngine;
using Zenject;
using DnD_Dynamics.MVP.Model;
using DnD_Dynamics.Models;
using DnD_Dynamics.UI.Windows;
using DnD_Dynamics.MVP.Presenter;
using TMPro;
using System.Threading.Tasks;

namespace DnD_Dynamics.UI
{
    public class UIHandler : MonoBehaviour
    {
        [Inject] private CharacterListPresenter _characterListPresenter;
        [Inject] private CharacterDetailPresenter _characterDetailPresenter;
        [Inject] private CreateCharacterPresenter _createCharacterPresenter;

        [Header("Windows")]
        [SerializeField] private MainMenuWindow _mainMenuWindow;
        [SerializeField] private CharacterListWindow _characterListWindow;
        [SerializeField] private CharacterDetailWindow _characterDetailWindow;
        [SerializeField] private CreateCharacterWindow _createCharacterWindow;
        [SerializeField] private HandbookWindow _handbookWindow;
        [SerializeField] private DMToolsWindow _dmToolsWindow;

        [Header("Loading")]
        [SerializeField] private GameObject _loadingPanel;
        [SerializeField] private TextMeshProUGUI _loadingText;

        [Header("Notifications")]
        [SerializeField] private GameObject _notificationPanel;
        [SerializeField] private TextMeshProUGUI _notificationText;

        private void Awake()
        {
            InitializeWindows();
        }

        private void InitializeWindows()
        {
            if (_mainMenuWindow != null)
            {
                _mainMenuWindow.OnCharactersClicked += ShowCharacterListHandler;
                _mainMenuWindow.OnCreateClicked += ShowCreateCharacter;
                _mainMenuWindow.OnHandbookClicked += ShowHandbook;
                _mainMenuWindow.OnDMToolsClicked += ShowDMTools;
                _mainMenuWindow.OnExitClicked += ExitApplication;
            }

            if (_characterListWindow != null)
            {
                _characterListWindow.SetPresenter(_characterListPresenter);
                _characterListWindow.OnBackClicked += ShowMainMenu;
                _characterListWindow.OnCreateClicked += ShowCreateCharacter;
                _characterListWindow.OnCharacterSelected += OnCharacterSelected;
            }

            if (_characterDetailWindow != null)
            {
                _characterDetailWindow.SetPresenter(_characterDetailPresenter);
                _characterDetailWindow.OnBackClicked += ShowCharacterListHandler;
                _characterDetailWindow.OnDamageClicked += async amount => await _characterDetailPresenter.ApplyDamageAsync(amount);
                _characterDetailWindow.OnHealClicked += async amount => await _characterDetailPresenter.ApplyHealAsync(amount);
                _characterDetailWindow.OnLevelUpClicked += async () => await _characterDetailPresenter.LevelUpAsync();
                _characterDetailWindow.OnDeleteClicked += async () =>
                {
                    await _characterDetailPresenter.DeleteCharacterAsync();
                    
                    await ShowCharacterList();
                };
            }

            if (_createCharacterWindow != null)
            {
                _createCharacterWindow.SetPresenter(_createCharacterPresenter);
                _createCharacterWindow.OnCancelClicked += ShowCharacterListHandler;
                _createCharacterWindow.OnCreateClicked += OnCreateCharacter;
            }
        }

        private void OnCharacterSelected(string characterId)
        {
            _characterListPresenter.SelectCharacter(characterId);
            ShowCharacterDetail();
        }

        private async void OnCreateCharacter(string name, string raceId, string classId, int strength, int dexterity, int constitution, int intelligence, int wisdom, int charisma)
        {
            await _createCharacterPresenter.CreateCharacterAsync(name, raceId, classId, strength, dexterity, constitution, intelligence, wisdom, charisma);

            await ShowCharacterList();
        }

        public void ShowMainMenu()
        {
            HideAllWindows();
            _mainMenuWindow?.Show();
        }

        public async Task ShowCharacterList()
        {
            HideAllWindows();
            await _characterListWindow?.Show();
        }

        private async void ShowCharacterListHandler()
        {
            HideAllWindows();
            await _characterListWindow?.Show();
        }

        public void ShowCharacterDetail()
        {
            HideAllWindows();
            _characterDetailWindow?.Show();
        }

        public async void ShowCreateCharacter()
        {
            HideAllWindows();

            await _createCharacterWindow?.Show();
        }


        public void ShowHandbook()
        {
            HideAllWindows();

            _handbookWindow?.Show();
        }

        public void ShowDMTools()
        {
            HideAllWindows();

            _dmToolsWindow?.Show();
        }

        private void HideAllWindows()
        {
            _mainMenuWindow?.Hide();
            _characterListWindow?.Hide();
            _characterDetailWindow?.Hide();
            _createCharacterWindow?.Hide();
        }

        public void ShowLoading(string message = "Загрузка...")
        {
            if (_loadingPanel != null)
            {
                if (_loadingText != null)
                    _loadingText.text = message;
                _loadingPanel.SetActive(true);
            }
        }

        public void HideLoading()
        {
            if (_loadingPanel != null)
                _loadingPanel.SetActive(false);
        }

        public void ShowNotification(string message, float duration = 2f)
        {
            if (_notificationPanel != null)
            {
                if (_notificationText != null)
                    _notificationText.text = message;
                _notificationPanel.SetActive(true);
                Invoke(nameof(HideNotification), duration);
            }
        }

        private void HideNotification()
        {
            if (_notificationPanel != null)
                _notificationPanel.SetActive(false);
        }

        public void ShowError(string message)
        {
            ShowNotification(message, 3f);
            Debug.LogError(message);
        }

        public void ShowSuccess(string message)
        {
            ShowNotification(message, 2f);
            Debug.Log(message);
        }

        private void ExitApplication()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}