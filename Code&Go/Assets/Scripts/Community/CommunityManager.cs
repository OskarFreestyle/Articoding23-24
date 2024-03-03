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

    [SerializeField] private ActivatedScript activated;
    [SerializeField] private BrowseLevelsDisplay browseLevelsDisplay;

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


    #region Button Functions
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

        levelJson.articodingLevel.initialState = "";//levelDataSO.initialState.ToString();

        activated.Post("levels", JsonUtility.ToJson(levelJson), OnUploadOK, OnUploadKO);
        Debug.Log("CommunityManager Upload Level" + levelDataSO.levelName);

        //activatedScript.Post();
    }
    int OnUploadOK(UnityWebRequest req) {
        try
        {
            //levelsList = JsonUtility.FromJson<ServerClasses.Level>(levelsJson);
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

    public void showCreatedLevels() {

    }

    public void GetBrowseLevels() {
        // Obtain the levels from the server    

        activated.Get("levels?publicLevels=true&size=6", GetBrowseLevelsOK, GetBrowseLevelsKO);

        Debug.Log("askldaldlajdla" + browseLevelsDisplay);
       

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


        //clasesManager.CreatePublicLevels(publicLevels);

        return 0;
    }

    int GetBrowseLevelsKO(UnityWebRequest req) {
        Debug.Log("Error al obtener niveles publicos: " + req.responseCode);
        return 0;
    }


    #endregion
}
