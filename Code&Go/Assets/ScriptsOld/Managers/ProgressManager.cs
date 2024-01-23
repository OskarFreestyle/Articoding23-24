using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.Localization;
using AssetPackage;
using UnityEditor;

public class ProgressManager : MonoBehaviour {

    #region Properties
    private static ProgressManager instance;
    public static ProgressManager Instance {
        get { return instance; }
    }

    [SerializeField] private bool allUnlocked;
    public bool AllUnlocked {
        get { return allUnlocked; }
    }
    #endregion

    // Old
    [SerializeField] private TextAsset activeBlocks;
    [SerializeField] private CategoryDataSO levelsCreatedCategory;

    private CategorySaveData[] categoriesData;

    private CategorySaveData currentCategoryData = null;

    private int currentLevel = 0, lastCategoryUnlocked = 0;

    private LevelsCreatedSaveData levelsCreated;
    private List<string> levelsCreatedHash;

    public LocalizedString createdLevelString;

    #region Methods
    private void Awake() {
        if (!instance) {
            instance = this;
            DontDestroyOnLoad(this);
            Init();
        }
        else {
            Debug.LogWarning("More than 1 Progress Manager created");
            DestroyImmediate(this);
        }
    }

    private void Init() {

        CategoryDataSO[] categoriesDataSO = GameManager.Instance.GetCategories();
        categoriesData = new CategorySaveData[categoriesDataSO.Length];

        int i = 0;
        foreach (CategoryDataSO categoryDataSO in categoriesDataSO) {

            CategorySaveData data = new CategorySaveData();
            data.levelsData = new int[categoryDataSO.GetTotalLevels()];

            // Create every level without any star
            for (int j = 0; j < data.levelsData.Length; j++) {
                data.levelsData[j] = allUnlocked ? 0 : -1;
            }

            categoriesData[i] = data;
            i++;
        }

        // Unlock the first level
        categoriesData[1].levelsData[0] = 0;

        // Created Levels
        levelsCreated = new LevelsCreatedSaveData();
        levelsCreated.levelsCreated = new string[0];
        levelsCreatedHash = new List<string>();
        levelsCreatedCategory.levels.Clear();
    }

    /// <summary>
    /// Manage the data when a levels is completed
    /// </summary>
    public void LevelCompleted(int starsAchieved) {
        // Set the stars for the current level complete
        if (currentCategoryData.levelsData[currentLevel] == -1) currentCategoryData.levelsData[currentLevel] = 0;

        int newStarsAchieved = Mathf.Clamp(starsAchieved - currentCategoryData.levelsData[currentLevel], 0, 3);
        currentCategoryData.levelsData[currentLevel] = currentCategoryData.levelsData[currentLevel] + newStarsAchieved;


        // Unlock the next level
        int nextLevelIndex = currentLevel + 1;
        int categoryIndex = Array.IndexOf(categoriesData, currentCategoryData);
        int nextCatagoryIndex = categoryIndex + 1;

        // If is in the same category
        if (nextLevelIndex < categoriesData[categoryIndex].levelsData.Length) {
            // If is locked
            if (categoriesData[categoryIndex].levelsData[nextLevelIndex] == -1) categoriesData[categoryIndex].levelsData[nextLevelIndex] = 0;
        }
        // If is the next category
        else if (nextCatagoryIndex < categoriesData.Length) {
            // If is locked
            if (categoriesData[nextCatagoryIndex].levelsData[0] == -1) categoriesData[nextCatagoryIndex].levelsData[0] = 0;
        }

        // Tracks
        //var levelName = GameManager.Instance.GetCurrentLevelName();
        //TrackerAsset.Instance.Completable.Completed(levelName, CompletableTracker.Completable.Level, true, starsAchieved);
        //var categoryName = categories[categoryIndex].name_id;
        //if (currentCategoryData.GetLevelsCompleted() < currentCategoryData.levelsData.Length) {
        //    TrackerAsset.Instance.Completable.Progressed(categoryName, CompletableTracker.Completable.Completable, (float)currentCategoryData.GetLevelsCompleted() / categories[categoryIndex].levels.Count);
        //}
        //else {
        //    TrackerAsset.Instance.Completable.Completed(categoryName, CompletableTracker.Completable.Completable, true, GetCategoryTotalStars(categoryIndex));
        //}
        //if (GetGameProgress() == 1.0f) {
        //    //TrackerAsset.Instance.Completable.Completed("articoding", CompletableTracker.Completable.Game, true, GetTotalStars());
        //}
    }
    #endregion


    //Setters


    public void LevelStarted(int categoryIndex, int level)
    {
        if (currentCategoryData == null || (categoryIndex >= 0 && categoryIndex < categoriesData.Length && categoriesData[categoryIndex] != currentCategoryData))
            currentCategoryData = categoriesData[categoryIndex];
        currentLevel = level;


        //if (currentLevel == 0 && !currentCategoryData.completableInitialized)
        //{
        //    var categoryName = categories[categoryIndex].name_id;
        //    TrackerAsset.Instance.Completable.Initialized(categoryName, CompletableTracker.Completable.Completable);
        //    currentCategoryData.completableInitialized = true;
        //}

        var levelName = GameManager.Instance.GetCurrentLevelName();
        TrackerAsset.Instance.Accessible.Accessed(levelName);
    }

    public void LevelStarted(CategoryDataSO category, int level)
    {
        int index = category.index;
        if (index < 0)
            TrackerAsset.Instance.Accessible.Accessed(levelsCreatedCategory.levels[level].levelName);
        else {
            LevelStarted(category.index, level);
        }
    }

    //Getters
    //----------

    public CategorySaveData GetCategoryData(int index) {
        return categoriesData[index];
    }

    public bool IsLevelUnlocked(int categoryIndex, int level)
    {
        //if (categoryIndex == 7) return true;
        //else return allUnlocked || (categoryIndex < categoriesData.Length && level < categoriesData[categoryIndex].levelsData.Length &&
        //    level <= categoriesData[categoryIndex].lastLevelUnlocked);

        // Quitar
        return true;
    }

    public bool IsCategoryUnlocked(int categoryIndex)
    {
        if (categoryIndex == 7) return true;
        else return allUnlocked || (categoryIndex <= lastCategoryUnlocked);
    }

    public int GetLvlsCompleted() {
        int levels = 0;

        //foreach (CategoryDataSO c in categories)
        //{
        //    int tmp = GetCategoryCurrentProgress(c);
        //    levels += Mathf.Max(0, tmp);
        //}

        return levels;
    }

    public int GetLvlsPerfects() {
        int levels = 0;

        foreach(CategorySaveData categoryData in categoriesData)
        {
            for (int i = 0; i < categoryData.levelsData.Length; i++)
            {
                //if (categoryData.lastLevelUnlocked <= i)
                //    break;

                if (categoryData.levelsData[i] == 3)
                    levels++;
            }
        }

        return levels;
    }

    public int GetLevelStars(int categoryIndex, int level)
    {
        if (categoryIndex >= categoriesData.Length || categoryIndex < 0 || level >= categoriesData[categoryIndex].levelsData.Length || level < 0) return 0;
        return categoriesData[categoryIndex].levelsData[level];
    }

    public int GetLevelStars(CategoryDataSO category, int level) {
        return GetLevelStars(category.index, level);
    }

    public int GetLastCategory() {
        return lastCategoryUnlocked;
    }

    public int GetCategoryTotalStars(int categoryIndex)
    {
        if (categoryIndex >= categoriesData.Length || categoryIndex < 0) return 0;
        return categoriesData[categoryIndex].GetCurrentNumStars();
    }

    //public int GetCategoryTotalStars(CategoryDataSO category)
    //{
    //    return GetCategoryTotalStars(categoryCards[IndexOf(category));
    //}

    //public int GetCategoryCurrentProgress(CategoryDataSO category)
    //{
    //    int index = categories.IndexOf(category);
    //    if (index >= categoriesData.Length || index < 0) return 0;

    //    return categoriesData[index].GetLevelsCompleted();
    //}

    //public int GetCategoryTotalProgress(CategoryDataSO category)
    //{
    //    int index = categories.IndexOf(category);
    //    if (index >= categoriesData.Length || index < 0) return 0;

    //    return categoriesData[index].levelsData.Length;
    //}

    public void UserCreatedLevel(string board, string customActiveBlocks, string customInitialState, string levelName, int levelCategory)
    {
        //si el nivel ya existe no se guarda
        if (levelsCreatedHash.Contains(Hash.ToHash(board, ""))) return;

        int index = levelsCreatedCategory.levels.Count + 1;
        string path = Application.dataPath;
        string directory = Path.Combine(path, "Resources/Levels/Boards/0_CreatedLevels/");

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
            string directoryInitial = Path.Combine(path, "Resources/Levels/InitialStates/0_CreatedLevels/");
            string filePathRestrinction = directoryInitial + levelName + ".txt";
            //Creamos el archivo contenedor del estado inicial
            FileStream fileRestriction = new FileStream(filePathRestrinction, FileMode.Create);
            fileRestriction.Close();
            StreamWriter writerRestriction = new StreamWriter(filePathRestrinction);
            writerRestriction.Write(customInitialState);
            writerRestriction.Close();
        }

        if (!customActiveBlocks.Equals("NaN")) { 

            string directoryRestrinction = Path.Combine(path, "Resources/Levels/ActiveBlocks/0_CreatedLevels/");
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
    public ProgressSaveData Save() {
        ProgressSaveData data = new ProgressSaveData();

        Debug.Log("Save data"); // TODO quitar

        data.categoriesInfo = categoriesData;
        data.levelsCreatedData = levelsCreated;

        data.DebugLogCategoriesData(); // Todo quitar
        
        return data;
    }


    public void Load(ProgressSaveData data) {
        categoriesData = data.categoriesInfo;
        levelsCreated = data.levelsCreatedData;

        LoadLevelsCreated();
        CheckLevelsData();

        //UpdateCategoryCards();

        data.DebugLogCategoriesData();
    }

    //private void UpdateCategoryCards() {
    //    for (int i = 0; i < categoryCards.Length; i++) {
    //        categoryCards[i].Configure(categoriesData[i]);
    //    }
    //}

    public float GetGameProgress()
    {
        int levels = 0;
        int totalLevel = 0;

        //foreach (CategoryDataSO c in categories)
        //{
        //    int tmp = GetCategoryCurrentProgress(c);
        //    levels += Mathf.Max(0, tmp);
        //    totalLevel += c.levels.Count;
        //}

        return levels / (float)totalLevel;
    }

    //public int GetTotalStars()
    //{
    //    int stars = 0;

    //    //for (int i = 0; i < categories.Count; i++)
    //    //{
    //    //    stars += (int)GetCategoryTotalStars(i);
    //    //}

    //    return stars;
    //}

    private void CheckLevelsData()
    {
        if (allUnlocked) return;

        foreach(CategorySaveData categoryData in categoriesData)
        {
            for (int i = 0; i < categoryData.levelsData.Length; i++)
            {
                //if (categoryData.lastLevelUnlocked <= i )
                //    categoryData.levelsData[i].stars = -1;
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
