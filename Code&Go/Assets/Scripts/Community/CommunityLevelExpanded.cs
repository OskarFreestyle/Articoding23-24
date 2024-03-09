using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommunityLevelExpanded : MonoBehaviour {

    [SerializeField] private Text levelName;
    [SerializeField] private Text levelAuthor;
    [SerializeField] private Text levelID;
    [SerializeField] private Image levelImage;
    [SerializeField] private Text levelLikes;
    [SerializeField] private Text levelPlays;
    [SerializeField] private Text levelHashtags;
    public Texture2D textureDelete;

    public void ConfigureLevel(ServerClasses.LevelWithImage levelWithImage) {
        // Set the level data
        ServerClasses.Level level = levelWithImage.level;
        levelName.text = level.title;
        levelAuthor.text = level.owner.username;
        levelID.text = level.id.ToString();
        levelLikes.text = level.likes.ToString();
        levelPlays.text = level.timesPlayed.ToString();

        // Set the level image (string to .png)
        byte[] imageBytes = Convert.FromBase64String(levelWithImage.image);

        Texture2D tex = new Texture2D(1, 1);
        if (ImageConversion.LoadImage(tex, imageBytes)) {
            Debug.Log("tex created correctly");
        }
        else Debug.Log("error load image into texture");

        Rect rect = new Rect(0, 0, 1000, 1000);
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
        CommunityManager.Instance.ModifyLikes(levelID.text, state);
    }

    public void PlayLevel() {
        levelPlays.text = (int.Parse(levelPlays.text) + 1).ToString();
        CommunityManager.Instance.IncreasePlays(levelID.text);
    }



}
