using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.Localization;
using AssetPackage;
using UnityEditor;

public class ProgressManager : MonoBehaviour
{
    public static ProgressManager Instance;

    [SerializeField] private List<CategoryDataSO> categories;
    [SerializeField] private bool allUnlocked;
    [SerializeField] private TextAsset activeBlocks;
    [SerializeField] private CategoryDataSO levelsCreatedCategory;

    private CategorySaveData[] categoriesData;
    private int hintsRemaining = 5;
    private int coins = 10;
    private int levelsCompleted = 0;
    private string name = "";

    private CategorySaveData currentCategoryData = null;
    private int currentLevel = 0, lastCategoryUnlocked = 0;

    private LevelsCreatedSaveData levelsCreated;
    private List<string> levelsCreatedHash;

    public LocalizedString createdLevelString;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Init()
    {
        categoriesData = new CategorySaveData[categories.Count];
        for (int i = 0; i < categoriesData.Length; i++)
        {
            CategorySaveData data = new CategorySaveData();
            data.lastLevelUnlocked = i <= lastCategoryUnlocked ? 0 : -1;
            data.totalStars = 0;
            //Si es la categoría de niveles creados, reseteamos el contador siempre
            //ya que los niveles no se guardan en el scriptable, se guardan en archivos
            if(i == 7)
                data.levelsData = new LevelSaveData[0];
            else
                data.levelsData = new LevelSaveData[categories[i].levels.Count];

            for (int j = 0; j < data.levelsData.Length; j++)
            {
                data.levelsData[j] = new LevelSaveData();
                data.levelsData[j].stars = -1;
            }
            categoriesData[i] = data;
        }

        currentCategoryData = categoriesData[0];
        currentLevel = 0;
        levelsCreated = new LevelsCreatedSaveData();
        levelsCreated.levelsCreated = new string[0];
        levelsCreatedHash = new List<string>();
        levelsCreatedCategory.levels.Clear();
    }

    public bool IsAllUnlockedModeOn()
    {
        return allUnlocked;
    }


    //Setters
    //---------
    public void LevelCompleted(int starsAchieved)
    {
        if (currentCategoryData.levelsData[currentLevel].stars == -1)
            currentCategoryData.levelsData[currentLevel].stars = 0;

        int newStarsAchieved = Mathf.Clamp(starsAchieved - currentCategoryData.levelsData[currentLevel].stars, 0, 3);
        currentCategoryData.levelsData[currentLevel].stars = currentCategoryData.levelsData[currentLevel].stars + newStarsAchieved;
        currentCategoryData.totalStars += (uint)newStarsAchieved;

        int categoryIndex = Array.IndexOf(categoriesData, currentCategoryData);

        if (currentLevel >= currentCategoryData.lastLevelUnlocked)
        {
            if (currentLevel + 1 >= currentCategoryData.levelsData.Length && lastCategoryUnlocked == categoryIndex && lastCategoryUnlocked + 1 < categories.Count)
            {
                lastCategoryUnlocked++;
                categoriesData[lastCategoryUnlocked].lastLevelUnlocked = 0;
            }
            currentCategoryData.lastLevelUnlocked = currentLevel + 1;
        }

        var levelName = GameManager.Instance.GetCurrentLevelName();
        TrackerAsset.Instance.Completable.Completed(levelName, CompletableTracker.Completable.Level, true, starsAchieved);

        var categoryName = categories[categoryIndex].name_id;
        if (currentCategoryData.GetLevelsCompleted() < currentCategoryData.levelsData.Length)
        {
            TrackerAsset.Instance.Completable.Progressed(categoryName, CompletableTracker.Completable.Completable, (float)currentCategoryData.GetLevelsCompleted() / categories[categoryIndex].levels.Count);
        }
        else
        {
            TrackerAsset.Instance.Completable.Completed(categoryName, CompletableTracker.Completable.Completable, true, GetCategoryTotalStars(categoryIndex));
        }

        if (GetGameProgress() == 1.0f)
        {
            TrackerAsset.Instance.Completable.Completed("articoding", CompletableTracker.Completable.Game, true, GetTotalStars());
        }
    }

    public void LevelStarted(int categoryIndex, int level)
    {
        if (currentCategoryData == null || (categoryIndex >= 0 && categoryIndex < categoriesData.Length && categoriesData[categoryIndex] != currentCategoryData))
            currentCategoryData = categoriesData[categoryIndex];
        currentLevel = level;


        if (currentLevel == 0 && !currentCategoryData.completableInitialized)
        {
            var categoryName = categories[categoryIndex].name_id;
            TrackerAsset.Instance.Completable.Initialized(categoryName, CompletableTracker.Completable.Completable);
            currentCategoryData.completableInitialized = true;
        }

        var levelName = GameManager.Instance.GetCurrentLevelName();
        TrackerAsset.Instance.Accessible.Accessed(levelName);
    }

    public void LevelStarted(CategoryDataSO category, int level)
    {
        int index = categories.IndexOf(category);
        if (index < 0)
            TrackerAsset.Instance.Accessible.Accessed(levelsCreatedCategory.levels[level].levelName);
        else
        {
            LevelStarted(categories.IndexOf(category), level);
        }
    }

    public void AddHints(int amount)
    {
        hintsRemaining += amount;
        Mathf.Clamp(hintsRemaining, 0, 100);
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        Mathf.Clamp(coins, 0, 100);
    }

    public void SetName(string n) {
        Debug.Log("Guarda nombre " + n);
        name = n;
    }

    //Getters
    //----------

    public bool IsLevelUnlocked(int categoryIndex, int level)
    {
        if (categoryIndex == 7) return true;
        else return allUnlocked || (categoryIndex < categoriesData.Length && level < categoriesData[categoryIndex].levelsData.Length &&
            level <= categoriesData[categoryIndex].lastLevelUnlocked);
    }

    public bool IsCategoryUnlocked(int categoryIndex)
    {
        if (categoryIndex == 7) return true;
        else return allUnlocked || (categoryIndex <= lastCategoryUnlocked);
    }

    public int GetLvlsCompleted() {
        int levels = 0;

        foreach (CategoryDataSO c in categories)
        {
            int tmp = GetCategoryCurrentProgress(c);
            levels += Mathf.Max(0, tmp);
        }

        return levels;
    }

    public int GetLvlsPerfects() {
        int levels = 0;

        foreach(CategorySaveData categoryData in categoriesData)
        {
            for (int i = 0; i < categoryData.levelsData.Length; i++)
            {
                if (categoryData.lastLevelUnlocked <= i)
                    break;

                if (categoryData.levelsData[i].stars == 3)
                    levels++;
            }
        }

        return levels;
    }

    public int GetLevelStars(int categoryIndex, int level)
    {
        if (categoryIndex >= categoriesData.Length || categoryIndex < 0 || level >= categoriesData[categoryIndex].levelsData.Length || level < 0) return 0;
        return categoriesData[categoryIndex].levelsData[level].stars;
    }

    public int GetLevelStars(CategoryDataSO category, int level)
    {
        return GetLevelStars(categories.IndexOf(category), level);
    }

    public int GetLastCategory() {
        return lastCategoryUnlocked;
    }

    public uint GetCategoryTotalStars(int categoryIndex)
    {
        if (categoryIndex >= categoriesData.Length || categoryIndex < 0) return 0;
        return categoriesData[categoryIndex].totalStars;
    }

    public uint GetCategoryTotalStars(CategoryDataSO category)
    {
        return GetCategoryTotalStars(categories.IndexOf(category));
    }

    public int GetCategoryCurrentProgress(CategoryDataSO category)
    {
        int index = categories.IndexOf(category);
        if (index >= categoriesData.Length || index < 0) return 0;

        return categoriesData[index].GetLevelsCompleted();
    }

    public int GetCategoryTotalProgress(CategoryDataSO category)
    {
        int index = categories.IndexOf(category);
        if (index >= categoriesData.Length || index < 0) return 0;

        return categoriesData[index].levelsData.Length;
    }

    public int GetHintsRemaining()
    {
        return hintsRemaining;
    }

    public int GetCoins()
    {
        return coins;
    }

    public string GetName()
    {
        return name;
    }

    public void UserCreatedLevel(string board, string customActiveBlocks, string customInitialState, string levelName, int levelCategory)
    {
        //si el nivel ya existe no se guarda
        if (levelsCreatedHash.Contains(Hash.ToHash(board, ""))) return;

        int index = levelsCreatedCategory.levels.Count + 1;
        string path = Application.dataPath;
        string directory = Path.Combine(path, "Resources/Levels/Boards/8_CreatedLevels/");

        CreateDirectories();

        if (levelName.Trim() == "") levelName = "NivelCreado";

        string filePath = directory + levelName + ".json";
        FileStream file = new FileStream(filePath, FileMode.Create);
        file.Close();
        StreamWriter writer = new StreamWriter(filePath);
        writer.Write(board);
        writer.Close();

        /**/

        if (!customInitialState.Equals("NaN"))
        {
            string directoryInitial = Path.Combine(path, "Resources/Levels/InitialStates/8_CreatedLevels/");
            string filePathRestrinction = directoryInitial + levelName + ".txt";
            //Creamos el archivo contenedor del estado inicial
            FileStream fileRestriction = new FileStream(filePathRestrinction, FileMode.Create);
            fileRestriction.Close();
            StreamWriter writerRestriction = new StreamWriter(filePathRestrinction);
            writerRestriction.Write(customInitialState);
            writerRestriction.Close();
        }

        if (!customActiveBlocks.Equals("NaN")) { 

            string directoryRestrinction = Path.Combine(path, "Resources/Levels/ActiveBlocks/8_CreatedLevels/");
            string filePathRestrinction = directoryRestrinction + levelName + ".json";
            //Creamos el archivo contenedor de las restricciones
            FileStream fileRestriction = new FileStream(filePathRestrinction, FileMode.Create);
            fileRestriction.Close();
            StreamWriter writerRestriction = new StreamWriter(filePathRestrinction);
            writerRestriction.Write(customActiveBlocks);
            writerRestriction.Close();

            //Y lo cargamos como TextAsset para poder añadirlo al Scriptable del nivel creado
            TextAsset customActiveBlocksAssets = (TextAsset)Resources.Load(levelName);

            AddLevelCreated(board, index, customActiveBlocksAssets, levelName, levelCategory);

        } else {
            AddLevelCreated(board, index, activeBlocks, levelName, levelCategory);
        }
               
        /**/
        Array.Resize(ref levelsCreated.levelsCreated, levelsCreated.levelsCreated.Length + 1);
        levelsCreated.levelsCreated[levelsCreated.levelsCreated.GetUpperBound(0)] = levelName;
    }

    private void LoadLevelsCreated()
    {
        string path =
#if UNITY_EDITOR
                   Application.dataPath;
#else
                   Application.persistentDataPath;
#endif
        for (int i = 0; i < levelsCreated.levelsCreated.Length; i++)
        {
            string levelName = levelsCreated.levelsCreated[i];
            string filePath = Path.Combine(path, "Boards/LevelsCreated/" + levelName + ".userLevel");

            string directoryRestrinction = Path.Combine(path, "Assets/Levels/LevelsCreated/");
            string filePathRestrinction = directoryRestrinction + levelName + ".json";
            TextAsset activeBlocks = Resources.Load(filePathRestrinction) as TextAsset;
            try
            {
                StreamReader reader = new StreamReader(filePath);
                string readerData = reader.ReadToEnd();
                reader.Close();
                AddLevelCreated(readerData, i + 1, activeBlocks, levelName, 7);
            }
            catch
            {
                Debug.Log("El archivo " + filePath + " no existe");
            }
        }
    }

    private void AddLevelCreated(string board, int index, TextAsset customActiveBlocks, string levelName, int levelCategory)
    {
        levelsCreatedHash.Add(board);
    }

    //Save and Load
    //----------------
    public ProgressSaveData Save()
    {
        ProgressSaveData data = new ProgressSaveData();
        data.categoriesInfo = categoriesData;
        data.hintsRemaining = hintsRemaining;
        data.lastCategoryUnlocked = lastCategoryUnlocked;
        data.coins = coins;
        data.name = name;
        data.levelsCreatedData = levelsCreated;
        return data;
    }

    public void Load(ProgressSaveData data)
    {
        categoriesData = data.categoriesInfo;
        hintsRemaining = data.hintsRemaining;
        lastCategoryUnlocked = data.lastCategoryUnlocked;
        coins = data.coins;
        name = data.name;
        levelsCreated = data.levelsCreatedData;
        LoadLevelsCreated();

        CheckLevelsData();
    }

    public float GetGameProgress()
    {
        int levels = 0;
        int totalLevel = 0;

        foreach (CategoryDataSO c in categories)
        {
            int tmp = GetCategoryCurrentProgress(c);
            levels += Mathf.Max(0, tmp);
            totalLevel += c.levels.Count;
        }

        return levels / (float)totalLevel;
    }

    public int GetTotalStars()
    {
        int stars = 0;

        for (int i = 0; i < categories.Count; i++)
        {
            stars += (int)GetCategoryTotalStars(i);
        }

        return stars;
    }

    private void CheckLevelsData()
    {
        if (allUnlocked) return;

        foreach(CategorySaveData categoryData in categoriesData)
        {
            for (int i = 0; i < categoryData.levelsData.Length; i++)
            {
                if (categoryData.lastLevelUnlocked <= i )
                    categoryData.levelsData[i].stars = -1;
            }
        }
    }

    private void CreateDirectories()
    {
        string path = Application.dataPath;

        //Creamos las carpetas pertinentes si no estan creadas
        if (!Directory.Exists(path + "/Resources/Levels/")) ;
        Directory.CreateDirectory(path + "/Resources/Levels/");
        if (!Directory.Exists(path + "/Resources/Levels/Boards/"))
            Directory.CreateDirectory(path + "/Resources/Levels/Boards/");
        if (!Directory.Exists(path + "/Resources/Levels/Boards/8_CreatedLevels/"))
            Directory.CreateDirectory(path + "/Resources/Levels/Boards/8_CreatedLevels/");
        if (!Directory.Exists(path + "/Resources/Levels/ActiveBlocks/"))
            Directory.CreateDirectory(path + "/Resources/Levels/ActiveBlocks/");
        if (!Directory.Exists(path + "/Resources/Levels/ActiveBlocks/8_CreatedLevels/"))
            Directory.CreateDirectory(path + "/Resources/Levels/ActiveBlocks/8_CreatedLevels/");
        if (!Directory.Exists(path + "/Resources/Levels/InitialStates/"))
            Directory.CreateDirectory(path + "/Resources/Levels/InitialStates/");
        if (!Directory.Exists(path + "/Resources/Levels/InitialStates/8_CreatedLevels/"))
            Directory.CreateDirectory(path + "/Resources/Levels/InitialStates/8_CreatedLevels/");
    }
}
