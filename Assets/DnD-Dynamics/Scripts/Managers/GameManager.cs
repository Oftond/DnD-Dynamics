using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private UIManager uiManager;

    private static GameManager _instance;
    public static GameManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = 60;

        Initialize();
    }

    private void Initialize()
    {
        Debug.Log("GameManager initialized");

        if (uiManager == null)
        {
            Debug.LogError("UIManager not found! Creating default...");
            CreateDefaultUIManager();
        }

        if (uiManager != null)
        {
            uiManager.ShowMainMenu();
        }
    }

    private void CreateDefaultUIManager()
    {
        var uiManagerObj = new GameObject("UIManager");
        uiManager = uiManagerObj.AddComponent<UIManager>();
        DontDestroyOnLoad(uiManagerObj);
    }

    public UIManager GetUIManager()
    {
        return uiManager;
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