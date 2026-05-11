using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour, IInitializable
{
    [Inject] private UIManager _uiManager;

    public UIManager UIManager => _uiManager;

    public void Initialize()
    {
        Debug.Log("GameManager initialized");

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        if (_uiManager == null)
        {
            Debug.LogError("UIManager not found! Creating default...");
            CreateDefaultUIManager();
        }

        if (_uiManager != null)
        {
            _uiManager.ShowMainMenu();
        }
    }

    private void CreateDefaultUIManager()
    {
        var uiManagerObj = new GameObject("UIManager");
        _uiManager = uiManagerObj.AddComponent<UIManager>();
        DontDestroyOnLoad(uiManagerObj);
    }

    private void OnApplicationQuit()
    {
        Debug.Log("Application quitting...");
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            Debug.Log("Application paused");
        }
        else
        {
            Debug.Log("Application resumed");
        }
    }
}