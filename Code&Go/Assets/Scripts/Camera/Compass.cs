using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Brujula que sigue la camara en los niveles para marcar el norte
/// </summary>
public class Compass : MonoBehaviour
{
    public Transform cameraTransform;
    Vector3 direction;

    private void Start()
    {
        direction = new Vector3();
    }
    // Update is called once per frame
    void Update()
    {
        direction.z = cameraTransform.eulerAngles.y;
        transform.localEulerAngles = direction;
    }
}
