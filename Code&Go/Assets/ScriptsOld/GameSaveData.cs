using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData {
    public GameSaveData gameData;
    public string hash;
}

[System.Serializable]
public class GameSaveData {
    public ProgressSaveData progressData;
    public TutorialSaveData tutorialInfo;
}

[System.Serializable]
public class ProgressSaveData {
    public LevelsCreatedSaveData levelsCreatedData;
    public CategorySaveData[] categoriesInfo;

    public void DebugLogCategoriesData() {
        int i = 0;
        foreach (CategorySaveData categorySaveData in categoriesInfo) {
            Debug.Log("Category " + i + ": " + categorySaveData.DebugLogLevelsData());
            i++;
        }
    }
}

[System.Serializable]
public class LevelsCreatedSaveData {
    public string[] levelsCreated;
}

[System.Serializable]
public class CategorySaveData {
    public int[] levelsData;

    public int GetTotalLevels() {
        int totalLevels = 0;
        foreach (int levelData in levelsData)
            totalLevels += 1;
        return totalLevels;
    }

    public int GetLevelsCompleted() {
        int levelsCompleted = 0;
        foreach (int levelData in levelsData)
            levelsCompleted += levelData > 0 ? 1 : 0;
        return levelsCompleted;
    }

    public int GetPerfectLevelsCompleted() {
        int perfectLevelsCompleted = 0;
        foreach (int levelData in levelsData)
            perfectLevelsCompleted += levelData >= 3 ? 1 : 0;
        return perfectLevelsCompleted;
    }

    public int GetCurrentNumStars() {
        int currentNumStars = 0;
        foreach (int levelData in levelsData)
            if (levelData != -1) currentNumStars += levelData;
        return currentNumStars;
    }

    public string DebugLogLevelsData() {
        string line = "";
        foreach (int level in levelsData) {
            line += " " + level + " ";
        }
        return line;
    }

    public bool IsCategoryFinished() {
        bool isFinished = true;
        int index = 0;
        while (isFinished && index < levelsData.Length) {
            isFinished = levelsData[index] != -1;
            index++;
        }
        return isFinished;
    }
}

[System.Serializable]
public class TutorialSaveData {
    public string[] tutorials;
}