using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrowsePlaylistParams : MonoBehaviour {

    [SerializeField] public InputField nameInputField;
    [SerializeField] public InputField authorInputField;
    [SerializeField] public InputField iDInputField;

    // Always the same
    private string header = "playlists";
    private bool publicLevels = true; // Always true?
    private int numLevels = 10;

    private bool liked = false;
    private bool orderByLikes = false;

    // Finished params
    private string param;

    public string GetParams() {
        // Disable the object
        gameObject.SetActive(false);
        // Update the params
        UpdateParams();

        //param += "&liked=true";

        return param;
    }

    public void UpdateParams() {
        // Header
        param = header;
        param += "?";

        // Public Levels
        param += "publicPlaylists=";
        param += publicLevels ? "true" : "false";

        // Num Levels
        param += "&size=";
        param += numLevels;

        // Filter by liked
        if (liked) {
            param += "&liked=true";
        }

        // Search by title
        if(nameInputField.text != "") {
            param += "&title=";
            param += nameInputField.text;
        }
        // Search by author
        if (authorInputField.text != "") {
            param += "&owner=";
            param += authorInputField.text;
        }

        // Search by ID
        if (iDInputField.text != "") {
            param += "&levelid=";
            param += iDInputField.text;
        }

        // SearchByTags


        // Order by
        param += "&orderByLikes=";
        if (orderByLikes) param += "true";
        else param += "false";
    }

    public void setLikedEnable(bool state) {
        Debug.Log("setLikedState: " + state);
        liked = state;
    }

    public void setOrderByLikes(bool state) {
        Debug.Log("setOrderByLikes: " + state);
        orderByLikes = state;
    }

}