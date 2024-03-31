using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LoginManager : MonoBehaviour {

    private static LoginManager instance;
    static public LoginManager Instance {
        get { return instance; }
    }

    [SerializeField] private RectTransform loginPage;
    [SerializeField] private RectTransform mainPage;
    [SerializeField] private ProfileCard profileCard;

    [SerializeField] private ReplyMessage replyMessage;

    public ActivatedScript activated;

    string userName = "";

    private string tempName;
    private string tempPass;

    private bool isNewRegister = false;


    private void Awake() {
        Debug.Log("Login Manager Awake");

        if (!instance) {
            instance = this;
            //DontDestroyOnLoad(this);
        }
        else {
            Debug.LogWarning("More than 1 Login Manager created");
            DestroyImmediate(gameObject);
        }

        Debug.Log("Login Manager Awake Finished");
    }

    public void TryToLogIn(string user, string pass) {
        tempName = user;
        tempPass = pass;

        ServerClasses.Login loginJson = new ServerClasses.Login();
        loginJson.username = user;
        loginJson.password = pass;

        userName = user;

        activated.Post("login", JsonUtility.ToJson(loginJson), OnLoginOK, OnLoginKO);
    }

    public void TryToCreateAccount(string user, string pass) {
        isNewRegister = true;

        tempName = user;
        tempPass = pass;

        ServerClasses.CreateAccount createAccountJson = new ServerClasses.CreateAccount();
        createAccountJson.username = user;
        createAccountJson.password = pass;

        userName = user;

        activated.Post("users/students", JsonUtility.ToJson(createAccountJson), OnRegisterOK, OnRegisterKO);
    }

    public int OnRegisterOK(UnityWebRequest req) {
        replyMessage.Configure(MessageReplyID.SuccessfulRegistration);
        TryToLogIn(tempName, tempPass);
        return 0;
    }

    public int OnRegisterKO(UnityWebRequest req) {
        replyMessage.Configure(MessageReplyID.FailedRegistration);
        return 0;
    }

    public void LogOut() {
        isNewRegister = false;

        // Conexion variables
        GameManager.Instance.SetToken("");
        GameManager.Instance.SetAdmin(false);
        GameManager.Instance.SetUserName("");
        GameManager.Instance.SetLogged(false);

        CommunityManager.Instance.ChangeEnablePage(loginPage);
    }

    int OnLoginOK(UnityWebRequest req) {
        Debug.Log("Logeado");

        ServerClasses.LoginResponse responseToken = JsonUtility.FromJson<ServerClasses.LoginResponse>(req.downloadHandler.text);

        // Conexion variables
        Debug.Log("name " + responseToken.role);
        Debug.Log("token " + responseToken.token);
        Debug.Log("image index " + responseToken.imageIndex);
        GameManager.Instance.userIconID = responseToken.imageIndex;
        GameManager.Instance.SetToken(responseToken.token);
        GameManager.Instance.SetRole(responseToken.role);
        GameManager.Instance.SetUserName(userName);
        GameManager.Instance.SetLogged(true);

        if(!isNewRegister) replyMessage.Configure(MessageReplyID.SuccessfulLogin);

        CommunityManager.Instance.ChangeEnablePage(mainPage);

        profileCard.Configure();

        CommunityManager.Instance.GetUserLikedThings();

        isNewRegister = false;
        return 0;
    }

    int OnLoginKO(UnityWebRequest req) {
        if (!isNewRegister) replyMessage.Configure(MessageReplyID.FailedLogin);

        userName = "";
        isNewRegister = false;
        return 0;
    }
}
