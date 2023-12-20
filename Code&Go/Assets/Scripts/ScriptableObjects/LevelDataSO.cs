using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Level")]
public class LevelDataSO : ScriptableObject {
    // New
    public CategoryDataSO categoryData;

    public LocalizedString levelNameLocalized;

    public Sprite levelPreview;
    
    // Old
    public string levelName;

    public LocalizedAsset<TextAsset> initialState; // Estado inicial en .xml
    public TextAsset customInitialState = null;

    [Header("Active Blocks")]
    public TextAsset activeBlocks;//Bloques y categorias disponibles    
    public bool allActive = false;

    [Space(10)]
    public TextAsset levelBoard;
    public string auxLevelBoard;
    
    [Space(10)] 
    public int minimosPasos;
    
    public LocalizedString endTextLocalized;
}
