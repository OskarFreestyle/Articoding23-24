using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommunityLevelExpanded : MonoBehaviour {

    [SerializeField] private Text levelName;
    [SerializeField] private Text levelAuthor;
    [SerializeField] private Text levelID;
    [SerializeField] private Image levelImage;
    [SerializeField] private Text levelLikes;
    [SerializeField] private Text levelPlays;
    [SerializeField] private Text levelHastags;


    public void ConfigureLevel(ServerClasses.Level level) {
        levelName.text = level.name;
        levelAuthor.text = level.owner.username;
        levelID.text = level.id.ToString();

        levelLikes.text = level.likes.ToString();
        levelPlays.text = level.plays.ToString();



    }



}
