using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CategoryManager : MonoBehaviour {

    public void PlayLevelCreated() {
        Debug.Log("TRY");
        if (ProgressManager.Instance.GetCategoryData(0).levelsData.Length == 0) {
            GameManager.Instance.LoadLevelCreator();
        }
        //else {
        //    GameManager.Instance.LoadLevel(levelsCreatedCategory, levelCreatedIndex);
        //}
    }


}
