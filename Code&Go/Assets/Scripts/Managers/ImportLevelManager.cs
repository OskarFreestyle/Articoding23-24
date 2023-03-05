using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImportLevelManager : MonoBehaviour
{
    public ActivatedScript activated;
    public IPInputText ipInputText;
    public IPInputText idInputText;

    public void CallImport()
    {
        string id = idInputText.GetString();
        string ip = ipInputText.GetString();

        activated.Import(id, ip);
    }
}
