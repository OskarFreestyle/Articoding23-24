using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelPrefab : MonoBehaviour
{
    protected ServerClasses.Level level;

    public Text levelName;

    public Button playButton;
    public Button saveButton;

    protected ClasesManager clasesManager;

    int levelListId = 0;

    public virtual void SetLevel(ServerClasses.Level level, ClasesManager clas, ComunidadLayout com)
    {
        this.level = level;
        levelName.text = level.title;
        clasesManager = clas;

        playButton.onClick.AddListener(() =>
        {
            clas.SetLevelIndex(levelListId);
            com.PlayCommunityLevel();
        });

        saveButton.onClick.AddListener(() =>
        {
            clas.SetLevelIndex(levelListId);
            com.SaveCommunityLevel();
        });
    }

    public void DeactivateSave()
    {
        saveButton.GetComponent<Button>().interactable = false;
    }
    public void SetLevelListId(int index) { levelListId = index; }
    public int GetLeveListId() { return levelListId; }
}
