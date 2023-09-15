using AssetPackage;
using Simva.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private List <Category> categories;
    [SerializeField] private bool loadSave = true;

    [SerializeField] private Category category;
    public int levelIndex;
    private bool gameLoaded = false;
    private Dictionary<UBlockly.Block, string> blockIDs;

    //Variables de conexion
    bool loggedIn;
    string token;
    bool isAdmin = false;
    string userName = "";
    bool playingCommunityLevel = false;

    BoardState communityBoard = null;
    ActiveBlocks communityActiveBlocks = null;
    string communityInitialState = null;
    string levelName = "";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);            
            SaveManager.Init();
        }
        else
            DestroyImmediate(gameObject);
    }

    private void Start()
    {
        LoadGame();
        TrackerAsset.Instance.setVar("language", LocalizationSettings.SelectedLocale.Identifier.Code);
        TrackerAsset.Instance.setVar("resolution", Screen.currentResolution.ToString());
        TrackerAsset.Instance.setVar("fullscreen", Screen.fullScreen);

        TrackerAsset.Instance.Completable.Initialized("articoding", CompletableTracker.Completable.Game);
        TrackerAsset.Instance.Completable.Progressed("articoding", CompletableTracker.Completable.Game, ProgressManager.Instance.GetGameProgress());
    }

    public void LoadGame()
    {
        if (loadSave && !gameLoaded)
        {
            SaveManager.Load();
            gameLoaded = true;
        }
    }

    public bool IsGameLoaded()
    {
        return gameLoaded;
    }

    public Category GetCurrentCategory()
    {
        return category;
    }

    public bool InCreatedLevel()
    {
        return category == categories[categories.Count - 1];
    }

    public int GetCurrentLevelIndex()
    {
        return levelIndex;
    }

    public void SetCurrentLevel(int levelIndex)
    {
        this.levelIndex = levelIndex;
    }

    public void SetCurrentCategory(Category category)
    {
        this.category = category;
    }

    // Esto habra que moverlo al MenuManager o algo asi
    public void LoadLevel(Category category, int levelIndex)
    {
        playingCommunityLevel = false;
        blockIDs = new Dictionary<UBlockly.Block, string>();
        this.category = category;
        this.levelIndex = levelIndex;

        ProgressManager.Instance.LevelStarted(category, levelIndex);
        if (loadSave)
            SaveManager.Save();

        if (LoadManager.Instance == null)
        {
            SceneManager.LoadScene("LevelScene");
            return;
        }

        LoadManager.Instance.LoadScene("LevelScene");
    }

    //Seteamos algunos flags para que sepa el juego que estamos jugando 
    //desde la pestaña de comunidad
    public void LoadCommunityLevel()
    {
        playingCommunityLevel = true;

        blockIDs = new Dictionary<UBlockly.Block, string>();
        this.category = null;
        this.levelIndex = -1;
    }

    public void LoadLevelCreator()
    {
        blockIDs = new Dictionary<UBlockly.Block, string>();
        if (LoadManager.Instance == null)
        {
            SceneManager.LoadScene("BoardCreation");
            return;
        }
        LoadManager.Instance.LoadScene("BoardCreation");
    }

    public void LoadScene(string name)
    {
        if (LoadManager.Instance == null)
        {
            SceneManager.LoadScene(name);
            return;
        }
        LoadManager.Instance.LoadScene(name);
    }

    public void OnDestroy()
    {
        if (Instance == this && loadSave)
            SaveManager.Save();
    }

    public void OnApplicationQuit()
    {
        if (loadSave)
            SaveManager.Save();
    }

    public string GetCurrentLevelName()
    {
        if (this.levelIndex == -1)
            return "editor_level";

        Category category = GetCurrentCategory();
        int levelIndex = GetCurrentLevelIndex();

        string levelName;
        levelName = category.levels[levelIndex].levelName;

        return levelName;
    }

    public List<Category> GetCategories()
    {
        return categories;
    }

    public string GetBlockId(UBlockly.Block block)
    {
        while (!blockIDs.ContainsKey(block))
        {
            var blockId = block.Type + "_" + SimvaPlugin.SimvaApi<IStudentsApi>.GenerateRandomBase58Key(4);
            if (!blockIDs.ContainsValue(blockId))
                blockIDs.Add(block, blockId);
        }
        return blockIDs[block];
    }
    
    public string ChangeCodeIDs(string code)
    {
        foreach(var kv in blockIDs)       
            code = code.Replace(kv.Key.ID, kv.Value);
        return code;
    }

    public void SetToken(string newtoken) { token = "Bearer " + newtoken; }
    public string GetToken() { return token; }
    public void SetLogged(bool isLogged) { loggedIn = isLogged; }
    public bool GetLogged() { return loggedIn; }
    public void SetRole(string role) { if (role == "ROLE_USER") isAdmin = false; else isAdmin = true; }
    public void SetAdmin(bool aux) { isAdmin = aux; }
    public bool GetIsAdmin() { return isAdmin; }
    public void SetUserName(string name) { userName = name; }
    public string GetUserName() { return userName; }

    public bool GetPlayingCommunityLevel() { return playingCommunityLevel; }
    public void SetPlayingCommunityLevel(bool aux) { playingCommunityLevel = aux; }
    public void SetCommunityLevelBoard(BoardState state) { communityBoard = state; }
    public BoardState GetCommunityLevelBoard() { return communityBoard; }
    public void SetCommunityLevelActiveBlocks(ActiveBlocks blocks) { communityActiveBlocks = blocks; }
    public ActiveBlocks GetCommunityLevelActiveBlocks() { return communityActiveBlocks; }
    public void SetCommunityInitialState(string state) { communityInitialState = state; }
    public string GetCommunityInitialState() { return communityInitialState; }

    public void ResetCommunityElements()
    {
        communityInitialState = null;
        communityActiveBlocks = null;
        communityBoard = null;
    }
}
