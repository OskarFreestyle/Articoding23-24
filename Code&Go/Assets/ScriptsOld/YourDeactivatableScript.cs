using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class YourDeactivatableScript : MonoBehaviour
{
    public IEnumerator Import_Courutine() {
         using (UnityWebRequest request = UnityWebRequest.Get("localhost:5000/levels/63bb086f1e0a2d98e2d2fe94"))
        {
            yield return request.Send();

            if (request.isNetworkError){
                Debug.Log(request.error);
            }else  {
                Debug.Log(request.downloadHandler.text);
            }
        }

    } 

    public void Import() {
         StartCoroutine(Import_Courutine());
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
