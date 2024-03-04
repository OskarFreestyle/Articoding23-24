using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using static UnityEditor.ShaderData;
using System;
using static ServerClasses;


public class CommunityManager : MonoBehaviour {


    private static CommunityManager instance;
    static public CommunityManager Instance {
        get { return instance; }
    }

    [SerializeField] private bool isLogIn;
    [SerializeField] private ActivatedScript activatedScript;
    
    [Space][Space]

    [SerializeField] private RectTransform loginPage;
    [SerializeField] private RectTransform mainPage;
    [SerializeField] private RectTransform communityLevelsPage;
    [SerializeField] private RectTransform uploadLevelsPage;
    [SerializeField] private RectTransform communityPlaylistPage;
    [SerializeField] private RectTransform createPlaylistPage;
    [SerializeField] private RectTransform classesPage;

    private RectTransform currentPage;

    [Space][Space]

    [SerializeField] private UploadLevelsDisplay uploadLevelsDisplay;

    [Space][Space]

    [SerializeField] private BrowseLevelsDisplay browseLevelsDisplay;
    [SerializeField] private BrowseLevelsParams browseLevelsParams;

    private List<ServerClasses.Level> levelsList;
    private ServerClasses.LevelPage publicLevels;
    public ServerClasses.LevelPage PublicLevels
    {
        get
        {
            return publicLevels;
        }
    }

    private void Awake() {
        Debug.Log("Community Manager Awake");

        if (!instance) {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else {
            Debug.LogWarning("More than 1 Community Manager created");
            DestroyImmediate(gameObject);
        }

        Debug.Log("Community Manager Awake Finished");
    }

    public void Start() {
        // Make sure of the enable/disable pages
        currentPage = isLogIn ? mainPage : loginPage;
        loginPage.gameObject.SetActive(!isLogIn);
        mainPage.gameObject.SetActive(isLogIn);
        communityLevelsPage.gameObject.SetActive(false);
        uploadLevelsPage.gameObject.SetActive(false);
        //communityPlaylistPage.gameObject.SetActive(false);
        //createPlaylistPage.gameObject.SetActive(false);
        //classesPage.gameObject.SetActive(false);
    }   


    #region Button Functions
    public void ChangeEnablePage(RectTransform enablePage) {
        currentPage.gameObject.SetActive(false);
        currentPage = enablePage;
        currentPage.gameObject.SetActive(true);
    }

    public void GoToUploadLevelsPage() {
        ChangeEnablePage(uploadLevelsPage);

        // Instance the created levels
        uploadLevelsDisplay.InstanciateCreatedLevels();
    }

    public void GoToCommunityLevelsPage() {
        ChangeEnablePage(communityLevelsPage);

        // Do a basic search
        GetBrowseLevels();
    }

    public void GoToMainPage() {
        ChangeEnablePage(mainPage);
    }

    public void UploadLevel(LevelDataSO levelDataSO) {

        ServerClasses.PostedLevel levelJson = new ServerClasses.PostedLevel();

        levelJson.title = levelDataSO.levelName;
        levelJson.classes = new List<int>();
        levelJson.hashtagsIDs = new List<int>();
        levelJson.publicLevel = true;

        levelJson.articodingLevel = new ServerClasses.ArticodingLevel();

        ActiveBlocks thisActiveBlocks = ActiveBlocks.FromJson(levelDataSO.activeBlocks.text);
        levelJson.articodingLevel.activeblocks = thisActiveBlocks;

        BoardState thisBoardState = BoardState.FromJson(levelDataSO.levelBoard.text);
        levelJson.articodingLevel.boardstate = thisBoardState;

        levelJson.articodingLevel.initialState = "";

        activatedScript.Post("levels", JsonUtility.ToJson(levelJson), OnUploadOK, OnUploadKO);
    }

    int OnUploadOK(UnityWebRequest req) {
        try
        {
            Debug.Log("Level Upload Correctly");
        }
        catch (System.Exception e)
        {
            Debug.Log("Error al subir nivel " + e);
        }

        return 0;
    }

    int OnUploadKO(UnityWebRequest req) {
        Debug.Log("Error al obtener niveles publicos");
        return 0;
    }

    public void GetBrowseLevels() {
        Debug.Log("Search: " + browseLevelsParams.GetParams());
        activatedScript.Get(browseLevelsParams.GetParams(), GetBrowseLevelsOK, GetBrowseLevelsKO); // "levels?publicLevels=true&size=6"
    }

    int GetBrowseLevelsOK(UnityWebRequest req) {
        string levelsJson = req.downloadHandler.text;
        Debug.Log("Entra en BrowseLevelsOK");

        try
        {
            publicLevels = JsonUtility.FromJson<ServerClasses.LevelPage>(levelsJson);
            browseLevelsDisplay.Configure();
        }
        catch (System.Exception e)
        {
            Debug.Log("Error al leer niveles " + e);
        }

        return 0;
    }

    int GetBrowseLevelsKO(UnityWebRequest req) {
        Debug.Log("Error al obtener niveles publicos: " + req.responseCode);
        return 0;
    }
    #endregion
}
