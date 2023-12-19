using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization;
using AssetPackage;

public class CategoryManagerOld : MonoBehaviour
{
    [SerializeField] private Category[] categories;

    [SerializeField] private GameObject categoriesParent;
    [SerializeField] private CategoryCardOld categoryCardPrefab;

    [SerializeField] private GameObject levelsParent;
    [SerializeField] private LevelCard levelCardPrefab;
    [SerializeField] private LevelCard createdLevelCardPrefab;

    [SerializeField] private Sprite createdLevelPreview;

    [SerializeField] private GameObject levelsCreatedParent;
    [SerializeField] private Button createLevelButton;
    [SerializeField] private Category levelsCreatedCategory;

    [SerializeField] private Text categoryName;
    [SerializeField] private Text categoryDescription;
    [SerializeField] private LocalizeStringEvent localizedCategoryName;
    [SerializeField] private LocalizeStringEvent localizedCategoryDescription;

    [SerializeField] private Text levelName;
    [SerializeField] private LocalizeStringEvent localizedLevelName;
    [SerializeField] private Image levelPreview;

    [SerializeField] private Text levelCreatedName;
    [SerializeField] private LocalizeStringEvent localizedLevelCreatedName;
    [SerializeField] private LocalizedString createLevelLocalizeString;

    [SerializeField] private GameObject categoriesPanel;
    [SerializeField] private GameObject levelsPanel;
    [SerializeField] private GameObject playSelectedLevelButton;

    [SerializeField] private GameObject currentCategoryPanel;
    [SerializeField] private GameObject currentLevelPanel;

    [SerializeField] private GameObject currentLevelCreatedPanel;

    [SerializeField] private Text currentCategoryLevelsText;
    [SerializeField] private LocalizeStringEvent currentCategoryLevelsTextLocalized;
    [SerializeField] private LocalizedString categoryNameLocaliced;
    [SerializeField] private LocalizedString[] selectedCategoryNameLocaliced;

    [SerializeField] private int currentCategory;
    [SerializeField] private int currentLevel;
    [SerializeField] private int levelCreatedIndex = -1000;

    [SerializeField] private Color deactivatedCategoryColor;
    [SerializeField] private Sprite deactivatedImage;

    [SerializeField] private LocalizedString nonCreatedLevelsString;
    [SerializeField] private Sprite nonCreatedLevelsSprite;

    private void Start() {
        if (!GameManager.Instance.IsGameLoaded()) GameManager.Instance.LoadGame();

        for (int i = 0; i < categories.Length; i++)
        {
            //int index = i;

            //CategoryCardOld card = Instantiate(categoryCardPrefab, categoriesParent.transform);
            //card.ConfigureCategory(categories[i]);
            //card.button.onClick.AddListener(() =>
            //{
            //    SelectCategory(index);
            //});

            //If it's not unlocked it can't be selected
            //if (!ProgressManager.Instance.IsCategoryUnlocked(index))
            //{
            //    card.button.enabled = false;
            //    card.image.sprite = deactivatedImage;
            //    card.button.image.color = deactivatedCategoryColor;
            //}
        }
        //currentLevelCreatedPanel.SetActive(false);

        HideLevels();

        SelectCategory(currentCategory);

        CreateUserLevelsCards();
    }

    private void CreateUserLevelsCards()
    {
        for (int i = 0; i < levelsCreatedCategory.levels.Count; i++)
        {
            int index = i;
            LevelData levelData = levelsCreatedCategory.levels[i];
            LevelCard levelCard = Instantiate(levelCardPrefab, levelsCreatedParent.transform);
            levelCard.ConfigureLevel(levelData, levelsCreatedCategory, i + 1);
            levelCard.DeactivateStars();
            levelCard.button.onClick.AddListener(() =>
            {
                //currentLevelCreatedPanel.SetActive(true);
                levelCreatedIndex = index;
                //levelCreatedName.text = levelData.levelName;

                localizedLevelCreatedName.StringReference = levelData.levelNameLocalized;
                localizedLevelCreatedName.StringReference.Arguments = new object[] { index + 1 };
                localizedLevelCreatedName.RefreshString();
            });
        }
        createLevelButton.gameObject.SetActive(true);
        createLevelButton.onClick.AddListener(() =>
        {
            //levelCreatedName.text = "Crear nivel";

            localizedLevelCreatedName.StringReference = createLevelLocalizeString;
            localizedLevelCreatedName.RefreshString();

            levelCreatedIndex = -1; // Reserved for creator mode
        });
        createLevelButton.transform.SetParent(levelsCreatedParent.transform);

        createLevelButton.onClick.Invoke();
    }

    private void SelectCategory(int index)
    {
        if (!ProgressManager.Instance.IsCategoryUnlocked(index)) return;

        if (index >= 0 && index < categories.Length)
        {
            currentCategory = index;

            localizedCategoryName.StringReference = categories[currentCategory].titleLocalized;
            localizedCategoryName.RefreshString();

            localizedCategoryDescription.StringReference = categories[currentCategory].descriptionLocalized;
            localizedCategoryDescription.RefreshString();

            //categoryName.text = categories[currentCategory].name_id;
            //categoryDescription.text = categories[currentCategory].description;
        }
    }

    public void ShowLevels()
    {
        if (!ProgressManager.Instance.IsCategoryUnlocked(currentCategory)) return;

        Category category = categories[currentCategory];
        //currentCategoryLevelsText.text = "Niveles - " + category.name_id;

        //selectedCategoryNameLocaliced.Arguments = new object[] { selectedLevelName };
        currentCategoryLevelsTextLocalized.StringReference = selectedCategoryNameLocaliced[currentCategory];
        currentCategoryLevelsTextLocalized.RefreshString();

        categoriesPanel.SetActive(false);
        levelsPanel.SetActive(true);

        currentCategoryPanel.SetActive(false);
        currentLevelPanel.SetActive(true);

        while (levelsParent.transform.childCount != 0)
            DestroyImmediate(levelsParent.transform.GetChild(0).gameObject);

        // Created Level Category
        if (category.name_id == "CreatedLevels") {

            localizedLevelName.StringReference = nonCreatedLevelsString;
            localizedLevelName.RefreshString();
            levelPreview.sprite = nonCreatedLevelsSprite;

            categories[currentCategory].levels.Clear();

            //Encontramos todos los archivos que haya en las carpetas de creación de niveles
            //y los almacenamos como TextAssets para su lectura
            string[] boardFilePaths = Directory.GetFiles(Application.dataPath + "/Resources/Levels/Boards/8_CreatedLevels", "*.json");
            string[] activeFilePaths = Directory.GetFiles(Application.dataPath + "/Resources/Levels/ActiveBlocks/8_CreatedLevels", "*.json");
            string[] initialFilePaths = Directory.GetFiles(Application.dataPath + "/Resources/Levels/InitialStates/8_CreatedLevels", "*.txt");
            TextAsset[] boards = new TextAsset[boardFilePaths.Length];
            TextAsset[] activeBlocks = new TextAsset[activeFilePaths.Length];
            TextAsset[] initialBlocks = new TextAsset[initialFilePaths.Length];
            string[] fileNames = new string[boardFilePaths.Length];

            
            playSelectedLevelButton.SetActive(boardFilePaths.Length > 0);


            for (int i = 0; i < boardFilePaths.Length; i++) {
                boards[i] = new TextAsset(File.ReadAllText(boardFilePaths[i]));
                activeBlocks[i] = new TextAsset(File.ReadAllText(activeFilePaths[i]));
                initialBlocks[i] = new TextAsset(File.ReadAllText(initialFilePaths[i]));
                fileNames[i] = Path.GetFileNameWithoutExtension(boardFilePaths[i]);
            }

            for (int i = 0; i < boards.Length; i++) {
                int index = i;
                LevelData levelData = new LevelData();
                levelData.levelName = fileNames[i];
                levelData.activeBlocks = activeBlocks[i];
                levelData.customInitialState = initialBlocks[i];
                levelData.levelBoard = boards[i];
                levelData.levelPreview = createdLevelPreview;
                    
                categories[currentCategory].levels.Add(levelData);

                LevelCard levelCard = Instantiate(createdLevelCardPrefab, levelsParent.transform);
                levelCard.ConfigureLevel(levelData, category, i + 1);
                if (ProgressManager.Instance.IsLevelUnlocked(currentCategory, i))
                {
                    levelCard.button.onClick.AddListener(() =>
                    {
                        currentLevel = index;

                        if (levelData.levelNameLocalized != null)
                        {
                            localizedLevelName.StringReference = levelData.levelNameLocalized;
                            localizedLevelName.RefreshString();
                        }

                        levelName.text = levelData.levelName;
                        levelPreview.sprite = levelData.levelPreview;
                        levelCard.button.Select();
                    });
                    levelCard.button.onClick.Invoke();
                    levelCard.editLevelButton.onClick.AddListener(() =>
                    {
                        GameManager.Instance.SetCurrentLevel(currentLevel);
                        GameManager.Instance.SetCurrentCategory(categories[currentCategory]);
                        levelCard.button.onClick.Invoke();
                        EditCreatedLevel();
                    });
                }
                else
                    levelCard.DeactivateCard();
            }
        }

        // Default Levels Categories
        else {
            playSelectedLevelButton.SetActive(true);
            for (int i = 0; i < category.levels.Count; i++)
            {
                int index = i;
                LevelData levelData = category.levels[i];
                LevelCard levelCard = Instantiate(levelCardPrefab, levelsParent.transform);
                levelCard.ConfigureLevel(levelData, category, i + 1);
                if (ProgressManager.Instance.IsLevelUnlocked(currentCategory, i))
                {
                    levelCard.button.onClick.AddListener(() =>
                    {
                        currentLevel = index;

                        localizedLevelName.StringReference = levelData.levelNameLocalized;
                        localizedLevelName.RefreshString();

                    //levelName.text = levelData.levelName;
                    levelPreview.sprite = levelData.levelPreview;
                        levelCard.button.Select();
                    });
                    levelCard.button.onClick.Invoke();
                }
                else
                    levelCard.DeactivateCard();
            }
        }

        TraceScreenAccesed();

    }

    public void HideLevels()
    {
        if (!levelsPanel.activeSelf) return;

        //currentCategoryLevelsText.text = "Categories";
        currentCategoryLevelsTextLocalized.StringReference = categoryNameLocaliced;
        currentCategoryLevelsTextLocalized.RefreshString();

        categoriesPanel.SetActive(true);
        levelsPanel.SetActive(false);

        currentCategoryPanel.SetActive(true);
        currentLevelPanel.SetActive(false);

        while (levelsParent.transform.childCount != 0)
            DestroyImmediate(levelsParent.transform.GetChild(0).gameObject);

        TraceScreenAccesed();
    }

    public void PlaySelectedLevel()
    {
        GameManager.Instance.LoadLevel(categories[currentCategory], currentLevel);
    }

    public void PlayLevelCreated()
    {
        if (levelCreatedIndex == -1)
        {
            GameManager.Instance.LoadLevelCreator();
        }
        else
        {
            GameManager.Instance.LoadLevel(levelsCreatedCategory, levelCreatedIndex);
        }
    }

    public void EditCreatedLevel()
    {
        LevelData thisleveldata = categories[currentCategory].levels[currentLevel];

        BoardState thisBoard = JsonUtility.FromJson<BoardState>(thisleveldata.levelBoard.text);
        ActiveBlocks thisActive = JsonUtility.FromJson<ActiveBlocks>(thisleveldata.activeBlocks.text);
        string thisInitial = thisleveldata.customInitialState.text;

        GameManager.Instance.SetCommunityLevelBoard(thisBoard);
        GameManager.Instance.SetCommunityLevelActiveBlocks(thisActive);
        GameManager.Instance.SetCommunityInitialState(thisInitial);
        GameManager.Instance.SetPlayingCommunityLevel(true);

        GameManager.Instance.LoadLevelCreator();
    }

    public void TraceScreenAccesed()
    {
        string nameID = "main";

        if (levelsPanel.activeSelf && currentCategory >= 0)
            nameID = categories[currentCategory].name_id;

        TrackerAsset.Instance.Accessible.Accessed("categories_" + nameID, AccessibleTracker.Accessible.Screen);

    }
}
