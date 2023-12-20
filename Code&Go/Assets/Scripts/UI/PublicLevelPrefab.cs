using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PublicLevelPrefab : LevelPrefabOld
{
    public Text username;

    public override void SetLevel(ServerClasses.Level level, ClasesManager clas, ComunidadLayout com)
    {
        base.level = level;
        levelName.text = level.title;
        clasesManager = clas;

        playButton.onClick.AddListener(() =>
        {
            com.PlayPublicLevel(base.level);
        });

        saveButton.onClick.AddListener(() =>
        {
            com.SavePublicLevel(base.level);
        });

        username.text = level.owner.username;
    }
}
