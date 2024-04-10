using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogInCard : MonoBehaviour {

    [SerializeField] private Button logInPageButton; 
    [SerializeField] private Button createAccountPageButton;
    [SerializeField] private Button logInButton; 
    [SerializeField] private Button createAccountButton;

    [SerializeField] private Transform logInPage;
    [SerializeField] private Transform createAccountPage;

    [SerializeField] private Image logInPageButtonImage;
    [SerializeField] private Image createAccountPageButtonImage;

    [SerializeField] private Color enableColor;
    [SerializeField] private Color disableColor;

    [SerializeField] public Text logInUsername;
    [SerializeField] public InputField logInPassword;    
    [SerializeField] public Text createAccountUsername;
    [SerializeField] public InputField createAccountPassword;
    [SerializeField] public InputField createAccountPasswordRepit;

    [SerializeField] public ReplyMessage replyMessage;

    public LoginManager loginManager;

    public void ChangePage() {
        Debug.Log("Change between Pages");
        // Change the buttons
        logInPageButton.interactable = !logInPageButton.interactable;
        createAccountPageButton.interactable = !createAccountPageButton.interactable;

        // Change the colors
        logInPageButtonImage.color = logInPageButton.interactable ? disableColor : enableColor;
        createAccountPageButtonImage.color = createAccountPageButton.interactable ? disableColor : enableColor;

        // Change the active page
        logInPage.gameObject.SetActive(createAccountPageButton.interactable);
        createAccountPage.gameObject.SetActive(logInPageButton.interactable);
    }

    public void TryToLogIn() {
        if(logInUsername.text.Length == 0) {
            replyMessage.Configure(MessageReplyID.FailedUsernameEmpty);
            return;
        }
        if (logInPassword.text.Length == 0) {
            replyMessage.Configure(MessageReplyID.FailedPasswordEmpty);
            return;
        }
        loginManager.TryToLogIn(logInUsername.text, logInPassword.text);
        logInPassword.text = "";
    }

    public void TryToCreateAccount() {
        if (createAccountUsername.text.Length == 0) {
            replyMessage.Configure(MessageReplyID.FailedUsernameEmpty);
            return;
        }
        if (createAccountUsername.text.Contains("\\") || createAccountUsername.text.Contains("/") || createAccountUsername.text.Contains(":") ||
            createAccountUsername.text.Contains("*") || createAccountUsername.text.Contains("?") || createAccountUsername.text.Contains("\"") ||
            createAccountUsername.text.Contains("<") || createAccountUsername.text.Contains(">") || createAccountUsername.text.Contains("|")) {
            replyMessage.Configure(MessageReplyID.IllegalCharacters);
            return;
        }
        if (createAccountPassword.text.Length == 0) {
            replyMessage.Configure(MessageReplyID.FailedPasswordEmpty);
            return;
        }
        if (createAccountPasswordRepit.text.Length == 0) {
            replyMessage.Configure(MessageReplyID.FailedPasswordEmpty);
            return;
        }
        if (createAccountPassword.text != createAccountPasswordRepit.text) {
            replyMessage.Configure(MessageReplyID.PasswordDoesNotMatch);
            return;
        }

        loginManager.TryToCreateAccount(createAccountUsername.text, createAccountPassword.text);
        createAccountPassword.text = "";
    }
}
