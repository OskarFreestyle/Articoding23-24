using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UploadLevelCard : MonoBehaviour {

    [SerializeField] private Text levelName;
    [SerializeField] private Image levelImage;

    private LevelDataSO levelDataSO;

    public void Configure(LevelDataSO ldSO) {
        levelDataSO = ldSO;
        levelName.text = levelDataSO.levelName;
        levelImage.sprite = levelDataSO.levelImage;
    }

    public void UploadLevel() {
        CommunityManager.Instance.UploadLevel(levelDataSO);
    }

}
