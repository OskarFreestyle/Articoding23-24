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

    public bool AutoStart = true;

    public GameObject content;
    public Text loadingText;
    public float extraLoadingTime = 1.0f;

    private List<AsyncOperation> loadOperations;

    private int lastLoadedIndex = -1;

    IEnumerator Start()
    {
        Debug.Log("Load Manager Start");
        // TODO create instance in Awake
        if(Instance != null)
        {
            Destroy(gameObject);
            yield break;
        }

        Instance = this;
        //DontDestroyOnLoad(gameObject);
        loadOperations = new List<AsyncOperation>();

        /*if (!LocalizationSettings.InitializationOperation.IsDone)
            loadingText.gameObject.SetActive(false);*/


        yield return SimvaExtension.Instance.OnAfterGameLoad();

        yield return WaitUntilLoadingIsComplete();

        if (AutoStart && lastLoadedIndex == -1)
            LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void LateUpdate()
    {
        Debug.Log("Load Manager Late Update");

        if (!content.activeInHierarchy) return;

        Color color = loadingText.color;
        color.a = 1.0f + Mathf.Sin(Time.timeSinceLevelLoad);
        loadingText.color = color;
    }

    public IEnumerator Unload()
    {
        Debug.Log("Load Manager Late Unload");

        content.SetActive(true);

        //Unload current Scene
        if (lastLoadedIndex != -1)
            loadOperations.Add(SceneManager.UnloadSceneAsync(lastLoadedIndex));

        yield return StartCoroutine(WaitUntilLoadingIsComplete());

        lastLoadedIndex = -1;
    }

    public void LoadScene(string sceneName)
    {
        Debug.Log("Load Manager Late load string");

        content.SetActive(true);

        //Unload current Scene
        if (lastLoadedIndex != -1)
            loadOperations.Add(SceneManager.UnloadSceneAsync(lastLoadedIndex));

        //Load async 
        loadOperations.Add(SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive));

        StartCoroutine(WaitUntilLoadingIsComplete());

        lastLoadedIndex = SceneManager.GetSceneByName(sceneName).buildIndex;
    }
    public void LoadScene(int index) {
        Debug.Log("Load Manager Late load int");

        content.SetActive(true);

        //Unload current Scene
        if (lastLoadedIndex != -1)
            loadOperations.Add(SceneManager.UnloadSceneAsync(lastLoadedIndex));

        //Load async 
        loadOperations.Add(SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive));

        StartCoroutine(WaitUntilLoadingIsComplete());

        lastLoadedIndex = index;
    }

    private IEnumerator WaitUntilLoadingIsComplete()
    {
        Debug.Log("Load Manager Late load wait until complete");

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
