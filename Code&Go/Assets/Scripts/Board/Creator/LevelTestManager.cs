using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using AssetPackage;
using UBlockly.UGUI;
using UnityEngine.SceneManagement;
using UnityEditor;

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

    [SerializeField] StreamRoom streamRoom;

    public GameObject endPanel;
    public GameObject transparentRect;
    public GameObject blackRect;
    public GameObject endPanelMinimized;
    public GameObject exitConfirmationPanel;

    public RestrictionsPanel restrictionsPanel;

    public GameObject gameOverPanel;
    public GameObject gameOverMinimized;
    GameObject deactivableObject;
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

    private string ipserver;

    private void Start()
    {

        Invoke("ChangeMode", 0.01f);
        ActivateLevelBlocks(activeBlocks, false);
#if UNITY_EDITOR
        loadBoardPanel.SetActive(true);
        saveBoardPanel.SetActive(true);
        saveButton.SetActive(true);
#endif
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
        Debug.Log("Entro aqui de jajis");
            completed = true;
            endPanel.SetActive(true);
            blackRect.SetActive(true);
            streamRoom.FinishLevel();
            ProgressManager.Instance.UserCreatedLevel(initialState.ToJson(), restrictionsPanel.GetActiveBlocks().ToJson());
            /**
            * TODO No las tengo todas conmigo de que la exportación aqui esté bien, pero creo que es el flujo de gruardado normal
            */
            Exportar(initialState, restrictionsPanel.GetActiveBlocks());

            string levelName = GameManager.Instance.GetCurrentLevelName();
            TrackerAsset.Instance.setVar("steps", board.GetCurrentSteps());
            TrackerAsset.Instance.Completable.Completed(levelName, CompletableTracker.Completable.Level, true, -1f);
            
        }
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

    public void SetIPServer(string ip)
    {
        ipserver = ip;
    }
    /**
    * TODO Aqui llamariamos al exportar, ahora sin nombre ni nada
    */
     public void Exportar(BoardState boardstate, ActiveBlocks activeblocks) {
       deactivableObject  = GameObject.Find("ActivatedObject");
       //De-activate it
       deactivableObject.SetActive(true);
       //Get it's component/script
       ActivatedScript script = deactivableObject.GetComponent<ActivatedScript>();
       //Start coroutine on the other script with this MonoBehaviour
       script.Export(boardstate, activeblocks);
    }
    public void ExportCustomLevel()
    {

        // Aqui exportamos
        //TODO ESTO EN TEORIA NO LO USA NADIE, CREO QUE NOS QUEDO ALGO POR SUBIR DEL ULTIMO DIA DE PAIR, LO PONGO EN ProgressManager PERO LO MISMO LUEGO HAY QUE TRAERLO, PERDONA SI ES ASÍ
    }

}
