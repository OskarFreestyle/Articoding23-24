using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsDisplay : MonoBehaviour
{
    [SerializeField] private LevelCard levelCardTemplate;
    [SerializeField] private CategoryDataSO category;

    [SerializeField] private List<Vector3> levelsLocalPositions;

    private void Start() {
        InstanciateLevels(category);
    }

    public void InstanciateLevels(CategoryDataSO category) {
        int i = 0;
        foreach(LevelDataSO levelData in category.levels) {
            LevelCard currentLevelCard = Instantiate(levelCardTemplate, transform);
            currentLevelCard.SetLevelData(levelData);
            currentLevelCard.transform.localPosition = levelsLocalPositions[i];
            i++;
        }
    }

}
