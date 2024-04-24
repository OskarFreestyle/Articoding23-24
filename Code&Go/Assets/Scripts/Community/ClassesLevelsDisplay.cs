using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassesLevelsDisplay : MonoBehaviour {

    [SerializeField] private CommunityLevelContract communityLevelContract;

    public void InstanciateClassLevels() {
        ClearDisplay();

        foreach (ServerClasses.LevelWithImage lWI in CommunityManager.Instance.ClassLevels.content) {

            CommunityLevelContract newCommunityLevelContract = Instantiate(communityLevelContract, transform);
            newCommunityLevelContract.ConfigureLevel(lWI);

            // Preguntar estado nivel
            newCommunityLevelContract.SetClassLevelState(CommunityManager.Instance.ClassLevelsPasses.Contains(lWI.level.id));

        }
    }

    private void ClearDisplay() {
        // Clear all the levelCards
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
    }
}