using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServerClasses;

public class BrowseLevelsDisplay : MonoBehaviour {

    [SerializeField] private CommunityLevelExpanded communityLevelExpandedPrefab;


    public void Configure() {
        Debug.Log("Entra Configure");
        ServerClasses.LevelPage levelPage = CommunityManager.Instance.PublicLevels;

        foreach (ServerClasses.Level level in levelPage.content)
        {
            Debug.Log("Instanciating level " + level.name);
            CommunityLevelExpanded currentLevelCard = Instantiate(communityLevelExpandedPrefab, transform);
            currentLevelCard.ConfigureLevel(level);
        }

    }
}
