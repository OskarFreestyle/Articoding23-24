using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServerClasses;

public class BrowsePlaylistsDisplay : MonoBehaviour {

    [SerializeField] private CommunityPlaylistExpanded communityPlaylistExpandedPrefab;
    [SerializeField] private List<CommunityPlaylistExpanded> playlistList = new List<CommunityPlaylistExpanded>();

    public void Configure() {
        // Delete the old playlists
        foreach (CommunityPlaylistExpanded playlist in playlistList) {
            Destroy(playlist.gameObject);
        }
        playlistList.Clear();

        Debug.Log("Entra Configure PlaylistCard");
        ServerClasses.PlaylistPage playlistPage = CommunityManager.Instance.PublicPlaylists;

        foreach (ServerClasses.Playlist playlist in playlistPage.content) {
            CommunityPlaylistExpanded currentPlaylistCard = Instantiate(communityPlaylistExpandedPrefab, transform);

            playlistList.Add(currentPlaylistCard);

            currentPlaylistCard.ConfigurePlaylist(playlist);

            // Compare the id with the id liked list
            currentPlaylistCard.SetLikeState(GameManager.Instance.LikedPlaylistIDs.Contains(playlist.id));
        }
    }
}
