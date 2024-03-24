using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddedLevelsPlaylistDisplay : MonoBehaviour {

    [SerializeField] private CommunityLevelContract communityLevelContractPrefab;
    [SerializeField] private RectTransform addLevelCard;

    public void AddLevel(ServerClasses.LevelWithImage lWI) {
        addLevelCard.SetParent(null);

        // Instanciate the new level Contract
        CommunityLevelContract cLC = Instantiate(communityLevelContractPrefab, transform);

        cLC.ConfigureLevel(lWI);

        addLevelCard.SetParent(transform);
    }
}