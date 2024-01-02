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
}

[System.Serializable]
public class LevelsCreatedSaveData {
    public string[] levelsCreated;
}

[System.Serializable]
public class CategorySaveData {
    public int[] levelsData;

    public int GetLevelsCompleted() {
        int levelsCompleted = 0;
        foreach (int levelData in levelsData)
            levelsCompleted += levelData >= 0 ? 1 : 0;
        return levelsCompleted;
    }

    public int GetCurrentNumStars() {
        int currentNumStars = 0;

        foreach (int levelData in levelsData)
            if (levelData != -1) currentNumStars += levelData;

        return currentNumStars;
    }
}

[System.Serializable]
public class TutorialSaveData {
    public string[] tutorials;
}