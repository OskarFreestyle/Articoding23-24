using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
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
    [SerializeField] private RectTransform classLevelsPage;
    private RectTransform currentPage;

    [Space][Space]

    [SerializeField] private UploadLevelsDisplay uploadLevelsDisplay;
    [SerializeField] private AddedLevelsPlaylistDisplay addedLevelsPlaylistDisplay;

    // Community Levels
    [Space][Space]
    [SerializeField] private BrowseLevelsDisplay browseLevelsDisplay;
    [SerializeField] private BrowseLevelsParams browseLevelsParams;

    private ServerClasses.LevelPage publicLevels;
    public ServerClasses.LevelPage PublicLevels
    {
        get
        {
            return publicLevels;
        }
    }

    private ServerClasses.LevelPage uploadedLevels;
    public ServerClasses.LevelPage UploadedLevels {
        get {
            return uploadedLevels;
        }
    }

    // Community Playlists
    [Space][Space]
    [SerializeField] private BrowsePlaylistsDisplay browsePlaylistsDisplay;
    [SerializeField] private BrowsePlaylistParams browsePlaylistsParams;
    private ServerClasses.PlaylistPage publicPlaylists;
    public ServerClasses.PlaylistPage PublicPlaylists {
        get {
            return publicPlaylists;
        }
    }



    private bool isPlaylistMode;
    public bool IsPlaylistMode { get { return isPlaylistMode; } }

    private List<int> creatingPlaylistIDs = new List<int>();
    [SerializeField] private InputField creatingPlaylistName;


    // Classes
    [Space][Space]
    [SerializeField] private ClassesDisplay classesDisplay;
    private ServerClasses.ClaseJSON classes;
    public ServerClasses.ClaseJSON Classes {
        get {
            return classes;
        }
    }
    [SerializeField] private ClassesLevelsDisplay classesLevelsDisplay;
    private ServerClasses.LevelPage classLevels;
    public ServerClasses.LevelPage ClassLevels {
        get {
            return classLevels;
        }
    }

    // Join class
    [SerializeField] private InputField classKey;

    [SerializeField] private ReplyMessage replyMessage;
    [SerializeField] private LoadingCircle loadingCircle;


    private void Awake() {
        Debug.Log("Community Manager Awake");

        if (!instance) {
            instance = this;
            //DontDestroyOnLoad(this);
        }
        else {
            Debug.LogWarning("More than 1 Community Manager created");
            DestroyImmediate(gameObject);
        }

        Debug.Log("Community Manager Awake Finished");
    }

    public void Start() {
        // Make sure of the enable/disable pages
        isLogIn = GameManager.Instance.GetLogged();
        currentPage = isLogIn ? mainPage : loginPage;
        loginPage.gameObject.SetActive(!isLogIn);
        mainPage.gameObject.SetActive(isLogIn);
        communityLevelsPage.gameObject.SetActive(false);
        uploadLevelsPage.gameObject.SetActive(false);
        communityPlaylistPage.gameObject.SetActive(false);
        createPlaylistPage.gameObject.SetActive(false);
        classesPage.gameObject.SetActive(false);
        classLevelsPage.gameObject.SetActive(false);
    }   


    #region Button Functions
    public void ChangeEnablePage(RectTransform enablePage) {
        Debug.Log("Current page before: " + currentPage.name);
        currentPage.gameObject.SetActive(false);
        currentPage = enablePage;
        currentPage.gameObject.SetActive(true);
        Debug.Log("Current page after: " + currentPage.name);
    }

    public void GoToUploadLevelsPage() {
        ChangeEnablePage(uploadLevelsPage);

        // Get the already uploaded levels
        activatedScript.Get("levels?size=10&publicLevels=true&owner=" + GameManager.Instance.GetUserName(), GetUserLevelsOK, GetUserLevelsKO);
        loadingCircle.Show(true);
        uploadLevelsDisplay.ClearDisplay();
    }

    private int GetUserLevelsOK(UnityWebRequest req) {
        Debug.Log("GetUserLevelsOK");
        try {
            string levelsJson = req.downloadHandler.text;
            uploadedLevels = JsonUtility.FromJson<ServerClasses.LevelPage>(levelsJson);
            // Instance the created levels
            uploadLevelsDisplay.InstanciateCreatedLevels();
        }
        catch (System.Exception e) {
            Debug.Log("Error in GetUserLevelsOK" + e);
        }
        return 0;
    }

    private int GetUserLevelsKO(UnityWebRequest req) {
        Debug.Log("GetUserLevelsKO");
        return 0;
    }

    public void GoToCreatePlaylistPage() {
        isPlaylistMode = true;

        ChangeEnablePage(createPlaylistPage);
    }

    public void GoToCommunityLevelsPage() {
        ChangeEnablePage(communityLevelsPage);

        // Do a basic search
        browseLevelsDisplay.SetPlaylistState(false);
        GetBrowseLevels();
    }

    public void GoToCommunityPlaylistPage() {
        ChangeEnablePage(communityPlaylistPage);

        // Do a basic search
        GetBrowsePlaylists();
    }

    public void GoToClassesPage() {
        ChangeEnablePage(classesPage);

        // Get the user classes
        activatedScript.Get("classes", GetClassesOK, GetClassesKO);
    }

    private int GetClassesOK(UnityWebRequest req) {
        Debug.Log("GetClassesOK");

        try {
            string levelsJson = req.downloadHandler.text;
            classes = JsonUtility.FromJson<ServerClasses.ClaseJSON>(levelsJson);
            // Instance the created levels
            classesDisplay.InstanciateClassCards();
        }
        catch (System.Exception e) {
            Debug.Log("Error in GetClassesOK" + e);
        }

        return 0;
    }

    private int GetClassesKO(UnityWebRequest req) {
        Debug.Log("GetClassesKO");

        return 0;
    }

    public void PlayClass(ServerClasses.Clase clas) {
        Debug.Log("Play Class " + clas.name);

        // Get the class levels
        string path = "levels?class=" + clas.id.ToString();
        activatedScript.Get(path, GetClassLevelsOK, GetClassLevelsKO);
    }

    public int GetClassLevelsOK(UnityWebRequest req) {
        Debug.Log("GetClassLevelsOK");

        try {
            ChangeEnablePage(classLevelsPage);
            string levelsJson = req.downloadHandler.text;
            classLevels = JsonUtility.FromJson<ServerClasses.LevelPage>(levelsJson);

            // Instance the created levels
            classesLevelsDisplay.InstanciateClassLevels();
        }
        catch (System.Exception e) {
            Debug.Log("Error in GetClassesOK" + e);
        }
        return 0;
    }

    public int GetClassLevelsKO(UnityWebRequest req) {
        Debug.Log("GetClassLevelsKO");

        return 0;
    }

    public void BackButtonLevelsDisplay() {
        if (isPlaylistMode) {
            GoToCreatePlaylistPage();
        }
        else GoToMainPage();
    }

    public void GoToMainPage() {
        isPlaylistMode = false;
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

        activatedScript.Post("levels", JsonUtility.ToJson(levelJson), OnUploadOK, OnUploadKO);
    }

    int OnUploadOK(UnityWebRequest req) {
        Debug.Log("OnUploadOK");
        replyMessage.Configure(MessageReplyID.LevelUploadedCorrectly);
        return 0;
    }

    int OnUploadKO(UnityWebRequest req) {
        Debug.Log("OnUploadKO");
        replyMessage.Configure(MessageReplyID.ErrorUploadingLevel);
        return 0;
    }
    public void HideLoadingCircle() {
        loadingCircle.Hide();
    }

    public void GetBrowseLevels() {
        Debug.Log("Search: " + browseLevelsParams.GetParams());
        activatedScript.Get(browseLevelsParams.GetParams(), GetBrowseLevelsOK, GetBrowseLevelsKO);
        browseLevelsParams.ResetParams();
        browseLevelsDisplay.ClearDisplay();
        loadingCircle.Show(true);
    }

    int GetBrowseLevelsOK(UnityWebRequest req) {
        Debug.Log("GetBrowseLevelsOK");
        try {
            publicLevels = JsonUtility.FromJson<ServerClasses.LevelPage>(req.downloadHandler.text);
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
        loadingCircle.Hide();
        return 0;
    }

    public void GetBrowsePlaylists() {
        Debug.Log("Search playlist: " + browsePlaylistsParams.GetParams());
        activatedScript.Get(browsePlaylistsParams.GetParams(), GetBrowsePlaylistsOK, GetBrowsePlaylistsKO);
        browsePlaylistsParams.ResetParams();
        browsePlaylistsDisplay.ClearDisplay();
        loadingCircle.Show(false);
    }

    int GetBrowsePlaylistsOK(UnityWebRequest req) {
        Debug.Log("GetBrowsePlaylistsOK");
        try {
            publicPlaylists = JsonUtility.FromJson<ServerClasses.PlaylistPage>(req.downloadHandler.text);
            browsePlaylistsDisplay.Configure();
        }
        catch (System.Exception e) {
            Debug.Log("Error in GetBrowsePlaylistsOK" + e);
        }

        return 0;
    }

    int GetBrowsePlaylistsKO(UnityWebRequest req) {
        Debug.Log("Error al obtener playlist publicos: " + req.responseCode);
        loadingCircle.Hide();
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

    public void IncreasePlaysPlaylist(string playlistID) {
        string path = "playlists/" + playlistID + "/play";
        ServerClasses.PostedPlaylist postedPlaylist = new ServerClasses.PostedPlaylist();
        activatedScript.Post(path, JsonUtility.ToJson(postedPlaylist), IncreasePlaysPlaylistOK, IncreasePlaysPlaylistKO);
    }

    int IncreasePlaysPlaylistOK(UnityWebRequest req) {
        Debug.Log("IncreasePlaysPlaylistOK");
        return 0;
    }

    int IncreasePlaysPlaylistKO(UnityWebRequest req) {
        Debug.Log("IncreasePlaysPlaylistKO");
        return 0;
    }

    public void ModifyLikesLevel(string levelID, bool liked) {

        if(liked) GameManager.Instance.LikedLevelIDs.Add(int.Parse(levelID));
        else GameManager.Instance.LikedLevelIDs.Remove(int.Parse(levelID));

        string path = "levels/" + levelID;
        if (liked) path += "/increaselikes";
        else path += "/decreaselikes";
        ServerClasses.PostedLevel postedLevel = new ServerClasses.PostedLevel();
        activatedScript.Post(path, JsonUtility.ToJson(postedLevel), ModifyLikesLevelOK, ModifyLikesLevelKO);
    }

    int ModifyLikesLevelOK(UnityWebRequest req) {
        Debug.Log("ModifyLikesOK");
        return 0;
    }

    int ModifyLikesLevelKO(UnityWebRequest req) {
        Debug.Log("ModifyLikesKO");
        return 0;
    }

    public void ModifyLikesPlaylist(string playlistID, bool liked) {
        if (liked) GameManager.Instance.LikedPlaylistIDs.Add(int.Parse(playlistID));
        else GameManager.Instance.LikedPlaylistIDs.Remove(int.Parse(playlistID));

        string path = "playlists/" + playlistID;
        if (liked) path += "/increaselikes";
        else path += "/decreaselikes";
        ServerClasses.PostedPlaylist postedPlaylist = new ServerClasses.PostedPlaylist();
        activatedScript.Post(path, JsonUtility.ToJson(postedPlaylist), ModifyLikesPlaylistOK, ModifyLikesPlaylistKO);
    }

    int ModifyLikesPlaylistOK(UnityWebRequest req) {
        Debug.Log("ModifyLikesPlaylistOK");
        return 0;
    }

    int ModifyLikesPlaylistKO(UnityWebRequest req) {
        Debug.Log("ModifyLikesPlaylistKO");
        return 0;
    }

    public void GetUserLikedLevels() {
        Debug.Log("GetUserLikedLevels()");
        string path = "users/getliked";
        activatedScript.Get(path, GetUserLikedLevelsOK, GetUserLikedLevelsKO);
    }

    int GetUserLikedLevelsOK(UnityWebRequest req) {
        Debug.Log("GetUserLikedLevelsOK");
        try {
            string levelsLiked = req.downloadHandler.text; // GetUserLikedLevels: {  10, 12, 412, 15};
            GameManager.Instance.LikedLevelIDs = JsonUtility.FromJson<ServerClasses.User>(levelsLiked).likedLevels; //Aquí ya te debería llegar los liked tranquilamente
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

    public void GetUserLikedPlaylist() {
        Debug.Log("GetUserLikedPlaylist()");
        Debug.LogError("Cesar aqui no se que poner para que me de las playlist con me gustas");
        //string path = "users/getliked";
        //activatedScript.Get(path, GetUserLikedPlaylistOK, GetUserLikedPlaylistKO);
    }

    int GetUserLikedPlaylistOK(UnityWebRequest req) {
        Debug.Log("GetUserLikedPlaylistOK");
        try {
            string playlistLiked = req.downloadHandler.text; // GetUserLikedLevels: {  10, 12, 412, 15};
            //GameManager.Instance.LikedPlaylistIDs = JsonUtility.FromJson<ServerClasses.User>(playlistLiked).likedPlaylist; //Aquí ya te debería llegar los liked tranquilamente
        }
        catch (System.Exception e) {
            Debug.Log("Error in GetUserLikedPlaylistOK" + e);
        }

        return 0;
    }

    int GetUserLikedPlaylistKO(UnityWebRequest req) {
        Debug.Log("GetUserLikedPlaylistKO");
        return 0;
    }

    public void CreatePlaylist() {
        Debug.Log("Create playlist");
        
        // Check the playlist name
        if(creatingPlaylistName.text.Length == 0) {
            replyMessage.Configure(MessageReplyID.ShortPlaylistNameError);
            return;
        }

        if (creatingPlaylistName.text.Length > 16) {
            replyMessage.Configure(MessageReplyID.LongPlaylistNameError);
            return;
        }

        if (creatingPlaylistIDs.Count == 0) {
            replyMessage.Configure(MessageReplyID.ErrorPlaylistEmpty);
            return;
        }

        ServerClasses.PostedPlaylist playlistJson = new ServerClasses.PostedPlaylist();

        playlistJson.title = creatingPlaylistName.text;
        playlistJson.levels = creatingPlaylistIDs;

        activatedScript.Post("playlists", JsonUtility.ToJson(playlistJson), OnCreateOK, OnCreateKO);
    }

    int OnCreateOK(UnityWebRequest req) {
        Debug.Log("OnCreateOK");
        replyMessage.Configure(MessageReplyID.SuccessfulCreatedPlaylist);
        return 0;
    }

    int OnCreateKO(UnityWebRequest req) {
        Debug.Log("OnCreateKO");
        replyMessage.Configure(MessageReplyID.ErrorCreatingPlaylist);
        return 0;
    }

    public void AddToCreatingPlaylist(ServerClasses.LevelWithImage lWI) {
        Debug.Log("AddToCreatingPlaylist");

        // By the moment check to not add the same level two times (ese nivel no tenía que salir en la búsqueda directamente)
        if (!creatingPlaylistIDs.Contains(lWI.level.id)) {

            // Save the level id
            creatingPlaylistIDs.Add(lWI.level.id);

            // Create the level target contract
            addedLevelsPlaylistDisplay.AddLevel(lWI);
        }
        else Debug.Log("Nivel ya repetido");

        // Return to create playlist 
        GoToCreatePlaylistPage();
    }

    public void RemoveFromPlaylist(int levelID) {
        creatingPlaylistIDs.Remove(levelID);
    }

    public void OpenPlaylist(ServerClasses.Playlist p) {
        // Change the scene
        ChangeEnablePage(communityLevelsPage);

        // Change the content of the page
        publicLevels = new ServerClasses.LevelPage();
        publicLevels.content = p.levelsWithImage;

        // Configure the levels
        browseLevelsDisplay.ClearDisplay();
        browseLevelsDisplay.Configure();
        browseLevelsDisplay.SetPlaylistState(true, p.title);

        IncreasePlaysPlaylist(p.id.ToString());
    }

    public void ChangeUserProfilePic(int id) {
        string path = "users/" + GameManager.Instance.GetUserName();
        ServerClasses.PostedUser postedUser = new ServerClasses.PostedUser();
        postedUser.imageIndex = id;
        activatedScript.Post("users/", JsonUtility.ToJson(postedUser), OnChangeUserProfilePicOK, OnChangeUserProfilePicKO);
    }

    public int OnChangeUserProfilePicOK(UnityWebRequest req) {
        Debug.Log("OnChangeUserProfilePicOK");
        return 0;
    }

    public int OnChangeUserProfilePicKO(UnityWebRequest req) {
        Debug.Log("OnChangeUserProfilePicKO");
        return 0;
    }

    public void JoinClass() {
        string path = "/enterclass/" + classKey.text;
        activatedScript.Post(path, JsonUtility.ToJson(classKey.text), OnJoinClassOK, OnJoinClassKO);
    }

    public int OnJoinClassOK(UnityWebRequest req) {
        Debug.Log("OnJoinClassOK");
        replyMessage.Configure(MessageReplyID.AddedClassSuccessfully);
        return 0;
    }

    public int OnJoinClassKO(UnityWebRequest req) {
        Debug.Log("OnJoinClassKO");
        replyMessage.Configure(MessageReplyID.ClassNotFound);
        return 0;
    }
    #endregion
}
