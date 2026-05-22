using DnD_Dynamics.UI;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour, IInitializable
{
    [Inject] private UIHandler _uiHandler;

    public UIHandler UIHandler => _uiHandler;

    public void Initialize()
    {
        Debug.Log("GameHandler initialized");

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        if (_uiHandler == null)
        {
            Debug.LogError("UIHandler not found! Creating default...");
            CreateDefaultUIHandler();
        }

        if (_uiHandler != null)
        {
            _uiHandler.ShowMainMenu();
        }
    }

    private void CreateDefaultUIHandler()
    {
        var uiHandlerObj = new GameObject("UIHandler");

        _uiHandler = uiHandlerObj.AddComponent<UIHandler>();

        DontDestroyOnLoad(uiHandlerObj);
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