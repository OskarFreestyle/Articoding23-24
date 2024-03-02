using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrowseLevelsDisplay : MonoBehaviour {

    [SerializeField] private CommunityLevelExpanded communityLevelExpandedPrefab;


    public void Configure() {
        for (int i = 0; i < 15; i++) {
            Debug.Log("Instanciating level " + i);

            CommunityLevelExpanded currentLevelCard = Instantiate(communityLevelExpandedPrefab, transform);
            //currentLevelCard.ConfigureLevel();
        }
    }
}
