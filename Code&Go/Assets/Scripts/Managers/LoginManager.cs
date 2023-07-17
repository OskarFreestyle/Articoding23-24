using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LoginManager : MonoBehaviour
{
    public GameObject waitPanel;
    public GameObject OKPanel;
    public GameObject KOPanel;
    public GameObject loginPanel;

    public ActivatedScript activated;

    public ComunidadLayout comunidadLayout;

    public void TryToLogIn(string user, string pass)
    {
        //Movidas de conexión

        waitPanel.SetActive(true);

        ServerClasses.Login loginJson = new ServerClasses.Login();
        loginJson.username = user;
        loginJson.password = pass;

        activated.Post("login", JsonUtility.ToJson(loginJson), OnLoginOK, OnLoginKO);
    }

    int OnLoginOK(UnityWebRequest req)
    {
        ServerClasses.LoginResponse responseToken = JsonUtility.FromJson<ServerClasses.LoginResponse>(req.downloadHandler.text);
        Debug.Log("Logeado");

        GameManager.Instance.SetToken(responseToken.token);
        GameManager.Instance.SetLogged(true);

        waitPanel.SetActive(false);
        loginPanel.SetActive(false);
        OKPanel.SetActive(true);
        KOPanel.SetActive(false);

        if (comunidadLayout.gameObject.active) 
            comunidadLayout.IsLoggedAction();

        return 0;
    }

    int OnLoginKO(UnityWebRequest req)
    {
        Debug.Log("No logeado");
        waitPanel.SetActive(false);
        KOPanel.SetActive(true);
        OKPanel.SetActive(false);

        return 0;
    }
}
