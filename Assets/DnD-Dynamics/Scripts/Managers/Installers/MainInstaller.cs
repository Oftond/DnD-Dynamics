using UnityEngine;
using Zenject;

public class MainInstaller : MonoInstaller
{
    [Header("Managers")]
    [SerializeField] private UIManager _uiManager;

    public override void InstallBindings()
    {
        Container.Bind<UIManager>().FromInstance(_uiManager).AsSingle().NonLazy();
    }
}