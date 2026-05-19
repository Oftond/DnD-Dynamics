using DnD_Dynamics.MVP.Model;
using DnD_Dynamics.MVP.Presenter;
using DnD_Dynamics.Services;
using DnD_Dynamics.UI;
using System.ComponentModel.Design;
using UnityEditor.Search;
using UnityEngine;
using Zenject;

public class GlobalInstaller : MonoInstaller
{
    [SerializeField] private UIHandler _uiHandler;

    public override void InstallBindings()
    {
        ServicesInstall();

        ManagersInstall();
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
        Container.Bind<IHandbookDataService>().To<HandbookDataService>().AsSingle();
        Container.Bind<IHandbookFilterService>().To<HandbookFilterService>().AsSingle();
        Container.Bind<ISearchService>().To<SearchService>().AsSingle();

        Container.Bind<CharacterModel>().AsSingle();
        Container.Bind<CharacterPresenter>().AsSingle();
    }
}