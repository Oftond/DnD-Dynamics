using UnityEngine;
using Zenject;

public class GlobalInstaller : MonoInstaller
{
    [SerializeField] private UIManager _uiManager;

    public override void InstallBindings()
    {
        Container.Bind<UIManager>().FromComponentsInNewPrefab(_uiManager).AsSingle().NonLazy();

        Container.BindInterfacesAndSelfTo<GameManager>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
    }
}