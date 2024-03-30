using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ServerClasses;

public class BrowseLevelsDisplay : MonoBehaviour {

    [SerializeField] private CommunityLevelExpanded communityLevelExpandedPrefab;
    [SerializeField] private List<CommunityLevelExpanded> levelList = new List<CommunityLevelExpanded>();

    [SerializeField] private RectTransform textTransform;
    [SerializeField] private Text text;

    [SerializeField] private Button levelsLikedButtonA;
    [SerializeField] private Button levelsLikedButtonB;
    [SerializeField] private Button levelsSearchButton;
    [SerializeField] private Button levelsOrderButtonA;
    [SerializeField] private Button levelsOrderButtonB;

    public void Configure() {
        StartCoroutine(ConfigureCoroutine());
    }

    private IEnumerator ConfigureCoroutine() {
        ServerClasses.LevelPage levelPage = CommunityManager.Instance.PublicLevels;

        foreach (ServerClasses.LevelWithImage levelWithImage in levelPage.content) {
            CommunityLevelExpanded currentLevelCard = Instantiate(communityLevelExpandedPrefab, transform);

            levelList.Add(currentLevelCard);
            currentLevelCard.ConfigureLevel(levelWithImage);
            // Compare the id with the id liked list
            currentLevelCard.SetLikeState(GameManager.Instance.LikedLevelIDs.Contains(levelWithImage.level.id));
            // Set the state to play or to create playlsit
            currentLevelCard.SetPlaylistMode(CommunityManager.Instance.IsPlaylistMode);
            currentLevelCard.gameObject.SetActive(false);

            yield return new WaitForEndOfFrame();
        }

        Show();
        yield return null;
    }

    public void Show() {
        CommunityManager.Instance.HideLoadingCircle();
        foreach (CommunityLevelExpanded level in levelList) {
            level.gameObject.SetActive(true);
        }
    }

    public void ClearDisplay() {
        // Delete the old levels
        foreach (CommunityLevelExpanded level in levelList) {
            Destroy(level.gameObject);
        }
        levelList.Clear();
    }

    public void SetPlaylistState(bool isPlaylist, string name = "") {
        textTransform.gameObject.SetActive(isPlaylist);
        text.text = name;
        if (isPlaylist) {
            levelsLikedButtonA.gameObject.SetActive(false);
            levelsLikedButtonB.gameObject.SetActive(false);
            levelsSearchButton.gameObject.SetActive(false);
            levelsOrderButtonA.gameObject.SetActive(false);
            levelsOrderButtonB.gameObject.SetActive(false);
        }
        else {
            levelsLikedButtonA.gameObject.SetActive(true);
            levelsLikedButtonB.gameObject.SetActive(false);
            levelsSearchButton.gameObject.SetActive(true);
            levelsOrderButtonA.gameObject.SetActive(true);
            levelsOrderButtonB.gameObject.SetActive(false);
        }
    }
}
