using System;
using AssetPackage;
using System.Collections;
using System.Collections.Generic;
using UBlockly;
using UBlockly.UGUI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Components;
using Input = UnityEngine.Input;

public class LevelManager : MonoBehaviour
{
    [System.Serializable]
    public struct CategoryTutorialsData
    {
        public PopUpData data;
        public string categoryName;
    }

    private Category currentCategory;
    [SerializeField] private LevelData currentLevel;
    private int currentLevelIndex = 0;

    [SerializeField] private bool buildLimits = true;

    [Space]
    [SerializeField] private StatementManager statementManager;

    [SerializeField] private BoardManager boardManager;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private CameraFit cameraFit;
    [SerializeField] private CodeZoom codeZoom;

    [SerializeField] private Category defaultCategory;
    [SerializeField] private int defaultLevelIndex;

    [SerializeField] private Text levelName;
    [SerializeField] private LocalizeStringEvent levelNameLocalized;

    [SerializeField] private GameObject saveButton;

    [SerializeField] private CategoryTutorialsData[] categoriesTutorials;

    [SerializeField] private int pasosOffset = 0;

    public GameObject endPanel;
    public GameObject transparentRect;
    public GameObject blackRect;
    public GameObject exitConfirmationPanel;

    public GameObject endPanelMinimized;
    public GameObject debugPanel;

    public GameObject gameOverPanel;
    public GameObject gameOverMinimized;

    public StarsController starsController;

    private int minimosPasos = 0;
    private string specialBlock = "";
    public LocalizeStringEvent endTextLocalized;
    //[SerializeField] private LocalizeStringEvent endText;

    public StreamRoom streamRoom;

    private bool completed = false;

    private void Awake()
    {
        GameManager gameManager = GameManager.Instance;

        if (gameManager != null)
        {
            currentCategory = gameManager.GetCurrentCategory();
            currentLevelIndex = gameManager.GetCurrentLevelIndex();
        }
        else
        {
            currentCategory = defaultCategory;
            currentLevelIndex = defaultLevelIndex;
        }

        currentLevel = currentCategory.levels[currentLevelIndex];
        minimosPasos = currentLevel.minimosPasos;
        //endTextLocalized.text = currentLevel.endText;
        
        endPanel.SetActive(false);
        transparentRect.SetActive(false);
        blackRect.SetActive(false);

#if UNITY_EDITOR
        saveButton.SetActive(true);
#endif
    }

    private void Start()
    {
        Initialize();

        var dom = UBlockly.Xml.WorkspaceToDom(BlocklyUI.WorkspaceView.Workspace);
        string text = UBlockly.Xml.DomToText(dom);
        text = GameManager.Instance.ChangeCodeIDs(text);
        
        TrackerAsset.Instance.setVar("code", "\r\n" + text);
        TrackerAsset.Instance.Completable.Initialized(GameManager.Instance.GetCurrentLevelName().ToLower(), CompletableTracker.Completable.Level);

        //levelName.text = currentLevel.levelName;
        levelNameLocalized.StringReference = currentLevel.levelNameLocalized;
        levelNameLocalized.RefreshString();

        endTextLocalized.StringReference = currentLevel.endTextLocalized;
        endTextLocalized.RefreshString();
    }

    private void Update()
    {
        if (boardManager == null)
            return;

        if (boardManager.BoardCompleted() && !endPanel.activeSelf && !endPanelMinimized.activeSelf)
        {
            string levelName = GameManager.Instance.GetCurrentLevelName();
            streamRoom.FinishLevel();
            
            // Pone las estrellas que toca
            starsController.GiveMinimumStepsStar(boardManager.GetCurrentSteps() - (minimosPasos + pasosOffset));
            starsController.GiveHangingCodeStar(boardManager.GetNumOfTopBlocksUsed());
            starsController.GiveSpecialStar(specialBlock);
            
            endPanel.SetActive(true);
            blackRect.SetActive(true);
            if (!GameManager.Instance.InCreatedLevel())
            {
                TrackerAsset.Instance.setVar("steps", boardManager.GetCurrentSteps());
                TrackerAsset.Instance.setVar("first_execution", starsController.IsSpecialBlockStarActive());
                TrackerAsset.Instance.setVar("minimum_steps", starsController.IsMinimumStepsStarActive());
                TrackerAsset.Instance.setVar("no_hints", starsController.IsNoHangingCodeStarActive());
                ProgressManager.Instance.LevelCompleted(starsController.GetStars());

                var dom = UBlockly.Xml.WorkspaceToDom(BlocklyUI.WorkspaceView.Workspace);
                string text = UBlockly.Xml.DomToText(dom);
                text = GameManager.Instance.ChangeCodeIDs(text);
                TrackerAsset.Instance.setVar("code", "\r\n" + text); //codigo
            }
            else
                TrackerAsset.Instance.Completable.Completed(levelName, CompletableTracker.Completable.Level, true, -1);

            completed = true;
        }

#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.N))
            LoadNextLevel();
#endif
    }

    private void Initialize()
    {
        if (currentLevel == null)
        {
            Debug.LogError("Cannot initialize Level. CurrentLevel is null");
            return;
        }

        // Maybe do more stuff
        ActivateLevelBlocks(currentLevel.activeBlocks, currentLevel.allActive); 
        LoadInitialBlocks(currentLevel.initialState);//UI

        string boardJson = currentLevel.levelBoard != null ? currentLevel.levelBoard.text : currentLevel.auxLevelBoard;
        BoardState state = BoardState.FromJson(boardJson);
        boardManager.LoadBoard(state, buildLimits);
        cameraFit.FitBoard(boardManager.GetRows(), boardManager.GetColumns());

        BlocklyUI.WorkspaceView.InitIDs();
    }

    public void LoadLevel(Category category, int levelIndex)
    {
        currentCategory = category;
        currentLevelIndex = levelIndex;
        LoadLevel(category.levels[levelIndex]);
    }
    private void LoadLevel(LevelData level)
    {
        currentLevel = level;
        Initialize();
    }

    // It is called when the current level is completed
    public void LoadNextLevel()
    {
        int levelSize = currentCategory.levels.Count;

        // Si habia una estrella de antes, la quitamos
        SetSpecialBlockStarActive(false);
        
        if (++currentLevelIndex < levelSize)
            GameManager.Instance.LoadLevel(currentCategory, currentLevelIndex);
        else
            LoadMainMenu(); // Por ejemplo
    }

    public void RestartLevel()
    {
        codeZoom.ResetZoom();
        LoadInitialBlocks(currentLevel.initialState);
        ActivateLevelBlocks(currentLevel.activeBlocks, currentLevel.allActive);
    }

    public void RetryLevel()
    {
        ResetLevel();
        gameOverPanel.SetActive(false);
        transparentRect.SetActive(false);
        blackRect.SetActive(false);
        gameOverMinimized.SetActive(false);

        streamRoom.Retry();

        TrackerAsset.Instance.GameObject.Interacted("retry_button");

        var levelName = GameManager.Instance.GetCurrentLevelName();
        TrackerAsset.Instance.Completable.Initialized(levelName, CompletableTracker.Completable.Level);
    }

    public void MinimizeEndPanel()
    {
        endPanelMinimized.SetActive(true);
        gameOverPanel.SetActive(false);
        endPanel.SetActive(false);
        transparentRect.SetActive(false);
        blackRect.SetActive(false);
        debugPanel.SetActive(false);
        TrackerAsset.Instance.GameObject.Interacted("end_panel_minimized_button");
    }

    public void MinimizeGameOverPanel()
    {
        gameOverMinimized.SetActive(true);
        gameOverPanel.SetActive(false);
        //endPanel.SetActive(false);
        transparentRect.SetActive(false);
        blackRect.SetActive(false);
        debugPanel.SetActive(false);
        TrackerAsset.Instance.GameObject.Interacted("game_over_panel_minimized_button");
    }

    public void SetActiveNoInputPanel()
    {
        transparentRect.SetActive(true);
    }

    public void SetActiveExitConfirmationPanel(bool active)
    {
        exitConfirmationPanel.SetActive(active);
        transparentRect.SetActive(false);
        blackRect.SetActive(active);
    }

    public void ResetLevel()
    {
        boardManager.Reset();
        string boardJson = currentLevel.levelBoard != null ? currentLevel.levelBoard.text : currentLevel.auxLevelBoard;
        BoardState state = BoardState.FromJson(boardJson);
        boardManager.GenerateBoardElements(state);
        debugPanel.SetActive(true);
        cameraFit.FitBoard(boardManager.GetRows(), boardManager.GetColumns());
        starsController.ReactivateMinimumStepsStar();
    }

    public void ReloadLevel() //TODO: mirar si se usa en algun sitio (lo dudo)
    {
        LoadLevel(currentLevel);
    }

    public void LoadMainMenu()
    {
        string levelName = GameManager.Instance.GetCurrentLevelName().ToLower();
        TrackerAsset.Instance.setVar("steps", boardManager.GetCurrentSteps());
        TrackerAsset.Instance.setVar("level", levelName);
        TrackerAsset.Instance.GameObject.Interacted("level_exit_button");


        var dom = UBlockly.Xml.WorkspaceToDom(BlocklyUI.WorkspaceView.Workspace);
        string text = UBlockly.Xml.DomToText(dom);
        text = GameManager.Instance.ChangeCodeIDs(text);

        // Si habia una estrella de antes, la quitamos
        SetSpecialBlockStarActive(false);
        
        if (!completed)
        {
            TrackerAsset.Instance.setVar("code", "\r\n" + text);
            TrackerAsset.Instance.Completable.Completed(levelName, CompletableTracker.Completable.Level, false, -1f);
        }

        if(LoadManager.Instance == null)
        {
            SceneManager.LoadScene("MenuScene");
            return;
        }

        LoadManager.Instance.LoadScene("MenuScene");
    }

    public void LoadInitialBlocks(LocalizedAsset<TextAsset> textAsset)
    {
        if (textAsset == null) return;

        StartCoroutine(AsyncLoadInitialBlocks(textAsset));
    }

    IEnumerator AsyncLoadInitialBlocks(LocalizedAsset<TextAsset> textAsset)
    {
        var loadingOp = textAsset.LoadAssetAsync();
        if (!loadingOp.IsDone)
            yield return loadingOp;

        TextAsset localizedTextAsset = loadingOp.Result;
        if (localizedTextAsset == null) yield break;

        BlocklyUI.WorkspaceView.CleanViews();

        var dom = UBlockly.Xml.TextToDom(localizedTextAsset.text);

        UBlockly.Xml.DomToWorkspace(dom, BlocklyUI.WorkspaceView.Workspace);
        BlocklyUI.WorkspaceView.BuildViews();

        yield return null;
    }

    public void ActivateLevelBlocks(TextAsset textAsset, bool allActive)
    {
        if (textAsset == null) return;

        StartCoroutine(AsyncActivateLevelBlocks(textAsset, allActive));
    }

    IEnumerator AsyncActivateLevelBlocks(TextAsset textAsset, bool allActive)
    {
        if (allActive) BlocklyUI.WorkspaceView.Toolbox.SetActiveAllBlocks();
        else if (textAsset != null)
        {
            ActiveBlocks blocks = ActiveBlocks.FromJson(textAsset.text);
            if(blocks.specialBlock == null)
                specialBlock = "None";
            else
            {
                // Ponemos la nueva estrella
                specialBlock = blocks.specialBlock;
                SetSpecialBlockStarActive(true);
            }
            
            BlocklyUI.WorkspaceView.Toolbox.SetActiveBlocks(blocks.AsMap(), CategoriesTutorialsAsMap());
        }

        yield return null;
    }

    public Dictionary<string, PopUpData> CategoriesTutorialsAsMap()
    {
        Dictionary<string, PopUpData> map = new Dictionary<string, PopUpData>();
        foreach (CategoryTutorialsData c in categoriesTutorials)//For each category       
            map[c.categoryName.ToUpper()] = c.data;
        return map;
    }

    private void SetSpecialBlockStarActive(bool active)
    {
        GameObject blockPrefab = BlockResMgr.Get().LoadBlockViewPrefab(specialBlock);

        if (blockPrefab != null)
        {
            Transform star = Array.Find(blockPrefab.GetComponentsInChildren<Transform>(),
                (transform => { return transform.gameObject.name == "Star"; }));
            if (star)
                star.gameObject.GetComponent<Image>().enabled = active;
        }
        
    }

}
