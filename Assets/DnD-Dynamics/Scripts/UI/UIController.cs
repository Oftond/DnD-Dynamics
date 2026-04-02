using UnityEngine;

public class UIController : MonoBehaviour
{
    private CharacterPresenter _presenter;
    private CharacterModel _model;

    private void Awake()
    {
        _model = new CharacterModel();
        _presenter = new CharacterPresenter(_model);

        if (UIManager.Instance != null)
        {
            var mainMenu = UIManager.Instance.GetComponentInChildren<MainMenuWindow>(true);
            var characterList = UIManager.Instance.GetComponentInChildren<CharacterListWindow>(true);
            var characterDetail = UIManager.Instance.GetComponentInChildren<CharacterDetailWindow>(true);
            var createWindow = UIManager.Instance.GetComponentInChildren<CreateCharacterWindow>(true);

            if (mainMenu != null)
            {
                mainMenu.OnCharactersClicked += ShowCharacterList;
                mainMenu.OnCreateClicked += ShowCreateCharacter;
            }

            if (characterList != null)
            {
                characterList.OnCharacterSelected += OnCharacterSelected;
                characterList.OnCreateClicked += ShowCreateCharacter;
                characterList.OnBackClicked += ShowMainMenu;
                characterList.SetPresenter(_presenter);
            }

            if (characterDetail != null)
            {
                characterDetail.OnBackClicked += ShowCharacterList;
                characterDetail.OnDamageClicked += OnDamageClicked;
                characterDetail.OnHealClicked += OnHealClicked;
                characterDetail.OnLevelUpClicked += OnLevelUpClicked;
                characterDetail.OnDeleteClicked += OnDeleteClicked;
                characterDetail.SetPresenter(_presenter);
            }

            if (createWindow != null)
            {
                createWindow.OnCreateClicked += OnCreateCharacter;
                createWindow.OnCancelClicked += ShowCharacterList;
                createWindow.SetPresenter(_presenter);
            }
        }
    }

    private void Start()
    {
        ShowMainMenu();
    }

    private void ShowMainMenu()
    {
        if (UIManager.Instance != null)
            UIManager.Instance.ShowMainMenu();
    }

    private void ShowCharacterList()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowCharacterList();
            _presenter.RefreshCharacters();
        }
    }

    private void ShowCharacterDetail(string characterId)
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowCharacterDetail();
            _presenter.SelectCharacter(characterId);
        }
    }

    private void ShowCreateCharacter()
    {
        if (UIManager.Instance != null)
            UIManager.Instance.ShowCreateCharacter();
    }

    private void OnCharacterSelected(string characterId)
    {
        ShowCharacterDetail(characterId);
    }

    private void OnDamageClicked(int amount)
    {
        print("ÓÐÎÎÎÎÎÎÎÎÎÎÎÎÎÍ!!!!!!!");
        _presenter.ApplyDamage(amount);
    }

    private void OnHealClicked(int amount)
    {
        _presenter.ApplyHeal(amount);
    }

    private void OnLevelUpClicked()
    {
        _presenter.LevelUp();
    }

    private void OnDeleteClicked()
    {
        _presenter.DeleteCharacter();
        ShowCharacterList();
    }

    private void OnCreateCharacter(string name, int race, int characterClass,
        int strength, int dexterity, int constitution, int intelligence, int wisdom, int charisma)
    {
        _presenter.CreateCharacter(name, (CharacterRace)race, (CharacterClass)characterClass,
            strength, dexterity, constitution, intelligence, wisdom, charisma);
        ShowCharacterList();
    }

    private void OnDestroy()
    {
        _presenter?.Dispose();
    }
}