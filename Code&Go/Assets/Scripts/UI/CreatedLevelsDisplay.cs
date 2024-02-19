using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatedLevelsDisplay : MonoBehaviour {

    [SerializeField] private LevelsDisplay levelsDisplay;
    [SerializeField] private CategoryDataSO createdLevelsCategory;

    private void Start() {
        levelsDisplay.InstanciateLevelsFromCategory(createdLevelsCategory);
    }
}
