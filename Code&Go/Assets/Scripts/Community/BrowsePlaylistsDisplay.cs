using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServerClasses;

public class BrowsePlaylistsDisplay : MonoBehaviour {

    [SerializeField] private CommunityPlaylistExpanded communityPlaylistExpandedPrefab;
    [SerializeField] private List<CommunityPlaylistExpanded> playlistList = new List<CommunityPlaylistExpanded>();

    public void Configure() {
        StartCoroutine(ConfigureCoroutine());
    }

    private IEnumerator ConfigureCoroutine() {
        ServerClasses.PlaylistPage playlistPage = CommunityManager.Instance.PublicPlaylists;

        foreach (ServerClasses.Playlist playlist in playlistPage.content) {
            CommunityPlaylistExpanded currentPlaylistCard = Instantiate(communityPlaylistExpandedPrefab, transform);

            playlistList.Add(currentPlaylistCard);
            currentPlaylistCard.gameObject.SetActive(false);
            currentPlaylistCard.ConfigurePlaylist(playlist);
            // Compare the id with the id liked list
            currentPlaylistCard.SetLikeState(GameManager.Instance.LikedLevelIDs.Contains(playlist.id));




            // TODO
            //currentPlaylistCard.SetLikeState(GameManager.Instance.LikedPlaylistIDs.Contains(playlist.id));







            yield return new WaitForEndOfFrame();
        }

        Show();
        yield return null;
    }

    public void Show() {
        CommunityManager.Instance.HideLoadingCircle();
        foreach (CommunityPlaylistExpanded list in playlistList) {
            list.gameObject.SetActive(true);
        }
    }

    public void ClearDisplay() {
        // Delete the old levels
        foreach (CommunityPlaylistExpanded list in playlistList) {
            Destroy(list.gameObject);
        }
        playlistList.Clear();
    }
}
