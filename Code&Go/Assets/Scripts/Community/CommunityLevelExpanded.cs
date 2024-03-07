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
    [SerializeField] private Text levelHashtags;

    public void ConfigureLevel(ServerClasses.Level level) {
        levelName.text = level.name;
        levelAuthor.text = level.owner.username;
        levelID.text = level.id.ToString();

        levelLikes.text = level.likes.ToString();
        levelPlays.text = level.plays.ToString();

        Texture2D tex = new Texture2D(1, 1);

        Debug.Log(level.image.Length);

        // CESAR AQUI SE CARGA LA IMAGEN

        if(ImageConversion.LoadImage(tex, level.image)) {
            Debug.Log("tex created correctly");
        } else Debug.Log("error load image into texture");

        Rect rect = new Rect(100, 100, 400, 400);
        Vector2 pivot = new Vector2(0.5f, 0.5f);
        levelImage.sprite = Sprite.Create(tex, rect, pivot);


        //UnityEngine.Experimental.Rendering.GraphicsFormat gf = (UnityEngine.Experimental.Rendering.GraphicsFormat)88;
        //var a = ImageConversion.EncodeArrayToPNG(level.image, gf, 1000, 1000);


        //byte[] imageBytes = levelDataSO.levelImage.texture.EncodeToPNG();
        //levelJson.levelImageEncode = imageBytes;

        //Sprite.Create()

        //levelImage.sprite.rect
    }

    public void LikeLevel(bool state) {
        if(state) levelLikes.text = (int.Parse(levelLikes.text) + 1).ToString();
        else levelLikes.text = (int.Parse(levelLikes.text) - 1).ToString();
        Debug.Log("LikeLeveles metodo");
    }



}
