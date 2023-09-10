using System.IO;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using AssetPackage;
using UBlockly.UGUI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class LevelTestManager : MonoBehaviour
{
    [SerializeField] CameraFit cameraFit;
    [SerializeField] OrbitCamera cameraOrbit;
    [SerializeField] CameraZoom cameraZoom;

    [SerializeField] GameObject levelObjects;
    [SerializeField] GameObject levelCanvas;
    [SerializeField] GameObject levelButtons;
    [SerializeField] RectTransform levelViewport;

    [SerializeField] GameObject creatorObjects;
    [SerializeField] GameObject creatorCanvas;
    [SerializeField] RectTransform creatorViewPort;

    [SerializeField] BoardManager board;
    [SerializeField] BoardCreator boardCreator;
    [SerializeField] TextAsset activeBlocks;

    [SerializeField] GameObject debugPanel;
    [SerializeField] GameObject changeModeButton;
    [SerializeField] Sprite changeToEditModeSprite;
    [SerializeField] Sprite changeToPlayModeSprite;

    [SerializeField] private GameObject saveButton;
    [SerializeField] GameObject loadBoardPanel;
    [SerializeField] GameObject saveBoardPanel;
    [SerializeField] GameObject nameWarningPanel;
    [SerializeField] GameObject confirmSavePanel;
    [SerializeField] GameObject errorSavePanel;

    [SerializeField] StreamRoom streamRoom;

    public GameObject endPanel;
    public GameObject exportButton;
    public GameObject exportUserButton;
    public GameObject logButton;
    public GameObject transparentRect;
    public GameObject blackRect;
    public GameObject endPanelMinimized;
    public GameObject exitConfirmationPanel;

    public RestrictionsPanel restrictionsPanel;
    public string levelName = "LevelCreated";
    public int levelCategory = 0;

    public GameObject gameOverPanel;
    public GameObject gameOverMinimized;
    private bool inCreator = false;
    private BoardState initialState;
    private bool completed = false;

    private string boardString = "";

    [SerializeField] Button resetViewButton;

    public Button exitButtonLeft;
    public Button exitButtonRight;
    public Button modeButton;
    public Button runButton;
    public Button stopButton;

    public Dropdown clasesDropdown;

    public ActivatedScript activatedScript;
    ServerClasses.ClaseJSON clases;

    private void Start()
    {

        Invoke("ChangeMode", 0.01f);
        ActivateLevelBlocks(activeBlocks, false);
#if UNITY_EDITOR
        loadBoardPanel.SetActive(true);
        saveBoardPanel.SetActive(true);
        saveButton.SetActive(true);
#endif

        //Obtenemos la lista de clases del profesor creando el nivel
        if(GameManager.Instance.GetIsAdmin())
        {
            activatedScript.Get("classes", GetClassesOK, GetClassesKO);
        }

    }

    private void Update()
    {
        resetViewButton.interactable = !cameraOrbit.IsReset();

        if (inCreator)
        {
            bool enabled = board.GetNEmitters() >= board.GetNReceivers() && board.GetNReceivers() > 0 && !board.AllReceiving();
            changeModeButton.GetComponent<Button>().enabled = enabled;
            changeModeButton.GetComponent<Image>().color = enabled ? Color.white : Color.grey;
        }
        else if (board.BoardCompleted() && !completed)
        {
            completed = true;
            endPanel.SetActive(true);

            bool logged = GameManager.Instance.GetLogged();
            logButton.SetActive(!logged);

            if (logged) {
                if (GameManager.Instance.GetIsAdmin())
                {
                    exportButton.SetActive(logged);
                    exportUserButton.SetActive(!logged);
                    //Si es profesor, activamos el boton y añadimos las clases a la elección de clases para subir el nivel
                    List<ServerClasses.Clase> clasesList = clases.content;

                    clasesDropdown.ClearOptions();

                    for (int i = 0; i < clasesList.Count; i++)
                    {
                        Dropdown.OptionData data = new Dropdown.OptionData();
                        data.text = clasesList[i].name;
                        clasesDropdown.options.Add(data);
                    }
                }
                else
                {
                    exportButton.SetActive(!logged);
                    exportUserButton.SetActive(logged);
                }
            }
            else
            {
                exportButton.SetActive(false);
                exportUserButton.SetActive(false);
            }

            blackRect.SetActive(true);
            streamRoom.FinishLevel();

            string levelNameEditor = GameManager.Instance.GetCurrentLevelName();
            TrackerAsset.Instance.setVar("steps", board.GetCurrentSteps());
            TrackerAsset.Instance.Completable.Completed(levelNameEditor, CompletableTracker.Completable.Level, true, -1f);
            
        }
    }

    public void TryToSaveLocal()
    {
        if(levelName.Trim() != "")
            SaveLevelLocal();
        else
            nameWarningPanel.SetActive(true);
    }

    public void TryToExport()
    {
        if (levelName.Trim() != "")
            ExportLevel(true);
        else
            nameWarningPanel.SetActive(true);
    }

    public void TryToUserExport()
    {
        if (levelName.Trim() != "")
            ExportLevel(false);
        else
            nameWarningPanel.SetActive(true);
    }

    public void SaveLevelLocal()
    {
        confirmSavePanel.SetActive(true);
        ProgressManager.Instance.UserCreatedLevel(initialState.ToJson(), restrictionsPanel.GetActiveBlocks().ToJson(), levelName, 7);
    }

    //Convertirmos los datos que tenemos (nombre, nivel y clase a la que va) a un objeto
    //que postear al servidor
    public void ExportLevel(bool isTeacher)
    {
        ServerClasses.PostedLevel levelToPost = new ServerClasses.PostedLevel();

        levelToPost.title = levelName;
        levelToPost.classes = new List<int>();
        levelToPost.description = "Nivel creador por " + GameManager.Instance.GetUserName();
        if(isTeacher)
            levelToPost.classes.Add(clases.content[clasesDropdown.value].id);
        levelToPost.publicLevel = !isTeacher;
        levelToPost.articodingLevel = new ServerClasses.ArticodingLevel();
        ActiveBlocks thisActiveBlocks = ActiveBlocks.FromJson(activeBlocks.text);
        levelToPost.articodingLevel.activeblocks = thisActiveBlocks;
        levelToPost.articodingLevel.boardstate = initialState;

        activatedScript.Post("levels", JsonUtility.ToJson(levelToPost), GetPostLevelOK, GetPostLevelKO);
    }

    int GetPostLevelOK(UnityWebRequest req)
    {
        confirmSavePanel.SetActive(true);

        return 0;
    }

    int GetPostLevelKO(UnityWebRequest req)
    {
        errorSavePanel.SetActive(true);

        return 0;
    }

    public void ChangeMode()
    {
        ChangeMode(false);
    }

    public void ChangeMode(bool fromButton)
    {
        inCreator = !inCreator;

        levelObjects.SetActive(!inCreator);
        levelCanvas.SetActive(!inCreator);
        levelButtons.SetActive(!inCreator);

        creatorObjects.SetActive(inCreator);
        creatorCanvas.SetActive(inCreator);

        board.SetModifiable(inCreator);

        if (!inCreator)
        {
            ActivateLevelBlocks(restrictionsPanel.GetActiveBlocks());
            completed = false;
            initialState = board.GetBoardState();
            cameraFit.SetViewPort(levelViewport);

            changeModeButton.GetComponent<Image>().sprite = changeToEditModeSprite;
            board.SetFocusPointOffset(new Vector3((board.GetColumns() - 2) / 2.0f + 0.5f, 0.0f, (board.GetRows() - 2) / 2.0f + 0.5f));
            cameraFit.FitBoard(board.GetRows(), board.GetColumns());

            string boardState = board.GetBoardStateAsFormatedString();

            if (fromButton)
            {
                TrackerAsset.Instance.setVar("mode", "test");
                TrackerAsset.Instance.setVar("board", boardState != boardString ? boardState : "unchanged");
            }
            boardString = boardState;
        }
        else
        {
            cameraOrbit.ResetInmediate();
            cameraZoom.ResetInmediate();
            cameraFit.SetViewPort(creatorViewPort);
            changeModeButton.GetComponent<Image>().sprite = changeToPlayModeSprite;
            boardCreator.FitBoard();


            if (fromButton)
                TrackerAsset.Instance.setVar("mode", "edition");
        }

        if (fromButton)
            TrackerAsset.Instance.GameObject.Interacted("editor_mode_change_button");

        if (inCreator)
            TrackerAsset.Instance.Accessible.Accessed("editor");
        else
            TrackerAsset.Instance.Accessible.Accessed("tester");
    }

    public void LoadMainMenu()
    {
        var dom = UBlockly.Xml.WorkspaceToDom(BlocklyUI.WorkspaceView.Workspace);
        string text = UBlockly.Xml.DomToText(dom);
        text = GameManager.Instance.ChangeCodeIDs(text);

        if (!completed)
        {
            TrackerAsset.Instance.setVar("code", "\r\n" + text);
            var levelName = GameManager.Instance.GetCurrentLevelName();
            TrackerAsset.Instance.Completable.Completed(levelName, CompletableTracker.Completable.Level, false, -1f);
        }

        if (LoadManager.Instance == null)
        {
            SceneManager.LoadScene("MenuScene");
            return;
        }

        LoadManager.Instance.LoadScene("MenuScene");
    }

    public void ResetLevel()
    {
        board.Reset();
        board.GenerateBoardElements(initialState);
        debugPanel.SetActive(true);
        cameraFit.FitBoard(board.GetRows(), board.GetColumns());
        TrackerAsset.Instance.GameObject.Interacted("editor_retry_button");

        var levelName = GameManager.Instance.GetCurrentLevelName();
        TrackerAsset.Instance.Completable.Initialized(levelName, CompletableTracker.Completable.Level);
    }

    public void RetryLevel()
    {
        ResetLevel();
        gameOverPanel.SetActive(false);
        endPanel.SetActive(false);
        transparentRect.SetActive(false);
        blackRect.SetActive(false);
        gameOverMinimized.SetActive(false);
        EnableHeaderButtons();

        streamRoom.Retry();
        completed = false;
    }

    public void MinimizeEndPanel()
    {
        endPanelMinimized.SetActive(true);
        gameOverPanel.SetActive(false);
        endPanel.SetActive(false);
        transparentRect.SetActive(false);
        blackRect.SetActive(false);
        debugPanel.SetActive(false);
        TrackerAsset.Instance.GameObject.Used("end_panel_minimized");
    }

    public void MinimizeGameOverPanel()
    {
        gameOverMinimized.SetActive(true);
        gameOverPanel.SetActive(false);
        //endPanel.SetActive(false);
        transparentRect.SetActive(false);
        blackRect.SetActive(false);
        debugPanel.SetActive(false);
        TrackerAsset.Instance.GameObject.Used("game_over_panel_minimized");
    }

    public void SetActiveNoInputPanel()
    {
        transparentRect.SetActive(true);
        DisableHeaderButtons();
    }

    void DisableHeaderButtons()
    {
        exitButtonLeft.enabled = false;
        exitButtonRight.enabled = false;
        modeButton.enabled = false;
        runButton.enabled = false;
        stopButton.enabled = true;
        stopButton.gameObject.SetActive(true);
    }

    void EnableHeaderButtons()
    {
        exitButtonLeft.enabled = true;
        exitButtonRight.enabled = true;
        modeButton.enabled = true;
        runButton.enabled = true;
        stopButton.enabled = false;
        stopButton.gameObject.SetActive(false);
    }

    public void SetActiveExitConfirmationPanel(bool active)
    {
        exitConfirmationPanel.SetActive(active);
        transparentRect.SetActive(false);
        blackRect.SetActive(active);
    }

    public void ActivateLevelBlocks(TextAsset textAsset, bool allActive)
    {
        if (textAsset == null) return;

        StartCoroutine(AsyncActivateLevelBlocks(textAsset, allActive));
    }

    public void ActivateLevelBlocks(ActiveBlocks ablocks)
    {
        ActiveBlocks blocks = ablocks;
        BlocklyUI.WorkspaceView.Toolbox.SetActiveBlocks(blocks.AsMap());
    }

    IEnumerator AsyncActivateLevelBlocks(TextAsset textAsset, bool allActive)
    {
        if (allActive) BlocklyUI.WorkspaceView.Toolbox.SetActiveAllBlocks();
        else if (textAsset != null)
        {
            ActiveBlocks blocks = ActiveBlocks.FromJson(textAsset.text);
            BlocklyUI.WorkspaceView.Toolbox.SetActiveBlocks(blocks.AsMap());
        }

        yield return null;
    }

    public void ChangeLevelName(string newLevelName)
    {
        levelName = newLevelName;
    }  

    public void ChangeLevelCategory(int newLevelCategory)
    {
        levelCategory = newLevelCategory;
    }

    public void SaveActualBlocks()
    {
        var dom = UBlockly.Xml.WorkspaceToDom(BlocklyUI.WorkspaceView.Workspace);
        string text = UBlockly.Xml.DomToText(dom);

        XDocument xmldoc = XDocument.Parse(text);
        string josn = JsonConvert.SerializeXNode(xmldoc, Formatting.None, true);

        ServerClasses.InitialState actualBlocks = JsonUtility.FromJson<ServerClasses.InitialState>(josn);


    }

    int GetClassesOK(UnityWebRequest req)
    {
        string clasesJson = req.downloadHandler.text;

        try
        {
            clases = JsonUtility.FromJson<ServerClasses.ClaseJSON>(clasesJson);
        }
        catch (System.Exception e)
        {
            Debug.Log("Error al leer clases " + e);
        }

        return 0;
    }

    int GetClassesKO(UnityWebRequest req)
    {
        Debug.Log("Error al obtener clases");
        return 0;
    }

    public void ActivateExportButtons(bool isTeacher)
    {
        exportButton.SetActive(isTeacher);
        exportUserButton.SetActive(!isTeacher);
    }

}
