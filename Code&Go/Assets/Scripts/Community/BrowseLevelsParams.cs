using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrowseLevelsParams : MonoBehaviour {

    [SerializeField] public InputField nameInputField;
    [SerializeField] public InputField authorInputField;
    [SerializeField] public InputField iDInputField;

    // Always the same
    private string header = "levels";
    private bool publicLevels = true; // Always true?
    private int numLevels = 10;

    // Finished params
    private string param;

    public string GetParams() {
        // Disable the object
        gameObject.SetActive(false);
        // Update the params
        UpdateParams();

        param += "&liked=true";

        return param;
    }

    public void UpdateParams() {
        // Header
        param = header;
        param += "?";

        // Public Levels
        param += "publicLevels=";
        param += publicLevels ? "true" : "false";

        param += "&";

        // Num Levels
        param += "size=";
        param += numLevels;

        // Search by title
        if(nameInputField.text != "") {
            param += "&title=";
            param += nameInputField.text;
        }
        // Search by author
        if (authorInputField.text != "") {
            param += "&author=";
            param += authorInputField.text;
        }

        // Search by ID
        if (iDInputField.text != "") {
            param += "&id=";
            param += iDInputField.text;
        }

        // SearchByTags
    }


}