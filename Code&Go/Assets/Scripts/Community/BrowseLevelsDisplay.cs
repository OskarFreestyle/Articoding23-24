using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServerClasses;

public class BrowseLevelsDisplay : MonoBehaviour {

    [SerializeField] private CommunityLevelExpanded communityLevelExpandedPrefab;
    [SerializeField] private List<CommunityLevelExpanded> levelList = new List<CommunityLevelExpanded>();

    public void Configure() {
        // Delete the old levels
        foreach (CommunityLevelExpanded level in levelList) {
            Destroy(level.gameObject);
        }
        levelList.Clear();

        Debug.Log("Entra Configure");
        ServerClasses.LevelPage levelPage = CommunityManager.Instance.PublicLevels;

        foreach (ServerClasses.LevelWithImage levelWithImage in levelPage.content)
        {
            Debug.Log("Instanciating level " + levelWithImage.level.title);
            CommunityLevelExpanded currentLevelCard = Instantiate(communityLevelExpandedPrefab, transform);

            levelList.Add(currentLevelCard);

            currentLevelCard.ConfigureLevel(levelWithImage);
        }

    }
}
