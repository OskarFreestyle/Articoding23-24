using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class UploadLevelsDisplay : MonoBehaviour {

    [SerializeField] private UploadLevelCard uploadLevelCardTemplate;
    [SerializeField] private List<Vector3> levelsLocalPositions;

    public void InstanciateCreatedLevels() {
        StartCoroutine(InstanciateCreatedLevelsCoroutine());
    }

    IEnumerator InstanciateCreatedLevelsCoroutine() {
        ClearDisplay();
        
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
            yield return new WaitForEndOfFrame();
        }

        // Read the icons
        for (int i = 0; i < levelPreviewPaths.Length; i++) {
            levelIconsSprites[i] = TextureReader.LoadNewSprite(levelPreviewPaths[i]);
        }

        List<LevelDataSO> levelDataSOs = new List<LevelDataSO>();

        for (int i = 0; i < boards.Length; i++) {
            int index = i;
            LevelDataSO levelData = ScriptableObject.CreateInstance<LevelDataSO>();
            levelData.categoryData = GameManager.Instance.GetCategoryByIndex(0);
            levelData.levelName = fileNames[i];
            levelData.activeBlocks = activeBlocks[i];
            levelData.customInitialState = initialBlocks[i];
            levelData.levelBoard = boards[i];
            levelData.index = i;
            try {
                levelData.levelImage = levelIconsSprites[i];
            }
            catch (System.Exception e) {
                Debug.Log("Error reading the level preview: " + e);
            }
            // Add the data
            levelDataSOs.Add(levelData);
            yield return new WaitForEndOfFrame();
        }

        int x = 0;
        int y = 0;
        // Create the others cards
        foreach (LevelDataSO levelDataSO in levelDataSOs) {
            Debug.Log("Instanciating level " + levelDataSO.levelName);

            UploadLevelCard currentLevelCard = Instantiate(uploadLevelCardTemplate, transform);
            currentLevelCard.Configure(levelDataSO);
            currentLevelCard.transform.localPosition = levelsLocalPositions[x] - new Vector3(0, (y * 385), 0);
            currentLevelCard.gameObject.SetActive(false);

            // Check the names to set uploaded the already uploaded
            Debug.Log("Count " + CommunityManager.Instance.UploadedLevels.content.Count);
            for (int i = 0; i < CommunityManager.Instance.UploadedLevels.content.Count; i++) {
                Debug.Log($"{levelDataSO.levelName} - {CommunityManager.Instance.UploadedLevels.content[i].level.title}");
                if(levelDataSO.levelName == CommunityManager.Instance.UploadedLevels.content[i].level.title) {
                    currentLevelCard.AlreadyUploaded();
                }
            }

            x++;
            if (x > 4) {
                y++;
                x = 0;
            }
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("Instaciation Upload Levels Finished");

        Show();
        CommunityManager.Instance.HideLoadingCircle();
        yield return null;
    }

    public void Show() {
        foreach (Transform child in transform) {
            child.gameObject.SetActive(true);
        }
    }

    public void ClearDisplay() {
        // Clear all the levelCards
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
    }
}
