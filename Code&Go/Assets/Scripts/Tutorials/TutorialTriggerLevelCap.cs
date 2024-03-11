using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTriggerLevelCap : MonoBehaviour {

    public TutorialTrigger tt;
    public LevelDataSO level;

    private GameObject gameObject;

    void Awake() {
        GameManager gameManager = GameManager.Instance;
        int categoryIndex = level.categoryData.index;
        int levelIndex = level.index;

        gameObject = transform.gameObject;

        //Para que salte ArgumentOutOfRangeException si esta en el editor de niveles
        if (levelIndex >= 0 && categoryIndex >= 0 && !GameManager.Instance.IsPlayingCommunityLevel()) {
            if(gameManager.GetCategories()[gameManager.CurrentCategoryIndex].levels[gameManager.CurrentLevelIndex] == level)
                tt.enabled = true;
            else
                tt.enabled = false;
        }

        if (ProgressManager.Instance.AllUnlocked)
            tt.enabled = true;
    }
}
