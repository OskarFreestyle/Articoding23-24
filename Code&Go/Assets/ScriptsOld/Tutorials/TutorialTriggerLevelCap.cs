using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TutorialTriggerLevelCap : MonoBehaviour {

    public TutorialTrigger tt;
    public LevelDataSO level;

    void Awake() {
        GameManager gameManager = GameManager.Instance;

        int levelID = gameManager.CurrentLevelIndex;

        //Para que salte ArgumentOutOfRangeException si esta en el editor de niveles
        if (levelID >= 0) {
            if (gameManager.GetCategories()[gameManager.CurrentCategoryIndex].levels[gameManager.CurrentLevelIndex] == level)
                tt.enabled = true;
            else
                tt.enabled = false;
        }

        if (ProgressManager.Instance.AllUnlocked)
            tt.enabled = true;
    }
}
