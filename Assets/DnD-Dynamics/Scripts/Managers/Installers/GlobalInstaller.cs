using DnD_Dynamics.MVP.Model;
using DnD_Dynamics.MVP.Presenter;
using DnD_Dynamics.Services;
using DnD_Dynamics.Services.Interfaces;
using DnD_Dynamics.UI;
using System.ComponentModel.Design;
using UnityEngine;
using Zenject;

public class GlobalInstaller : MonoInstaller
{
    [SerializeField] private UIHandler _uiHandler;

    public override void InstallBindings()
    {
        ServicesInstall();

        ManagersInstall();

        PresentersInstall();

        ModelsInstall();
    }

    private void ManagersInstall()
    {
        Container.Bind<UIHandler>().FromComponentsInNewPrefab(_uiHandler).AsSingle().NonLazy();

        Container.BindInterfacesAndSelfTo<GameManager>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();

        Container.Bind<SkillManager>().FromNew().AsSingle().NonLazy();
    }

    private void ServicesInstall()
    {
        Container.Bind<IDataService>().To<DataService>().AsSingle();

        Container.Bind<IGameDataService>().To<GameDataService>().AsSingle();
        Container.Bind<IHandbookFilterService>().To<HandbookFilterService>().AsSingle();
        Container.Bind<ISearchService>().To<SearchService>().AsSingle();

        Container.Bind<ICombatService>().To<CombatService>().AsSingle();
        Container.Bind<DiceRollerService>().AsSingle();

        Container.Bind<ICharacterStatCalculator>().To<CharacterStatCalculator>().AsSingle();

        Container.Bind<ICharacterCombatService>().To<CharacterCombatService>().AsSingle();

        Container.Bind<ICharacterProgressionService>().To<CharacterProgressionService>().AsSingle();

        Container.Bind<ICharacterUiMapper>().To<CharacterUiMapper>().AsSingle();

        Container.Bind<IPortraitDataService>().To<PortraitDataService>().AsSingle();
    }

    private void PresentersInstall()
    {
        Container.Bind<CharacterListPresenter>().AsSingle();
        Container.Bind<CharacterDetailPresenter>().AsSingle();
        Container.Bind<CreateCharacterPresenter>().AsSingle();
    }

    private void ModelsInstall()
    {
        Container.Bind<CharacterModel>().AsSingle();
    }
}