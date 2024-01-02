using System.Collections;
using System.Collections.Generic;
using System.IO;
using uAdventure.Simva;
using UnityEngine;

public class SaveManager {
    public static SaveManager Instance;

    private static string filename = "gameSave.save";
    private static string Filepath = "";

    public void Awake() {
        if (Instance == null) Instance = this;
    }

    public static void Init()
    {
        string dataPath =
#if UNITY_EDITOR
        Application.dataPath;
#else
        Application.persistentDataPath;
#endif
        string token = "";

        try
        {
            token = SimvaExtension.Instance.API.AuthorizationInfo.Username;
            token += "_";
        }
        catch(System.Exception e)
        {
            Debug.LogError(e.Message);
            token = "";
        }
        Filepath = Path.Combine(dataPath, token + filename);
        Debug.Log(Filepath);
    }

    public static void Load() {
        // Si no existe, se crea
        if (!File.Exists(Filepath)) {
            Debug.LogWarning("Archivo no encontrado");
            FileStream file = new FileStream(Filepath, FileMode.Create);
            file.Close();
            Save();
            return;
        }

        StreamReader reader = new StreamReader(Filepath);
        string readerData = reader.ReadToEnd();
        reader.Close();

        // Leemos
        SaveData data = JsonUtility.FromJson<SaveData>(readerData);

        // Verificamos
        if (Hash.ToHash(data.gameData.ToString(), "") == data.hash) {
            ProgressManager.Instance.Load(data.gameData.progressData);
            TutorialManager.Instance.Load(data.gameData.tutorialInfo);
        }

        // Se ha modificado el archivo, empiezas de 0
        Save();
    }

    public static void Save() {
        SaveData data = new SaveData();
        GameSaveData gameData = new GameSaveData();
        gameData.tutorialInfo = TutorialManager.Instance.Save();
        gameData.progressData = ProgressManager.Instance.Save();
        data.gameData = gameData;

        data.hash = Hash.ToHash(data.gameData.ToString(), "");

        string finalJson = JsonUtility.ToJson(data);
        // Se crea de nuevo
        FileStream file = new FileStream(Filepath, FileMode.Create);
        file.Close();

        StreamWriter writer = new StreamWriter(Filepath);
        writer.Write(finalJson);
        writer.Close();
    }
}
