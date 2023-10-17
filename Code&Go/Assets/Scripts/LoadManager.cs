using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Settings;
using uAdventure.Simva;

/// <summary>
/// Manage the loading of the scenes
/// </summary>
public class LoadManager : MonoBehaviour
{
    public static LoadManager Instance;

    [SerializeField] private bool autoStart;
    public bool AutoStart {
        get { return autoStart; }
        set { autoStart = value; }
    }

    [SerializeField] private GameObject content;
    [SerializeField] private Text loadingText;
    [SerializeField] private float extraLoadingTime = 1.0f;

    private List<AsyncOperation> loadOperations = new List<AsyncOperation>();
    private int lastLoadedIndex = -1;

    private void Awake() {
        if (Instance) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    IEnumerator Start() {
        yield return SimvaExtension.Instance.OnAfterGameLoad();

        yield return WaitUntilLoadingIsComplete();

        if (autoStart && lastLoadedIndex == -1)
            LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void LateUpdate() {
        if (!content.activeInHierarchy) return;

        Color color = loadingText.color;
        color.a = 1.0f + Mathf.Sin(Time.timeSinceLevelLoad);
        loadingText.color = color;
    }

    public IEnumerator Unload() {
        content.SetActive(true);

        // Unload current Scene
        if (lastLoadedIndex != -1)
            loadOperations.Add(SceneManager.UnloadSceneAsync(lastLoadedIndex));

        yield return StartCoroutine(WaitUntilLoadingIsComplete());

        lastLoadedIndex = -1;
    }

    public void LoadScene(string sceneName) {
        content.SetActive(true);

        // Unload current Scene
        if (lastLoadedIndex != -1)
            loadOperations.Add(SceneManager.UnloadSceneAsync(lastLoadedIndex));

        // Load async 
        loadOperations.Add(SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive));

        StartCoroutine(WaitUntilLoadingIsComplete());

        lastLoadedIndex = SceneManager.GetSceneByName(sceneName).buildIndex;
    }

    public void LoadScene(int index) {
        content.SetActive(true);

        // Unload current Scene
        if (lastLoadedIndex != -1)
            loadOperations.Add(SceneManager.UnloadSceneAsync(lastLoadedIndex));

        // Load async 
        loadOperations.Add(SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive));

        StartCoroutine(WaitUntilLoadingIsComplete());

        lastLoadedIndex = index;
    }

    private IEnumerator WaitUntilLoadingIsComplete() {
        // Wait for scene loading operations
        for (int i = 0; i < loadOperations.Count; i++)
        {
            while(!loadOperations[i].isDone)
            {
                yield return null;
            }
        }
        loadOperations.Clear();

        // Wait for localization operations
        while (!LocalizationSettings.InitializationOperation.IsDone)
        {
            yield return null;
        }

        yield return new WaitForSeconds(extraLoadingTime);

        content.SetActive(false);

        if(!loadingText.gameObject.activeSelf)
            loadingText.gameObject.SetActive(true);
    }
}
