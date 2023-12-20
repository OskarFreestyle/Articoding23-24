using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TutorialTriggerLevelCap : MonoBehaviour
{

    public TutorialTrigger tt;
    public LevelDataSO level;

    void Awake()
    {
        GameManager gm = GameManager.Instance;
        int levelID = gm.GetCurrentLevelIndex();
        if (levelID >= 0) //Para que salte ArgumentOutOfRangeException si esta en el editor de niveles
        {
            if (gm.GetCurrentCategory().levels[gm.GetCurrentLevelIndex()] == level)
                tt.enabled = true;
            else
                tt.enabled = false;
        }

        if (ProgressManager.Instance.IsAllUnlockedModeOn())
            tt.enabled = true;
    }
}
