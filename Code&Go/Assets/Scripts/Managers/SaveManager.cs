using System.Collections;
using System.Collections.Generic;
using System.IO;
using uAdventure.Simva;
using UnityEngine;



using UnityEngine.UI;

/// <summary>
/// Manage the save and load data
/// </summary>
public class SaveManager : MonoBehaviour {

    #region Properties
    public static SaveManager instance;
    static public SaveManager Instance {
        get { return instance; }
    }

    [SerializeField] private string filename = "gameSave.json";
    [SerializeField] private string filename2 = "gameSave2.json";

    private string Filepath = "";
    private string Filepath2 = "";
    #endregion

    #region Methods
    public void Awake() {
        if (!instance) {
            instance = this;
            DontDestroyOnLoad(this);
            test();
            Init();
        }
        else {
            Debug.LogWarning("More than 1 Save Manager created");
            DestroyImmediate(gameObject);
        }
    }

    public Text text;

    private void test() {
        string aux = "-";
        if (ProgressManager.Instance) aux += "Progress-";
        if (TutorialManager.Instance) aux += "Tutorial-";
        text.text = aux;
    }

    public void Init() {
        string dataPath =
#if UNITY_EDITOR
        Application.dataPath;
#else
        Application.persistentDataPath;
#endif
        string token = "";

        try {
            token = SimvaExtension.Instance.API.AuthorizationInfo.Username;
            token += "_";
        }
        catch (System.Exception e) {
            Debug.LogError("SimvaExtension: " + e.Message);
            token = "";
        }

        Filepath = Path.Combine(dataPath, token + filename);
        Filepath2 = Path.Combine(dataPath, token + filename2);
    }

    public void Load() {
        Debug.Log("Save Manager - Load data");

        // Si no existe, se crea
        if (!File.Exists(Filepath)) {
            Debug.LogWarning("File not found, creating a new one");
            FileStream file = new FileStream(Filepath, FileMode.Create);
            file.Close();
            Save();
            return;
        }
        else {
            Debug.Log("File found");
        }

        StreamReader reader = new StreamReader(Filepath);
        string readerData = reader.ReadToEnd();
        reader.Close();
        SaveData data = JsonUtility.FromJson<SaveData>(readerData);

        // Check the hash TODO active
        //if (Hash.ToHash(data.gameData.ToString(), "") == data.hash) {
            ProgressManager.Instance.Load(data.gameData.progressData);
            TutorialManager.Instance.Load(data.gameData.tutorialInfo);
        //}

        // Se ha modificado el archivo, empiezas de 0
        Save();
    }

    public void Save() {
        Debug.Log("Save Manager - Save data");

        // Save the game data
        GameSaveData gameData = new GameSaveData();
        if(ProgressManager.Instance) gameData.progressData = ProgressManager.Instance.Save();
        if(TutorialManager.Instance) gameData.tutorialInfo = TutorialManager.Instance.Save();
        
        // Add the hash to the save data
        SaveData data = new SaveData();
        data.gameData = gameData;
        data.hash = Hash.ToHash(data.gameData.ToString(), "");

        // Parse to json
        string finalJson = JsonUtility.ToJson(data);

        // Create a new file
        FileStream file = new FileStream(Filepath, FileMode.Create);
        file.Close();

        // Write the new file
        StreamWriter writer = new StreamWriter(Filepath);
        writer.Write(finalJson);
        writer.Close();

        // Testing TODO quitar
        FileStream file2 = new FileStream(Filepath2, FileMode.Create, FileAccess.ReadWrite);
        file2.Close();
        StreamWriter writer2 = new StreamWriter(Filepath2);


        if (ProgressManager.Instance) writer2.Write("Progress Manager Instance: true");
        if (TutorialManager.Instance) writer2.Write("Tutorial Manager Instance: true");
        writer2.Write("Categories lenght:" + data.gameData.progressData.categoriesInfo.Length);
        writer2.Write("Tutorial lenght:" + data.gameData.tutorialInfo.tutorials.Length);


        writer2.Close();
    }
    #endregion

}
