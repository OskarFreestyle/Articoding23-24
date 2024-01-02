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
    public LevelSaveData[] levelsData;

    public int GetLevelsCompleted() {
        int levelsCompleted = 0;
        foreach (LevelSaveData levelData in levelsData)
            levelsCompleted += levelData.stars >= 0 ? 1 : 0;
        return levelsCompleted;
    }

    public int GetCurrentNumStars() {
        int currentNumStars = 0;

        foreach (LevelSaveData levelData in levelsData)
            currentNumStars += levelData.stars + 1;            //totalStars += levelData.stars; TODO quitar el +1

        return currentNumStars;
    }
}

[System.Serializable]
public class LevelSaveData {
    public int stars = -1; // -1 if the level is not completed
}

[System.Serializable]
public class TutorialSaveData {
    public string[] tutorials;
}