using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "Category", menuName = "ScriptableObjects/Category")]
public class CategoryDataSO : ScriptableObject {
    // New
    public LocalizedString titleLocalized;
    public LocalizedString descriptionLocalized;
    
    public Sprite icon;

    public Color primaryColor;
    public Color secondaryColor;

    public List<LevelDataSO> levels;

    public int GetTotalStars() {
        return levels.Count * 3;
    }

    public int GetTotalLevels() {
        return levels.Count;
    }

    // Old - BORRAR?
    public string name_id;
    [TextArea(3, 6)]
    public string description;
}
