using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class ComunidadLayout : MonoBehaviour {

    public GameObject clasesLayout;
    public GameObject nivelesLayout;
    public GameObject notLoggedLayout;
    public GameObject infoZone;
    public GameObject refreshButton;
    public GameObject addClassButton;

    public GameObject clasesList;
    public GameObject savePanel;

    public ActivatedScript activatedScript;

    ServerClasses.ClaseJSON clases;
    ServerClasses.LevelPage publicLevels;

    public ClasesManager clasesManager;

    public GameObject clasePrefab;

    public Text username;
    public Text userRole;

    bool alreadyLogged = false;

    private Texture2D levelIconImage;

    private void OnEnable()
    {
        if(!GameManager.Instance.GetLogged()) {
            IsNotLoggedAction();
        }
        else {
            IsLoggedAction();
            CreateClasses();
        }
    }

    public void IsLoggedAction() {
        clasesLayout.SetActive(true);
        nivelesLayout.SetActive(false);
        notLoggedLayout.SetActive(false);
        infoZone.SetActive(true);
        refreshButton.SetActive(true);

        username.text = GameManager.Instance.GetUserName();
        if (GameManager.Instance.GetIsAdmin()) {
            userRole.text = "Profesor";
            addClassButton.SetActive(true);
        }
        else userRole.text = "Alumno";

        ReadLevelFiles();

        CreateClasses();
    }

    public void IsNotLoggedAction() {
        clasesLayout.SetActive(false);
        nivelesLayout.SetActive(false);
        notLoggedLayout.SetActive(true);
        infoZone.SetActive(false);
        refreshButton.SetActive(false);
        username.text = "";
        userRole.text = "";
    }

    void CreateClasses() {
        if (!alreadyLogged) {
            clasesManager.ReadCreatedLevels();
            activatedScript.Get("classes", GetClassesOK, GetClassesKO);
            activatedScript.Get("levels?publicLevels=true&size=6", GetPublicLevelsOK, GetPublicLevelsKO);
            alreadyLogged = true;
        }
    }

    int GetClassesOK(UnityWebRequest req) {
        string clasesJson = req.downloadHandler.text;

        try {
            clases = JsonUtility.FromJson<ServerClasses.ClaseJSON>(clasesJson);
        }
        catch (System.Exception e)
        {
            Debug.Log("Error al leer clases " + e);
        }

        for (int i = 0; i < clases.content.Count; i++)
        {
            var newclase = Instantiate(clasePrefab, clasesList.transform);
            clasesManager.clases.Add(newclase);
            newclase.GetComponent<ClasePrefab>().SetupClase(clases.content[i]);
        }

        clasesManager.ConfigureTabs();

        return 0;
    }

    int GetClassesKO(UnityWebRequest req)
    {
        Debug.Log("Error al obtener clases");
        return 0;
    }

    int GetPublicLevelsOK(UnityWebRequest req) {
        string nivelesJson = req.downloadHandler.text;

        try {
            publicLevels = JsonUtility.FromJson<ServerClasses.LevelPage>(nivelesJson);
        }
        catch (System.Exception e) {
            Debug.Log("Error al leer niveles " + e);
        }

        clasesManager.CreatePublicLevels(publicLevels);

        return 0;
    }

    int GetPublicLevelsKO(UnityWebRequest req) {
        Debug.Log("Error al obtener niveles publicos");
        return 0;
    }

    public void PlayCommunityLevel() {
        GameManager.Instance.LoadCommunityLevel();

        clasesManager.SetCommunityLevel();

        if (LoadManager.Instance == null) {
            SceneManager.LoadScene("LevelScene");
            return;
        }

        LoadManager.Instance.LoadScene("LevelScene");
    }

    public void PlayPublicLevel(ServerClasses.Level level) {
        GameManager.Instance.LoadCommunityLevel();

        clasesManager.SetPublicLevel(level);

        if (LoadManager.Instance == null) {
            SceneManager.LoadScene("LevelScene");
            return;
        }

        LoadManager.Instance.LoadScene("LevelScene");
    }

    public void SaveCommunityLevel() {
        ServerClasses.Level theLevel = clasesManager.GetCommunityLevel();

        ProgressManager.Instance.UserCreatedLevel(theLevel.articodingLevel.boardstate.ToJson(), theLevel.articodingLevel.activeblocks.ToJson(), theLevel.articodingLevel.initialState, levelIconImage, theLevel.title, 7);

        savePanel.SetActive(true);
    }

    public void SavePublicLevel(ServerClasses.Level theLevel) {
        ProgressManager.Instance.UserCreatedLevel(theLevel.articodingLevel.boardstate.ToJson(), theLevel.articodingLevel.activeblocks.ToJson(), theLevel.articodingLevel.initialState, levelIconImage, theLevel.title, 7);

        savePanel.SetActive(true);
    }

    public void RefreshClases() {
        //Eliminamos todas las clases y las volvemos a llamar
        for (var i = clasesList.transform.childCount - 1; i >= 1; i--) {
            Destroy(clasesList.transform.GetChild(i).gameObject);
        }

        clasesManager.DeleteLists();

        activatedScript.Get("classes", GetClassesOK, GetClassesKO);
    }

    public ServerClasses.LevelPage GetPublicLevels() {
        return publicLevels;
    }

    public void SetPublicLevels(ServerClasses.LevelPage lvls) {
        publicLevels = lvls;
    }

    void ReadLevelFiles() {
        string path = Application.dataPath;
        //Creamos las carpetas pertinentes si no estan creadas
        if (!Directory.Exists(path + "/Resources/Levels/"));
            Directory.CreateDirectory(path + "/Resources/Levels/");
        if (!Directory.Exists(path + "/Resources/Levels/Boards/"))
            Directory.CreateDirectory(path + "/Resources/Levels/Boards/");
        if (!Directory.Exists(path + "/Resources/Levels/Boards/8_CreatedLevels/"))
            Directory.CreateDirectory(path + "/Resources/Levels/Boards/8_CreatedLevels/");
        if (!Directory.Exists(path + "/Resources/Levels/ActiveBlocks/"))
            Directory.CreateDirectory(path + "/Resources/Levels/ActiveBlocks/");
        if (!Directory.Exists(path + "/Resources/Levels/ActiveBlocks/8_CreatedLevels/"))
            Directory.CreateDirectory(path + "/Resources/Levels/ActiveBlocks/8_CreatedLevels/");
    }
}
