using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

//public class LevelCardOld : MonoBehaviour
//{
//    private LevelDataSO level;
//    private CategoryDataSO category;
//    public Button editLevelButton;

//    [SerializeField] private Text title;
//    [SerializeField] private Image[] stars;
//    [SerializeField] private Image lockedImage;
//    [SerializeField] private Image cardImage;

//    [HideInInspector] public Button button;

//    private Color starsColor;
//    public Color deactivatedColor;

//    int numLevel = 1;

//#if !UNITY_EDITOR
//    private void Awake()
//    {
//        Configure();
//    }
//#else
//    private void Update()
//    {

//    }
//#endif
//    //private void Configure()
//    //{
//    //    if (level == null) return;

//    //    button = GetComponent<Button>();
//    //    starsColor = stars[0].color;
//    //    starsColor.a = 0f;

//    //    lockedImage.enabled = false;
//    //    title.text = numLevel.ToString();

//    //    int levelStars = ProgressManager.Instance.GetLevelStars(category, numLevel - 1);

//    //    //cambia el color de las estrellas que ha conseguido el jugador en el nivel
//    //    for (int i = 0; i < 3; i++) {
//    //        if (i >= levelStars)
//    //            stars[i].color = starsColor;
//    //        else
//    //            starsColor.a = 0.2f;
//    //    }
//    //}

//    public void ConfigureLevel(LevelDataSO level, CategoryDataSO category, int numLevel)
//    {
//        this.level = level;
//        this.numLevel = numLevel;
//        this.category = category;
//        //Configure();
//    }

//    public void DeactivateStars()
//    {
//        for (int i = 0; i < stars.Length; i++)
//            stars[i].enabled = false;
//    }

//    public void DeactivateCard()
//    {
//        cardImage.color = deactivatedColor;
//        button.interactable = false;
//        lockedImage.enabled = true;
//        title.text = "";
//    }
//}
