using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "Category", menuName = "ScriptableObjects/Category")]
public class Category : ScriptableObject
{
    // New

    public LocalizedString titleLocalized;
    public LocalizedString descriptionLocalized;
    
    public Sprite icon;

    public Color primaryColor;
    public Color secondaryColor;

    public List<LevelData> levels;

    public int GetTotalStars() {
        return levels.Count * 3;
    }


    // Old
    public string name_id;
    [TextArea(3, 6)]
    public string description;



}
