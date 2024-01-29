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
    [SerializeField] private List<Vector3> levelsLocalPositions;

    public void InstanciateLevelsFromCategory(CategoryDataSO category) {
        // Active the levels page
        transform.parent.gameObject.SetActive(true);

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
            foreach(LevelDataSO levelData in category.levels) {
                LevelCard currentLevelCard = Instantiate(levelCardTemplate, transform);
                currentLevelCard.SetLevelData(levelData);
                currentLevelCard.SetLevelStars(ProgressManager.Instance.GetLevelStars(category, i));
                currentLevelCard.transform.localPosition = levelsLocalPositions[i];
                i++;
            }
        }
        // Levels Created category
        else {
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
                try {
                    levelData.levelPreview = levelIconsSprites[i];
                } catch(System.Exception e) {
                    Debug.Log("Error reading the level preview: " + e);
                }
                levelDataSOs.Add(levelData);
            }

            int x = 0;
            foreach (LevelDataSO levelData in levelDataSOs) {
                LevelCard currentLevelCard = Instantiate(levelCardTemplate, transform);
                currentLevelCard.SetLevelData(levelData);
                currentLevelCard.DisableStars();
                currentLevelCard.transform.localPosition = levelsLocalPositions[x];
                x++;
            }
        }
    }

    //public Sprite LoadNewSprite(string FilePath, float PixelsPerUnit = 100.0f, SpriteMeshType spriteType = SpriteMeshType.Tight) {
    //    // Load a PNG or JPG image from disk to a Texture2D, assign this texture to a new sprite and return its reference
    //    Texture2D SpriteTexture = LoadTexture(FilePath);
    //    Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0), PixelsPerUnit, 0, spriteType);

    //    return NewSprite;
    //}

    //public Texture2D LoadTexture(string FilePath) {

    //    // Load a PNG or JPG file from disk to a Texture2D
    //    // Returns null if load fails

    //    Texture2D Tex2D;
    //    byte[] FileData;

    //    if (File.Exists(FilePath)) {
    //        FileData = File.ReadAllBytes(FilePath);
    //        Tex2D = new Texture2D(2, 2);           // Create new "empty" texture
    //        if (Tex2D.LoadImage(FileData))           // Load the imagedata into the texture (size is set automatically)
    //            return Tex2D;                 // If data = readable -> return texture
    //    }
    //    return null;                     // Return null if load failed
    //}

    public void ClearDisplay() {
        // Clear all the levelCards
        foreach(Transform child in transform) {
            Destroy(child.gameObject);
        }

        // Then disable the gameObject
        transform.parent.gameObject.SetActive(false);
    }
}
