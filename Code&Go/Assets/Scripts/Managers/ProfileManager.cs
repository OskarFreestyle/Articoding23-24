using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{
    public Text starsText;
    public Text levelsText;
    public Text perfectText;
    public Text categoryText;

    // Start is called before the first frame update
    void Start()
    {
        int levls = ProgressManager.Instance.GetLvlsCompleted();
        //starsText.text = ProgressManager.Instance.GetTotalStars().ToString();
        levelsText.text = levls.ToString();
        if(levls > 0) {
            perfectText.text = ((ProgressManager.Instance.GetLvlsPerfects() * 100) / levls).ToString() + "%";
        }
        else {
            perfectText.text = "0%";
        }
        categoryText.text = ProgressManager.Instance.GetLastCategory().ToString();
    }
}