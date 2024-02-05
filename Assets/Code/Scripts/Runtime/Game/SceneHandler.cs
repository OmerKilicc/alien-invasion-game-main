using Euphrates;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{

    [Header("Events")]
    [SerializeField] TriggerChannelSO _initialize;
    [Space]
    [SerializeField] TriggerChannelSO _loadMainMenu;
    [SerializeField] TriggerChannelSO _loadLevel;
    [Space]
    [SerializeField] SceneEventSO _loadComplete;
    [SerializeField] TriggerChannelSO _menuLoadComplete;
    [SerializeField] TriggerChannelSO _levelLoadComplete;

    [Space]
    [Space]
    [Header("Scenes")]
    [SerializeField] int _uiBuildIndex;
    [SerializeField] int _menuBuildIndex;
    [Tooltip("This is the build index of the first game level."), SerializeField] int _levelBuildIndexOffset;

    [Space]
    [SerializeField] IntSO _levelIndex;

    int _currentScene = -1;

    private void OnEnable()
    {
        _initialize.AddListener(Initialize);

        _loadMainMenu.AddListener(LoadMenu);
        _loadLevel.AddListener(LoadGameScene);
    }

    private void OnDisable()
    {
        _initialize.RemoveListener(Initialize);

        _loadMainMenu.RemoveListener(LoadMenu);
        _loadLevel.RemoveListener(LoadGameScene);
    }

    void Initialize()
    {
        LoadScene(_uiBuildIndex).completed += op => LoadMenu();
    }

    AsyncOperation LoadScene(int sceneBuildIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneBuildIndex, LoadSceneMode.Additive);
        operation.completed += op => OnSceneLoaded(sceneBuildIndex);

        return operation;
    }

    void LoadSceneActive(int sceneBuildIndex) => SceneManager.LoadSceneAsync(sceneBuildIndex, LoadSceneMode.Additive).completed += op => OnSceneLoadedActive(sceneBuildIndex);

    void OnSceneLoaded(int buildIndex)
    {
        if (buildIndex == _menuBuildIndex)
            _menuLoadComplete?.Invoke();

        if (buildIndex >= _levelBuildIndexOffset)
            _levelLoadComplete?.Invoke();

        _loadComplete?.Invoke(SceneManager.GetSceneByBuildIndex(buildIndex));
    }

    void OnSceneLoadedActive(int buildIndex)
    {
        Scene scene = SceneManager.GetSceneByBuildIndex(buildIndex);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(buildIndex));

        if (buildIndex == _menuBuildIndex)
            _menuLoadComplete?.Invoke();

        if (buildIndex >= _levelBuildIndexOffset)
            _levelLoadComplete?.Invoke();

        _loadComplete.Invoke(scene);
    }

    void LoadMenu()
    {
        void LoadInternal(AsyncOperation op)
        {
            LoadSceneActive(_menuBuildIndex);
            _currentScene = _menuBuildIndex;
        }

        if (_currentScene != -1)
        {
            SceneManager.UnloadSceneAsync(_currentScene).completed += LoadInternal;
            return;
        }

        LoadInternal(null);
    }

    void LoadGameScene()
    {
        void LoadInternal(AsyncOperation op)
        {
            int buildIndex = _levelBuildIndexOffset + _levelIndex.Value;

            LoadSceneActive(buildIndex);
            _currentScene = buildIndex;
        }

        if (_currentScene != -1)
        {
            AsyncOperation aop = SceneManager.UnloadSceneAsync(_currentScene);
            aop.completed += LoadInternal;
            return;
        }

        LoadInternal(null);
    }
}
