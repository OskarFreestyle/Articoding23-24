using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddedLevelsPlaylistDisplay : MonoBehaviour {

    [SerializeField] private CommunityLevelContract communityLevelContractPrefab;
    [SerializeField] private RectTransform addLevelCard;
    [SerializeField] private InputField nameField;

    private int levelCount = 0;
    private int maxLevels = 3;

    public void AddLevel(ServerClasses.LevelWithImage lWI) {
        addLevelCard.SetParent(null);

        // Instanciate the new level Contract
        CommunityLevelContract cLC = Instantiate(communityLevelContractPrefab, transform);

        cLC.ConfigureLevel(lWI);

        addLevelCard.SetParent(transform);

        AddLevelCount(1);
    }

    public void ClearDisplay() {
        addLevelCard.SetParent(null);
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
        addLevelCard.SetParent(transform);
        nameField.text = "";
    }

    public void AddLevelCount(int a) {
        levelCount += a;
        CheckLevelsNumber();
    }

    public void CheckLevelsNumber() {
        Debug.Log("Checking with " + transform.childCount + " children");
        if(levelCount >= maxLevels) {
            addLevelCard.gameObject.SetActive(false);
        }
        else addLevelCard.gameObject.SetActive(true);
    }
}