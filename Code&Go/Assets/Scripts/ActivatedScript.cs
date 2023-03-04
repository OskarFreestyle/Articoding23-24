using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
/**
TODO Esta es la clase que se comunica con el servidor, la he generado en un gameObject a parte porque me ha dado muchisimos problemas para ejecutar si estaba inactivo, 
por lo que siempre la activo y luego la llamo, no se si es muy mala practica, pero es lo único que me ha funcionado.
*/
public class ActivatedScript : MonoBehaviour
{   //TODO Esto no se muy bien si estaria mejor en campos que configurara el usuario en el momento de la exportación/Importación
    public string server = "http://localhost";
    public string port = "5000";

    [System.Serializable]
    public class Level
    {
        public BoardState boardstate;
        public ActiveBlocks activeblocks;
    }

    IEnumerator Import_Courutine(string id) {
        string url = server + ":" + port + "/levels/" + id;
        Debug.Log("Obteniendo nivel de servidor " + url);
         using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.Send();

            if (request.isNetworkError){
                Debug.Log(request.error);
            }else  {
                Debug.Log(request.downloadHandler.text);
                Level level = JsonUtility.FromJson<Level>(request.downloadHandler.text);
                Debug.Log("Llamamos a almacenar el nivel importado...");
                ProgressManager.Instance.UserCreatedLevel(level.boardstate.ToJson(), level.activeblocks.ToJson());
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
        
        var req = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        req.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return req.SendWebRequest();

        if (req.isNetworkError) {
            Debug.Log("Error While Sending: " + req.error);
        } else {
            Debug.Log("Exportado! " + req.downloadHandler.text);
        }
    }

    public void Import(string id) {
         StartCoroutine(Import_Courutine(id));
    }

    public void Export(BoardState boardstate, ActiveBlocks activeblocks) {
         StartCoroutine(Export_Courutine(boardstate, activeblocks));
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
