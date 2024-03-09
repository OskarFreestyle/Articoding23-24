using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using static UnityEditor.ShaderData;
using System;
using static ServerClasses;
using System.IO;
using UnityEditor;

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

    private List<int> likedLevelIDs = new List<int>();


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

        levelJson.articodingLevel.initialState = "";    // TODO subir el initial state

        // Level image
        levelJson.image = levelDataSO.levelImage.texture.EncodeToPNG();

        Debug.Log(levelJson.image);

        //Debug.Log("Subiendo el nivel image lenght: " + levelJson.image.Length);
        //Debug.Log("Starts with " + levelJson.image[0] + levelJson.image[1] + levelJson.image[2] + levelJson.image[3] + levelJson.image[4] + levelJson.image[5] + levelJson.image[6]+ "...");

        activatedScript.Post("levels", JsonUtility.ToJson(levelJson), OnUploadOK, OnUploadKO);
    }

    int OnUploadOK(UnityWebRequest req) {
        Debug.Log("OnUploadOK");
        return 0;
    }

    int OnUploadKO(UnityWebRequest req) {
        Debug.Log("OnUploadKO");
        return 0;
    }

    public void GetBrowseLevels() {
        Debug.Log("Search: " + browseLevelsParams.GetParams());
        activatedScript.Get(browseLevelsParams.GetParams(), GetBrowseLevelsOK, GetBrowseLevelsKO); // "levels?publicLevels=true&size=6"
    }

    int GetBrowseLevelsOK(UnityWebRequest req) {
        Debug.Log("GetBrowseLevelsOK");
        try
        {
            string levelsJson = req.downloadHandler.text;
            publicLevels = JsonUtility.FromJson<ServerClasses.LevelPage>(levelsJson);
            browseLevelsDisplay.Configure();
        }
        catch (System.Exception e)
        {
            Debug.Log("Error in GetBrowseLevelsOK" + e);
        }

        return 0;
    }

    int GetBrowseLevelsKO(UnityWebRequest req) {
        Debug.Log("Error al obtener niveles publicos: " + req.responseCode);
        return 0;
    }

    public void IncreasePlays(string levelID) {
        string path = "levels/" + levelID + "/play";
        ServerClasses.PostedLevel postedLevel = new ServerClasses.PostedLevel();
        activatedScript.Post(path, JsonUtility.ToJson(postedLevel), IncreasePlaysOK, IncreasePlaysKO);
    }

    int IncreasePlaysOK(UnityWebRequest req) {
        Debug.Log("LikeLevelOK");
        return 0;
    }

    int IncreasePlaysKO(UnityWebRequest req) {
        Debug.Log("LikeLevelKO");
        return 0;
    }

    public void ModifyLikes(string levelID, bool liked) {

        if(liked) likedLevelIDs.Add(int.Parse(levelID));
        else likedLevelIDs.Remove(int.Parse(levelID));

        string path = "levels/" + levelID;
        if (liked) path += "/increaselikes";
        else path += "/decreaselikes";
        ServerClasses.PostedLevel postedLevel = new ServerClasses.PostedLevel();
        activatedScript.Post(path, JsonUtility.ToJson(postedLevel), ModifyLikesOK, ModifyLikesKO);
    }

    int ModifyLikesOK(UnityWebRequest req) {
        Debug.Log("ModifyLikesOK");
        return 0;
    }

    int ModifyLikesKO(UnityWebRequest req) {
        Debug.Log("ModifyLikesKO");
        return 0;
    }

    public void GetUserLikedLevels() {
        string path = "users/getLiked/" + GameManager.Instance.GetUserName();
        activatedScript.Get(path, GetUserLikedLevelsOK, GetUserLikedLevelsKO);
    }

    int GetUserLikedLevelsOK(UnityWebRequest req) {
        Debug.Log("GetUserLikedLevelsOK");
        try {
            string levelsLiked = req.downloadHandler.text; // GetUserLikedLevels: {  10, 12, 412, 15};
            likedLevelIDs = JsonUtility.FromJson<ServerClasses.User>(levelsLiked).likedLevels;
        }
        catch (System.Exception e) {
            Debug.Log("Error in GetBrowseLevelsOK" + e);
        }

        return 0;
    }

    int GetUserLikedLevelsKO(UnityWebRequest req) {
        Debug.Log("GetUserLikedLevelsKO");
        return 0;
    }

    #endregion
}
