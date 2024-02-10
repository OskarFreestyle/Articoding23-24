using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour {

    [SerializeField] private Button logInPageButton; 
    [SerializeField] private Button createAccountPageButton;
    [SerializeField] private Button logInButton; 
    [SerializeField] private Button createAccountButton;

    [SerializeField] private Transform logInPage;
    [SerializeField] private Transform createAccountPage;

    [SerializeField] private Image logInPageButtonImage;
    [SerializeField] private Image logInPageBGImage;
    [SerializeField] private Image createAccountPageButtonImage;
    [SerializeField] private Image createAccountPageBGImage;

    [SerializeField] private Color enableColor;
    [SerializeField] private Color disableColor;

    [SerializeField] public Text logInUsername;
    [SerializeField] public InputField logInPassword;    
    [SerializeField] public Text createAccountUsername;
    [SerializeField] public InputField createAccountPassword;

    public LoginManager loginManager;

    public void ChangePage() {
        Debug.Log("Change between Pages");
        // Change the buttons
        logInPageButton.interactable = !logInPageButton.interactable;
        createAccountPageButton.interactable = !createAccountPageButton.interactable;

        // Change the colors
        logInPageButtonImage.color = logInPageButton.interactable ? disableColor : enableColor;
        logInPageBGImage.color = logInPageButton.interactable ? disableColor : enableColor;
        createAccountPageButtonImage.color = createAccountPageButton.interactable ? disableColor : enableColor;
        createAccountPageBGImage.color = createAccountPageButton.interactable ? disableColor : enableColor;

        // Change the active page
        logInPage.gameObject.SetActive(createAccountPageButton.interactable);
        createAccountPage.gameObject.SetActive(logInPageButton.interactable);
    }


    //private void Start() {
    //    // Delegate the Log in button
    //    logInButton.onClick.AddListener(delegate { TryToLogIn(logInUsername.text, logInPassword.text); });

    //    // Delegate the Create account button
    //    createAccountButton.onClick.AddListener(delegate { TryToCreateAccount(createAccountUsername.text, createAccountPassword.text); });

    //    // Disable the create account page
    //    createAccountPage.gameObject.SetActive(false);
    //}

    public void TryToLogIn() {
        Debug.Log("Log In user: " + logInUsername.text + ", pass: " + logInPassword.text);
        //loginManager.TryToLogIn(user, pass);
        //logInPassword.text = "";
    }

    public void TryToCreateAccount() {
        Debug.Log("Create Account user: " + createAccountUsername.text + ", pass: " + createAccountPassword.text);
        //loginManager.TryToLogIn(user, pass);
        //createAccountPassword.text = "";
    }
}
