using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CommunityLevelExpanded : MonoBehaviour {

    [SerializeField] private Text levelName;
    [SerializeField] private Text levelAuthor;
    [SerializeField] private Text levelID;
    [SerializeField] private Image levelImage;
    [SerializeField] private Text levelLikes;
    [SerializeField] private Text levelPlays;
    [SerializeField] private Text levelHashtags;
    [SerializeField] private Button likedEnableButton;
    [SerializeField] private Button likedDisableButton;
    [SerializeField] private RectTransform playButtonTransform;
    [SerializeField] private RectTransform addPlaylistButtonTransform;

    public ServerClasses.LevelWithImage levelWithImage;
    private ServerClasses.Level level;


    public void ConfigureLevel(ServerClasses.LevelWithImage lWI) {
        // Set the level data
        levelWithImage = lWI;
        level = lWI.level;
        levelName.text = level.title;
        levelAuthor.text = "Author: " + level.owner.username;
        levelID.text = "ID: " + level.id.ToString();
        levelLikes.text = level.likes.ToString();
        levelPlays.text = level.timesPlayed.ToString();

        // Set the level image (string to .png)
        byte[] imageBytes = Convert.FromBase64String(lWI.image);

        Texture2D tex = new Texture2D(1, 1);
        if (ImageConversion.LoadImage(tex, imageBytes)) {
            //Debug.Log("tex created correctly");
        }
        else Debug.Log("error load image into texture");

        Rect rect = new Rect(0, 0, 200, 200);
        Vector2 pivot = new Vector2(0.5f, 0.5f);
        levelImage.sprite = Sprite.Create(tex, rect, pivot);
    }

    public void LikeLevel(bool state) {
        if (state) {
            levelLikes.text = (int.Parse(levelLikes.text) + 1).ToString();
        }
        else {
            levelLikes.text = (int.Parse(levelLikes.text) - 1).ToString();
        }
        CommunityManager.Instance.ModifyLikesLevel(level.id.ToString(), state);
    }

    public void PlayLevel() {
        GameManager.Instance.LevelName = level.title;
        levelPlays.text = (int.Parse(levelPlays.text) + 1).ToString();
        CommunityManager.Instance.IncreasePlays(levelID.text);

        // Set the level data
        GameManager.Instance.SetCommunityLevelBoard(level.articodingLevel.boardstate);
        GameManager.Instance.SetCommunityLevelActiveBlocks(level.articodingLevel.activeblocks);
        GameManager.Instance.SetCommunityInitialState(level.articodingLevel.initialState);
        GameManager.Instance.LoadCommunityLevel();

        // Change the scene
        if (LoadManager.Instance == null) {
            SceneManager.LoadScene("LevelScene");
            return;
        }

        LoadManager.Instance.LoadScene("LevelScene");

        Debug.Log("Play level finish");
    }

    public void SetLikeState(bool state) {
        likedEnableButton.gameObject.SetActive(!state);
        likedDisableButton.gameObject.SetActive(state);
    }

    public void SetPlaylistMode(bool state) {
        addPlaylistButtonTransform.gameObject.SetActive(state);
        playButtonTransform.gameObject.SetActive(!state);
    }

    public void AddToPlaylist() {
        Debug.Log("Add to playlist");
        CommunityManager.Instance.AddToCreatingPlaylist(levelWithImage);
    }
}
