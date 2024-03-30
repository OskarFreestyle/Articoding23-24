using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CommunityPlaylistExpanded : MonoBehaviour {

    [SerializeField] private Text playlistName;
    [SerializeField] private Text playlistAuthor;
    [SerializeField] private Text playlistID;
    [SerializeField] private List<Sprite> playlistSprites;  // List of images
    [SerializeField] private Image currentImage;
    [SerializeField] private Text playlistLikes;
    [SerializeField] private Text playlistPlays;
    [SerializeField] private Text playlistHashtags;
    [SerializeField] private Button likedEnableButton;
    [SerializeField] private Button likedDisableButton;

    private ServerClasses.Playlist playlist;
    private List<ServerClasses.LevelWithImage> levelsWithImage;

    [SerializeField] private float timeToChangeImage;
    private float timer;
    private int currentSpriteIndex;

    public void ConfigurePlaylist(ServerClasses.Playlist p) {
        Debug.Log($"Configure playlist {p.title}");

        // Set the level data
        playlist = p;
        playlistName.text = playlist.title;
        playlistAuthor.text = playlist.owner.username;
        playlistID.text = playlist.id.ToString();
        playlistLikes.text = playlist.likes.ToString();
        playlistPlays.text = playlist.timesPlayed.ToString();

        levelsWithImage = new List<ServerClasses.LevelWithImage>();
        playlistSprites = new List<Sprite>();

        foreach (ServerClasses.LevelWithImage lwi in playlist.levelsWithImage) {
            //Debug.Log(lwi.level.id);
            levelsWithImage.Add(lwi);

            // Set the level image (string to .png)
            byte[] imageBytes = Convert.FromBase64String(lwi.image);

            Texture2D tex = new Texture2D(1, 1);
            if (ImageConversion.LoadImage(tex, imageBytes)) {
                //Debug.Log("tex created correctly");
            }
            else Debug.Log("error load image into texture");

            Rect rect = new Rect(0, 0, 1000, 1000);
            Vector2 pivot = new Vector2(0.5f, 0.5f);

            playlistSprites.Add(Sprite.Create(tex, rect, pivot));
        }

        currentSpriteIndex = 0;
        currentImage.sprite = playlistSprites[currentSpriteIndex];
    }

    private void Update() {
        if (gameObject.activeInHierarchy) {
            timer += Time.deltaTime;
            if (timer >= timeToChangeImage) {
                timer = 0.0f;

                currentSpriteIndex++;
                if (currentSpriteIndex >= playlistSprites.Count) currentSpriteIndex = 0;

                currentImage.sprite = playlistSprites[currentSpriteIndex];
            }
        }
    }

    public void LikePlaylist(bool state) {
        if (state) {
            playlistLikes.text = (int.Parse(playlistLikes.text) + 1).ToString();
        }
        else {
            playlistLikes.text = (int.Parse(playlistLikes.text) - 1).ToString();
        }
        CommunityManager.Instance.ModifyLikesPlaylist(playlistID.text, state);
    }

    public void OpenPlaylist() {
        Debug.Log("Open playlist");
        CommunityManager.Instance.OpenPlaylist(playlist);
    }

    public void SetLikeState(bool state) {
        likedEnableButton.gameObject.SetActive(!state);
        likedDisableButton.gameObject.SetActive(state);
    }
}
