using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IPInputText : MonoBehaviour
{
    private string ip;
    private InputField mainInputField;
    public ActivatedScript activated;
    void Start()
    {
        mainInputField = GetComponent<InputField>();
        mainInputField.onEndEdit.AddListener(delegate { EndEditCheck(); });
    }

    void EndEditCheck()
    {
        ip = mainInputField.text;
        activated.SetIp(ip);
    }
}
