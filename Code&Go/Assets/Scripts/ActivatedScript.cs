using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
/**
TODO Esta es la clase que se comunica con el servidor, la he generado en un gameObject a parte porque me ha dado muchisimos problemas para ejecutar si estaba inactivo, 
por lo que siempre la activo y luego la llamo, no se si es muy mala practica, pero es lo único que me ha funcionado.
*/
public class ActivatedScript : MonoBehaviour
{   //TODO Esto no se muy bien si estaria mejor en campos que configurara el usuario en el momento de la exportación/Importación
    string server = "http://13.48.149.249";
    string port = "8080";
    public GameObject InfoImportPanel;
    [SerializeField] 
    private Text _title;

    IEnumerator PostCourutine(string path, string json, Func<UnityWebRequest, int> onOK, Func<UnityWebRequest, int> onKO)
    {
        string url = server + ":" + port + "/" + path;

        Debug.Log("Posteito a " + url);

        var req = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        req.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        if (GameManager.Instance.GetLogged())
            req.SetRequestHeader("Authorization", GameManager.Instance.GetToken());

        //Send the request then wait here until it returns
        yield return req.SendWebRequest();

        if (req.isNetworkError)
        {
            showError("Error en post: " + req.error);
        }
        else
        {
            //Todo guay
            if(req.responseCode == 200)
            {
                onOK(req);
            }
            else
            {
                onKO(req);
            }
        }
    }

    IEnumerator GetCourutine(string path, Func<UnityWebRequest, int> onOK, Func<UnityWebRequest, int> onKO)
    {
        string url = server + ":" + port + "/" + path;

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            if(GameManager.Instance.GetLogged())
                request.SetRequestHeader("Authorization", GameManager.Instance.GetToken());

            yield return request.Send();

            if (request.isNetworkError)
            {
                showError("Error en get: " + request.error);
            }
            else
            {
                //Todo guay
                if (request.responseCode == 200)
                {
                    onOK(request);
                }
                else
                {
                    onKO(request);
                }
            }
        }
    }

    public void Post(string path, string json, Func<UnityWebRequest, int> onOK, Func<UnityWebRequest, int> onKO)
    {
        StartCoroutine(PostCourutine(path, json, onOK, onKO));
    }

    public void Get(string path, Func<UnityWebRequest, int> onOK, Func<UnityWebRequest, int> onKO)
    {
        StartCoroutine(GetCourutine(path, onOK, onKO));
    }

    public void Get(string path, Func<UnityWebRequest, int> onOK, Func<UnityWebRequest, int> onKO, int param)
    {

    }

    public void SetIp(string newip)
    {
        string[] serverport = newip.Split(':');
        server = serverport[0];
        port = serverport[1];
    }

    public void showError(string msg){
        InfoImportPanel.SetActive(true);

        //To find `child2` which is the second index(1)
        GameObject iconError = InfoImportPanel.transform.GetChild(1).gameObject;
        iconError.SetActive(true);
        //To find `child3` which is the third index(2)
        GameObject iconSuccess = InfoImportPanel.transform.GetChild(2).gameObject;
        iconSuccess.SetActive(false);
        Debug.Log(msg);

        _title.text = msg;
        //TODO AQUI LA IDEA ES QUE SE MUESTRE EN UNA VENTANA EMERGENTE GUAY
    }

    public void showSuccess(string msg){
        InfoImportPanel.SetActive(true);

        //To find `child2` which is the second index(1)
        GameObject iconError = InfoImportPanel.transform.GetChild(1).gameObject;
        iconError.SetActive(false);
        //To find `child3` which is the third index(2)
        GameObject iconSuccess = InfoImportPanel.transform.GetChild(2).gameObject;
        iconSuccess.SetActive(true);
        Debug.Log(msg);

        _title.text = msg;
        //TODO AQUI LA IDEA ES QUE SE MUESTRE EN UNA VENTANA EMERGENTE GUAY
    }

    public void closeInfoPanel(){
        InfoImportPanel.SetActive(false);
    }
}
