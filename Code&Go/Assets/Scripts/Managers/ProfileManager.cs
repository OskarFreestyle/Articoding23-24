using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{
    public Text starsText;

    // Start is called before the first frame update
    void Start()
    {
        starsText.text = (string)ProgressManager.Instance.GetTotalStars().ToString();
    }
}