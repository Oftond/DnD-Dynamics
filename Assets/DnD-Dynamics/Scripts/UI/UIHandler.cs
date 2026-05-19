using UnityEngine;
using Zenject;
using DnD_Dynamics.MVP.Model;
using DnD_Dynamics.Models;
using DnD_Dynamics.UI.Windows;
using DnD_Dynamics.MVP.Presenter;
using TMPro;

namespace DnD_Dynamics.UI
{
    public class UIHandler : MonoBehaviour
    {
        [Inject] private CharacterPresenter _presenter;

        [Header("Windows")]
        [SerializeField] private MainMenuWindow mainMenuWindow;
        [SerializeField] private CharacterListWindow characterListWindow;
        [SerializeField] private CharacterDetailWindow characterDetailWindow;
        [SerializeField] private CreateCharacterWindow createCharacterWindow;

        [Header("Loading")]
        [SerializeField] private GameObject loadingPanel;
        [SerializeField] private TextMeshProUGUI loadingText;

        [Header("Notifications")]
        [SerializeField] private GameObject notificationPanel;
        [SerializeField] private TextMeshProUGUI notificationText;

        private void Awake()
        {
            InitializeWindows();
        }

        private void InitializeWindows()
        {
            if (mainMenuWindow != null)
            {
                mainMenuWindow.Initialize();
                mainMenuWindow.OnCharactersClicked += ShowCharacterList;
                mainMenuWindow.OnCreateClicked += ShowCreateCharacter;
                mainMenuWindow.OnExitClicked += ExitApplication;
            }

            if (characterListWindow != null)
            {
                characterListWindow.Initialize();
                characterListWindow.SetPresenter(_presenter);
                characterListWindow.OnBackClicked += ShowMainMenu;
                characterListWindow.OnCreateClicked += ShowCreateCharacter;
                characterListWindow.OnCharacterSelected += OnCharacterSelected;
            }

            if (characterDetailWindow != null)
            {
                characterDetailWindow.Initialize();
                characterDetailWindow.SetPresenter(_presenter);
                characterDetailWindow.OnBackClicked += ShowCharacterList;
                characterDetailWindow.OnDamageClicked += amount => _presenter.ApplyDamage(amount);
                characterDetailWindow.OnHealClicked += amount => _presenter.ApplyHeal(amount);
                characterDetailWindow.OnLevelUpClicked += () => _presenter.LevelUp();
                characterDetailWindow.OnDeleteClicked += () => { _presenter.DeleteCharacter(); ShowCharacterList(); };
            }

            if (createCharacterWindow != null)
            {
                createCharacterWindow.Initialize();
                createCharacterWindow.SetPresenter(_presenter);
                createCharacterWindow.OnCancelClicked += ShowCharacterList;
                createCharacterWindow.OnCreateClicked += OnCreateCharacter;
            }
        }

        private void OnCharacterSelected(string characterId)
        {
            _presenter.SelectCharacter(characterId);
            ShowCharacterDetail();
        }

        private void OnCreateCharacter(string name, string raceId, string classId, int strength, int dexterity, int constitution, int intelligence, int wisdom, int charisma)
        {
            var race = _presenter.GetRaceById(raceId);
            var characterClass = _presenter.GetClassById(classId);

            _presenter.CreateCharacter(name, race, characterClass,
                strength, dexterity, constitution, intelligence, wisdom, charisma);

            ShowCharacterList();
            characterListWindow?.RefreshCharacters();
        }

        public void ShowMainMenu()
        {
            HideAllWindows();
            mainMenuWindow?.Show();
        }

        public void ShowCharacterList()
        {
            HideAllWindows();
            characterListWindow?.Show();
            _presenter?.RefreshCharacters();
        }

        public void ShowCharacterDetail()
        {
            HideAllWindows();
            characterDetailWindow?.Show();
        }

        public void ShowCreateCharacter()
        {
            HideAllWindows();
            createCharacterWindow?.Show();
        }

        private void HideAllWindows()
        {
            mainMenuWindow?.Hide();
            characterListWindow?.Hide();
            characterDetailWindow?.Hide();
            createCharacterWindow?.Hide();
        }

        public void ShowLoading(string message = "Загрузка...")
        {
            if (loadingPanel != null)
            {
                if (loadingText != null)
                    loadingText.text = message;
                loadingPanel.SetActive(true);
            }
        }

        public void HideLoading()
        {
            if (loadingPanel != null)
                loadingPanel.SetActive(false);
        }

        public void ShowNotification(string message, float duration = 2f)
        {
            if (notificationPanel != null)
            {
                if (notificationText != null)
                    notificationText.text = message;
                notificationPanel.SetActive(true);
                Invoke(nameof(HideNotification), duration);
            }
        }

        private void HideNotification()
        {
            if (notificationPanel != null)
                notificationPanel.SetActive(false);
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