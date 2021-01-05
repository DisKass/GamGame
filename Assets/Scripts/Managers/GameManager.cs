using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public enum GameState
    {
        PREGAME,
        PREGAME_RUNNING_SWITCH,
        RUNNING,
        CHANGELEVEL,
        SHOWINGITEMS,
        ENDGAMEMENU
    }

    public enum StoreDataType
    {
        SAVE,
        LOAD,
        RESET
    }

    [SerializeField] public string[] ScenesToLoad;
    [SerializeField] public List<string> NotGameLevel;
    public GameObject[] SystemPrefabs;
    public Events.EventGameStateChanged OnGameStateChanged;
    public Events.EventLevelLoaded OnGameLevelChanged;
    public Events.EventStoreData OnStoreData = new Events.EventStoreData();

    List<GameObject> _instancedSystemPrefabs;
    Dictionary<AsyncOperation, string> _loadOperations;
    Dictionary<AsyncOperation, string> _unloadOperations;

    GameState _currentGameState = GameState.PREGAME;
    public string CurrentLevelName { get => _currentLevelName; set => _currentLevelName = value; }


    public GameState CurrentGameState
    {
        get => _currentGameState;
        private set => _currentGameState = value;
    }

    private string _currentLevelName = "Town";

    protected override void Awake()
    {
        base.Awake();
        //Application.quitting += SaveState;
        //Application.focusChanged += SaveState;
        DontDestroyOnLoad(gameObject);

        Application.targetFrameRate = 60;

        _instancedSystemPrefabs = new List<GameObject>();
        _loadOperations = new Dictionary<AsyncOperation, string>();
        _unloadOperations = new Dictionary<AsyncOperation, string>();
    }
    private void Start()
    {

        InstantiateSystemPrefabs();
        UIManager.Instance.OnMainMenuFadeComplete.AddListener(HandleMainMenuFadeComplete);
        UIManager.Instance.OnLoadingScreenFadeComplete.AddListener(HandleLoadingScreenFadeComplete);
    }
    void OnLoadOperationComplete(AsyncOperation ao)
    {
        if (_loadOperations.ContainsKey(ao))
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(_loadOperations[ao]));
            _loadOperations.Remove(ao);

            if (_loadOperations.Count == 0)
            {
                if (_currentGameState == GameState.PREGAME)
                    UpdateState(GameState.PREGAME_RUNNING_SWITCH);
            }
        }
    }
    public void ChangeLevel(string levelName)
    {
        string currentLevel = CurrentLevelName;
        StartCoroutine(ChangeLevel(currentLevel, levelName));

    }
    IEnumerator ChangeLevel(string currentLevel, string levelName)
    {
        UIManager.Instance.StartLoadingScreenFadeIn();
        yield return new WaitUntil(() => CurrentGameState == GameState.CHANGELEVEL);
        UnloadLevel(currentLevel);
        LoadLevel(levelName);
        UpdateState(GameState.RUNNING);
        OnGameLevelChanged.Invoke(levelName);
    }

    void HandleLoadingScreenFadeComplete(bool fadeOut)
    {
        if (!fadeOut)
        {
            UpdateState(GameState.CHANGELEVEL);
        }
    }
    void OnUnloadOperationComplete(AsyncOperation ao)
    {
        if (_unloadOperations.ContainsKey(ao))
        {
            _unloadOperations.Remove(ao);

        }
    }
    void HandleMainMenuFadeComplete(bool fadeOut)
    {
        if (!fadeOut)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i).name != "MainMenu")
                    UnloadLevel(SceneManager.GetSceneAt(i).name);
            }
            UpdateState(GameState.PREGAME);
        }
        else
            UpdateState(GameState.RUNNING);
    }
    void UpdateState(GameState state)
    {
        GameState previousState = _currentGameState;
        _currentGameState = state;

        switch (_currentGameState)
        {
            case GameState.PREGAME:
                Time.timeScale = 1.0f;
                break;
            case GameState.PREGAME_RUNNING_SWITCH:
                break;
            case GameState.RUNNING:
                Time.timeScale = 1.0f;
                break;
            case GameState.CHANGELEVEL:
                break;
            case GameState.SHOWINGITEMS:
                Time.timeScale = 0.0f;
                break;
            case GameState.ENDGAMEMENU:
                Time.timeScale = 0.0f;
                break;
            default:
                break;
        }
        OnGameStateChanged.Invoke(_currentGameState, previousState);
    }

    void InstantiateSystemPrefabs()
    {
        for (int i = 0; i < SystemPrefabs.Length; i++)
        {
            _instancedSystemPrefabs.Add(Instantiate(SystemPrefabs[i]));
        }
    }

    public void MoveToScene(GameObject go, string sceneName)
    {
        SceneManager.MoveGameObjectToScene(go, SceneManager.GetSceneByName(sceneName));
    }

    public void LoadLevel(string levelName)
    {
        if (_unloadOperations.ContainsValue(levelName)) return;
        if (_loadOperations.ContainsValue(levelName)) return;
        if (SceneManager.GetSceneByName(levelName).isLoaded) return;

        //if (_currentLevelName == levelName) return;
        AsyncOperation ao = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);

        if (ao == null)
        {
            Debug.LogError("[GameManager] Unable to load level " + levelName);
            return;
        }
        _loadOperations.Add(ao, levelName);
        ao.completed += OnLoadOperationComplete;
        CurrentLevelName = levelName;
    }

    public void UnloadLevel(string levelName)
    {
        AsyncOperation ao = SceneManager.UnloadSceneAsync(levelName);
        if (ao == null)
        {
            Debug.LogError("[GameManager] Unable to unload level " + levelName);
            return;
        }
        _unloadOperations.Add(ao, levelName);
        ao.completed += OnUnloadOperationComplete;
        if (CurrentLevelName == levelName) CurrentLevelName = SceneManager.GetActiveScene().name;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();


        for (int i = 0; i < _instancedSystemPrefabs.Count; i++)
        {
            Destroy(_instancedSystemPrefabs[i]);
        }
        _instancedSystemPrefabs.Clear();
    }

    public void StartGame()
    {
        foreach (string level in ScenesToLoad)
            LoadLevel(level);

        if (SaveSystem<GameManager, GameManagerData>.CheckSaveFile("GameManager"))
        {
            GameManagerData data = SaveSystem<GameManager, GameManagerData>.Load("GameManager");
            LoadLevel(data.lastLocation ?? "Town");
        }
        else
        {
            LoadLevel("Town");
        }
    }
    public void TogglePause()
    {
        if (_currentGameState != GameState.RUNNING && _currentGameState != GameState.SHOWINGITEMS)
        {
            return;
        }
        UpdateState(_currentGameState == GameState.RUNNING ? GameState.SHOWINGITEMS : GameState.RUNNING);
    }
    public void EndOfGame()
    {
        UpdateState(GameState.ENDGAMEMENU);
        DeleteAllData();
    }
    public void RestartGame()
    {
        OnStoreData.Invoke(StoreDataType.RESET, "GameManager");
        UpdateState(GameState.PREGAME);
        DeleteAllData();
    }
    void DeleteAllData()
    {
        SaveSystem<GameManager, GameManagerData>.Delete("GameManager");
        SaveSystem<GameManager, GameManagerData>.Delete("LevelState");
        SaveSystem<GameManager, GameManagerData>.Delete("CharacterStatsData");
        SaveSystem<GameManager, GameManagerData>.Delete("DamageInfoData");
        SaveSystem<GameManager, GameManagerData>.Delete("ItemPool");
        SaveSystem<GameManager, GameManagerData>.Delete("TownData");
    }

    public void ReturnMainMemu()
    {
        //OnStoreData.Invoke(StoreDataType.SAVE);
        //SaveState();
        UpdateState(GameState.PREGAME);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    //void SaveState()
    //{
    //    SaveGame("GameManager");
    //}
    public void SaveGame(string source)
    {
        if (source == "ItemPool" || source == "LevelStateController")
            SaveSystem<GameManager, GameManagerData>.Save(this, "GameManager");
        OnStoreData.Invoke(StoreDataType.SAVE, source);
        //Debug.Log("[GameManager] Saved.");
    }
    //void SaveState(bool state)
    //{
    //    if (!state) SaveState();
    //}
}
