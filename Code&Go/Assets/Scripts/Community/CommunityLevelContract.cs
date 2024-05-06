using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CommunityLevelContract : MonoBehaviour {


    [SerializeField] private Text levelName;
    [SerializeField] private Image levelImage;
    [SerializeField] private RectTransform completeTextTransform;
    [SerializeField] private RectTransform incompleteTextTransform;

    private ServerClasses.Level level;
    private int levelID;

    public void ConfigureLevel(ServerClasses.LevelWithImage levelWithImage) {
        // Set the level data
        level = levelWithImage.level;
        levelID = level.id;
        levelName.text = level.title;

        // Set the level image (string to .png)
        byte[] imageBytes = Convert.FromBase64String(levelWithImage.image);

        Texture2D tex = new Texture2D(1, 1);
        if (ImageConversion.LoadImage(tex, imageBytes)) {
            //Debug.Log("tex created correctly");
        }
        else Debug.Log("error load image into texture");

        Rect rect = new Rect(0, 0, 200, 200);
        Vector2 pivot = new Vector2(0.5f, 0.5f);
        levelImage.sprite = Sprite.Create(tex, rect, pivot);
    }

    public void PlayLevel() {
        // Save the data of the level
        GameManager.Instance.currentLevelID = level.id;

        // Set the level data
        GameManager.Instance.LevelName = level.title;
        GameManager.Instance.SetCommunityLevelBoard(level.articodingLevel.boardstate);
        GameManager.Instance.SetCommunityLevelActiveBlocks(level.articodingLevel.activeblocks);
        GameManager.Instance.SetCommunityInitialState(level.articodingLevel.initialState);
        GameManager.Instance.LoadCommunityLevel();
        //GameManager.Instance.CurrentLevel = level.articodingLevel;
        GameManager.Instance.isClassLevel = true;

        // Change the scene
        if (LoadManager.Instance == null) {
            SceneManager.LoadScene("LevelScene");
            return;
        }

        LoadManager.Instance.LoadScene("LevelScene");

        Debug.Log("Play level finish");
    }

    public void DeleteFromPlaylist() {
        CommunityManager.Instance.RemoveFromPlaylist(levelID);

        Destroy(gameObject);
    }

    public void SetClassLevelState(bool state) {
        if (completeTextTransform) completeTextTransform.gameObject.SetActive(state);
        if (incompleteTextTransform) incompleteTextTransform.gameObject.SetActive(!state);
    }
}
