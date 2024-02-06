using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;

public class LevelsDisplay : MonoBehaviour {

    [SerializeField] private Text titleText;
    [SerializeField] private LocalizeStringEvent titleLocalized;
    [SerializeField] private Image titleBGImage;

    [SerializeField] private Image backButton;

    [SerializeField] private LevelCard levelCardTemplate;
    [SerializeField] private LevelCard createLevelCardTemplate;
    [SerializeField] private List<Vector3> levelsLocalPositions;

    [SerializeField] private Transform levelsPage;

    public void InstanciateLevelsFromCategory(CategoryDataSO category) {
        // Active the levels page
        levelsPage.gameObject.SetActive(true);

        // Set the title
        titleLocalized.StringReference = category.titleLocalized;
        titleLocalized.RefreshString();
        titleText.text = titleLocalized.StringReference.GetLocalizedStringAsync().Result;
        titleBGImage.color = category.secondaryColor;

        // Set the back button color
        backButton.color = category.secondaryColor;

        // Normal Categories
        if(category.name != "0_Levels_Created") {
            // Set the data for all the levels
            int i = 0;
            int x = 0;
            int y = 0;
            foreach (LevelDataSO levelData in category.levels) {
                LevelCard currentLevelCard = Instantiate(levelCardTemplate, transform);
                currentLevelCard.SetLevelData(levelData);
                currentLevelCard.SetLevelStars(ProgressManager.Instance.GetLevelStars(category, i));
                currentLevelCard.transform.localPosition = levelsLocalPositions[x] - new Vector3(0, (y * 385), 0);
                i++;
                x++;
                if (x > 4) {
                    y++;
                    x = 0;
                }
            }
        }
        // Levels Created category
        else {
            // Clear the lastest levels data
            category.levels.Clear();

            // Get the paths of all the files
            string[] boardFilePaths = Directory.GetFiles(Application.dataPath + "/Levels/Boards/0_CreatedLevels", "*.json");
            string[] activeFilePaths = Directory.GetFiles(Application.dataPath + "/Levels/ActiveBlocks/0_CreatedLevels", "*.json");
            string[] initialFilePaths = Directory.GetFiles(Application.dataPath + "/Levels/InitialStates/0_CreatedLevels", "*.txt");
            string[] levelPreviewPaths = Directory.GetFiles(Application.dataPath + "/Levels/LevelPreviewIcons/0_CreatedLevels", "*.png");
            string[] fileNames = new string[boardFilePaths.Length];
            
            // Create an array for each kind of asset
            TextAsset[] boards = new TextAsset[boardFilePaths.Length];
            TextAsset[] activeBlocks = new TextAsset[activeFilePaths.Length];
            TextAsset[] initialBlocks = new TextAsset[initialFilePaths.Length];
            Sprite[] levelIconsSprites = new Sprite[levelPreviewPaths.Length];

            // Read the textAssets
            for (int i = 0; i < boardFilePaths.Length; i++) {
                boards[i] = new TextAsset(File.ReadAllText(boardFilePaths[i]));
                activeBlocks[i] = new TextAsset(File.ReadAllText(activeFilePaths[i]));
                initialBlocks[i] = new TextAsset(File.ReadAllText(initialFilePaths[i]));
                fileNames[i] = Path.GetFileNameWithoutExtension(boardFilePaths[i]);
            }

            // Read the icons
            for (int i = 0; i < levelPreviewPaths.Length; i++) {
                levelIconsSprites[i] = TextureReader.LoadNewSprite(levelPreviewPaths[i]);
            }

            List<LevelDataSO> levelDataSOs = new List<LevelDataSO>();

            for (int i = 0; i < boards.Length; i++) {
                int index = i;
                LevelDataSO levelData = new LevelDataSO();
                levelData.categoryData = GameManager.Instance.GetCategoryByIndex(0);
                levelData.levelName = fileNames[i];
                levelData.activeBlocks = activeBlocks[i];
                levelData.customInitialState = initialBlocks[i];
                levelData.levelBoard = boards[i];
                levelData.index = i;
                try {
                    levelData.levelPreview = levelIconsSprites[i];
                } catch(System.Exception e) {
                    Debug.Log("Error reading the level preview: " + e);
                }
                // Add the data
                levelDataSOs.Add(levelData);
                category.levels.Add(levelData);
            }

            // Create the create level card 
            int x = 0;
            int y = 0;
            LevelCard createLevelCard = Instantiate(createLevelCardTemplate, transform);
            createLevelCard.transform.localPosition = levelsLocalPositions[x];
            x++;

            // Create the others cards
            foreach (LevelDataSO levelData in levelDataSOs) {

                LevelCard currentLevelCard = Instantiate(levelCardTemplate, transform);
                currentLevelCard.SetLevelData(levelData);
                currentLevelCard.DisableStars();
                currentLevelCard.transform.localPosition = levelsLocalPositions[x] - new Vector3(0, (y * 385), 0);

                x++;
                if (x > 4) {
                    y++;
                    x = 0;
                }
            }
        }
    }

    public void ClearDisplay() {
        // Clear all the levelCards
        foreach(Transform child in transform) {
            Destroy(child.gameObject);
        }

        // Then disable the gameObject
        levelsPage.gameObject.SetActive(false);
    }
}
