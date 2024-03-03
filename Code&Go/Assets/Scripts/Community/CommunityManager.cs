using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using static UnityEditor.ShaderData;
using System;

public class CommunityManager : MonoBehaviour {

    private static CommunityManager instance;
    static public CommunityManager Instance {
        get { return instance; }
    }

    [SerializeField] private ActivatedScript activated;
    [SerializeField] private BrowseLevelsDisplay browseLevelsDisplay;

    private List<ServerClasses.Level> levelsList;


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
        //ServerClasses.Level newLevel;
        //newLevel.name = levelDataSO.levelName;
        ServerClasses.Level levelJson = new ServerClasses.Level();

        levelJson.name = levelDataSO.name;
        levelJson.owner = LoginManager.user;
        

        activated.Post("levels", JsonUtility.ToJson(levelJson), OnUploadOK, OnUploadKO);
        Debug.Log("CommunityManager Upload Level");

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
        //activatedScript.Get("levels?publicLevels=true&size=6", GetBrowseLevelsOK, GetBrowseLevelsKO);

        // Load the levels
        browseLevelsDisplay.Configure();

    }

    int GetBrowseLevelsOK(UnityWebRequest req) {
        string levelsJson = req.downloadHandler.text;

        try {
            //levelsList = JsonUtility.FromJson<ServerClasses.Level>(levelsJson);
        }
        catch (System.Exception e) {
            Debug.Log("Error al leer niveles " + e);
        }

        return 0;
    }

    int GetBrowseLevelsKO(UnityWebRequest req) {
        Debug.Log("Error al obtener niveles publicos");
        return 0;
    }


    #endregion
}
