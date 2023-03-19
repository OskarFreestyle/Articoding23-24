using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
/**
TODO Esta es la clase que se comunica con el servidor, la he generado en un gameObject a parte porque me ha dado muchisimos problemas para ejecutar si estaba inactivo, 
por lo que siempre la activo y luego la llamo, no se si es muy mala practica, pero es lo único que me ha funcionado.
*/
public class ActivatedScript : MonoBehaviour
{   //TODO Esto no se muy bien si estaria mejor en campos que configurara el usuario en el momento de la exportación/Importación
    public string server = "http://localhost";
    public string port = "5000";
    public string levelName = "level";
    public GameObject InfoImportPanel;
    [SerializeField] 
        private Text _title;
    [System.Serializable]
    public class Level
    {
        public string _id;
        public BoardState boardstate;
        public ActiveBlocks activeblocks;
    }

    IEnumerator Import_Courutine(string id, string ip) {
        string url = ip + "/levels/" + id;
        Debug.Log("Obteniendo nivel de servidor " + url);
         using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.Send();

            if (request.isNetworkError){
                showError("Error en la importación del nivel: " + request.error);
            }else  {
                Level level = JsonUtility.FromJson<Level>(request.downloadHandler.text);
                ProgressManager.Instance.UserCreatedLevel(level.boardstate.ToJson(), level.activeblocks.ToJson(), "NivelImportado");
                showSuccess("¡Nivel importado con éxito!");
            }
        }

    } 

    IEnumerator Export_Courutine(BoardState boardstate, ActiveBlocks activeblocks) {
        string url = server + ":" + port + "/levels/";
        
        Debug.Log("Exportando al servidor " + url);
        
        Level level = new Level();
        level.boardstate = boardstate;
        level.activeblocks = activeblocks;

        string json = JsonUtility.ToJson(level);
        Debug.Log(json);
        
        var req = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        req.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return req.SendWebRequest();

        if (req.isNetworkError) {
            showError("Error en la exportación del nivel: " + req.error);

        } else {
            Level newLevel = JsonUtility.FromJson<Level>(req.downloadHandler.text);
            showSuccess("Nivel exportado con éxito. ID: " + newLevel._id);
            
        }
    }

    public void Import(string id, string ip) {
         StartCoroutine(Import_Courutine(id, ip));
    }

    public void Export(BoardState boardstate, ActiveBlocks activeblocks) {
         StartCoroutine(Export_Courutine(boardstate, activeblocks));
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
