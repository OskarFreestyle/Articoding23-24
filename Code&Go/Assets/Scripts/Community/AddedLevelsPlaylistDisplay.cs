using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddedLevelsPlaylistDisplay : MonoBehaviour {

    [SerializeField] private CommunityLevelContract communityLevelContractPrefab;
    [SerializeField] private RectTransform addLevelCard;
    [SerializeField] private InputField nameField;

    public void AddLevel(ServerClasses.LevelWithImage lWI) {
        addLevelCard.SetParent(null);

        // Instanciate the new level Contract
        CommunityLevelContract cLC = Instantiate(communityLevelContractPrefab, transform);

        cLC.ConfigureLevel(lWI);

        addLevelCard.SetParent(transform);
    }

    public void ClearDisplay() {
        addLevelCard.SetParent(null);
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
        addLevelCard.SetParent(transform);
        nameField.text = "";
    }
}