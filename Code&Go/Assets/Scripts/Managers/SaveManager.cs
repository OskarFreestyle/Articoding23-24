using System.Collections;
using System.Collections.Generic;
using System.IO;
using uAdventure.Simva;
using UnityEngine;

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

    private string Filepath = "";
    #endregion

    #region Methods
    public void Awake() {
        if (!instance) {
            instance = this;
            DontDestroyOnLoad(this);
            Init();
        }
        else {
            Debug.LogWarning("More than 1 Save Manager created");
            DestroyImmediate(gameObject);
        }
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
        catch(System.Exception e) {
            Debug.LogError("SimvaExtension: " + e.Message);
            token = "";
        }

        Filepath = Path.Combine(dataPath, token + filename);
    }

    public void Load() {
        Debug.Log("Load Save Data");

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
        // Save the game data
        GameSaveData gameData = new GameSaveData();
        gameData.tutorialInfo = TutorialManager.Instance.Save();
        gameData.progressData = ProgressManager.Instance.Save();
        
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
    }
    #endregion

}
