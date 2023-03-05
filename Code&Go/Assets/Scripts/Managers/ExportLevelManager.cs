using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExportLevelManager : MonoBehaviour
{
    public ActivatedScript activated;
    public IPInputText ipInputText;

    public void CallExport()
    {
        string ip = ipInputText.GetString();

        activated.SetIp(ip);
    }
}
