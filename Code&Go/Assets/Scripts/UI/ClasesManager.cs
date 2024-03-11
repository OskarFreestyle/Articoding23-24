using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ClasesManager : MonoBehaviour
{
    public List<GameObject> clases;
    public List<GameObject> lists;
    public GameObject levelListsGameObject;

    private List<ServerClasses.Level> publicLevels;
    public GameObject publicLevelList;

    public InputField searchBar;

    public GameObject levelPrefab;
    public GameObject publicLevelPrefab;
    public GameObject levelListPrefab;

    public ActivatedScript activatedScript;

    public int tabIndex;

    private int currentClass;
    private int currentLevel;

    public ScrollRect nivelesScroll;

    public ComunidadLayout comunidadLayout;

    public Text className;

    private string[] fileNames;

    private string lastCall;
    private int actualPage = 0;
    private int totalPages = 0;

    private void Start()
    {
        searchBar.onEndEdit.AddListener(delegate
        {
            CreateLevelsWithText(searchBar.text);
        });
    }

    public void ConfigureTabs()
    {
        ReadCreatedLevels();

        for (int i = 0; i < clases.Count; i++)
        {
            int index = i;

            clases[i].GetComponent<Button>().onClick.AddListener(() =>
            {
                SelectClassCallback(index);
            });

            var newLevelsList = Instantiate(levelListPrefab, levelListsGameObject.transform);
            lists.Add(newLevelsList);
            newLevelsList.name = "LevelsList" + i.ToString();
            newLevelsList.SetActive(false);
        }

        currentClass = 0;
    }

    public void DeleteLists()
    {
        //Eliminamos todas las clases y las volvemos a llamar
        for (var i = clases.Count - 1; i >= 0; i--)
        {
            Destroy(clases[i]);
        }

        clases.Clear();

        for (var i = lists.Count-1; i >= 0; i--)
        {
            Destroy(lists[i]);
        }

        lists.Clear();
    }

    private void SelectClassCallback(int index)
    {
        if (index >= clases.Count || index < 0) return;

        // Desactivamos el tab actual
        if (tabIndex != index) lists[tabIndex].SetActive(false);

        // Activamos el nuevo tab seleccionado
        tabIndex = index;
        lists[tabIndex].SetActive(true);
        nivelesScroll.content = (RectTransform)lists[tabIndex].transform;

        ClasePrefab claseprefab = clases[index].GetComponent<ClasePrefab>();

        className.text = claseprefab.GetClaseName();
        string getString = "levels?class=" + claseprefab.GetClaseIndex().ToString();

        currentClass = index;
        activatedScript.Get(getString, GetClassLevelsOK, GetClassLevelsKO);
    }

    int GetClassLevelsOK(UnityWebRequest req)
    {
        string levelsJson = req.downloadHandler.text;

        string idClassString = req.uri.Query;
        int idClass = int.Parse(idClassString[idClassString.Length-1].ToString());

        ServerClasses.LevelPage levelPage = new ServerClasses.LevelPage();

        try
        {
            levelPage = JsonUtility.FromJson<ServerClasses.LevelPage>(levelsJson);
        }
        catch (System.Exception e)
        {
            Debug.Log("Error al leer clases " + e);
        }

        clases[currentClass].GetComponent<ClasePrefab>().SetLevelPage(levelPage);

        if (levelPage.content == null || levelPage.content.Count == 0)
        {
            Debug.Log("No hay niveles en la clase " + idClass);
        }
        else
        {
            if (!clases[currentClass].GetComponent<ClasePrefab>().GetLevelsCreated())
            {
                for (int i = 0; i < levelPage.content.Count; i++)
                {
                    var newlevel = Instantiate(levelPrefab, lists[currentClass].transform);
                    newlevel.GetComponent<LevelPrefabOld>().SetLevel(levelPage.content[i].level, this, comunidadLayout);
                    newlevel.GetComponent<LevelPrefabOld>().SetLevelListId(i);
                    if (IsLevelAlreadySaved(levelPage.content[i].level.title)) newlevel.GetComponent<LevelPrefabOld>().DeactivateSave();
                }
                clases[currentClass].GetComponent<ClasePrefab>().SetLevelsCreated();
            }
        }


        return 0;
    }

    private bool IsLevelAlreadySaved(string name)
    {
        bool aux = false;
        for(int i = 0; i < fileNames.Length; i++)
        {
            if (fileNames[i] == name) return true;
        }
        return aux;
    }

    int GetClassLevelsKO(UnityWebRequest req)
    {
        //error

        return 0;
    }

    public void CreatePublicLevels(ServerClasses.LevelPage levels) 
    {
        if (publicLevels != null)
        {
            for (var i = publicLevels.Count - 1; i >= 0; i--)
            {
                Destroy(publicLevelList.transform.GetChild(i).gameObject);
            }

            publicLevels.Clear();
        }

        totalPages = levels.totalPages;
        publicLevels = new List<ServerClasses.Level>();

        for(int i = 0; i < levels.content.Count; i++)
        {
            var newlevel = Instantiate(publicLevelPrefab, publicLevelList.transform);
            newlevel.GetComponent<PublicLevelPrefab>().SetLevel(levels.content[i].level, this, comunidadLayout);
            if (IsLevelAlreadySaved(levels.content[i].level.title)) newlevel.GetComponent<LevelPrefabOld>().DeactivateSave();
            publicLevels.Add(levels.content[i].level);
        }
    }

    public void CreateLevelsWithText(string name)
    {
        for (var i = publicLevels.Count - 1; i >= 0; i--)
        {
            Destroy(publicLevelList.transform.GetChild(i).gameObject);
        }

        publicLevels.Clear();

        string getCall = "levels?publicLevels=true&size=6&page=" + actualPage + "&title=" + name;
        lastCall = getCall;
        activatedScript.Get(getCall, GetCreateLevelsNameOK, GetCreateLevelsNameKO);
    }

    int GetCreateLevelsNameOK(UnityWebRequest req)
    {
        string nivelesJson = req.downloadHandler.text;

        ServerClasses.LevelPage levels = comunidadLayout.GetPublicLevels();

        try
        {
            levels = JsonUtility.FromJson<ServerClasses.LevelPage>(nivelesJson);
        }
        catch (System.Exception e)
        {
            Debug.Log("Error al leer niveles " + e);
        }

        comunidadLayout.SetPublicLevels(levels);

        CreatePublicLevels(levels);

        return 0;
    }

    public void NextPage()
    {
        if (actualPage < totalPages - 1) actualPage++;

        CreateLevelsWithText(searchBar.text);
    }

    public void PrevPage()
    {
        if (actualPage > 0) actualPage--;

        CreateLevelsWithText(searchBar.text);
    }

    int GetCreateLevelsNameKO(UnityWebRequest req)
    {
        Debug.Log("Error al crear niveles publicos con nombre");

        return 0;
    }

    public string[] ReadCreatedLevels()
    {
        string[] boardFilePaths = Directory.GetFiles(Application.dataPath + "/Resources/Levels/Boards/8_CreatedLevels", "*.json");
        fileNames = new string[boardFilePaths.Length];

        for (int i = 0; i < boardFilePaths.Length; i++)
        {
            fileNames[i] = Path.GetFileNameWithoutExtension(boardFilePaths[i]);
        }

        return fileNames;
    }

    public void SetCommunityLevel()
    {
        ServerClasses.Level theLevel = GetCommunityLevel();

        GameManager.Instance.SetCommunityLevelBoard(theLevel.articodingLevel.boardstate);
        GameManager.Instance.SetCommunityLevelActiveBlocks(theLevel.articodingLevel.activeblocks);
        GameManager.Instance.SetCommunityInitialState(theLevel.articodingLevel.initialState);
    }

    public void SetPublicLevel(ServerClasses.Level theLevel)
    {
        GameManager.Instance.SetCommunityLevelBoard(theLevel.articodingLevel.boardstate);
        GameManager.Instance.SetCommunityLevelActiveBlocks(theLevel.articodingLevel.activeblocks);
        GameManager.Instance.SetCommunityInitialState(theLevel.articodingLevel.initialState);
    }

    public ServerClasses.Level GetCommunityLevel()
    {
        ServerClasses.Level theLevel;

        ServerClasses.LevelPage thePage = clases[currentClass].GetComponent<ClasePrefab>().GetLevelPage();
        theLevel = thePage.content[currentLevel].level;

        return theLevel;
    }

    public List<GameObject> GetClasses()
    {
        return clases;
    }

    public void SetLevelIndex(int lindx)
    {
        currentLevel = lindx;
    }
}
