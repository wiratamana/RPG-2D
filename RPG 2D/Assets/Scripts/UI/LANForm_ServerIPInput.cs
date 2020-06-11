using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LANForm_ServerIPInput : MonoBehaviour
{
    private TMP_InputField lanIP;

    private void Awake()
    {
        lanIP = GetComponent<TMP_InputField>();

        if (string.IsNullOrEmpty(ServerAddress.ServerIP) == false)
        {
            lanIP.text = ServerAddress.ServerIP;
        }
    }

    public string LanIP => lanIP.text;
}
