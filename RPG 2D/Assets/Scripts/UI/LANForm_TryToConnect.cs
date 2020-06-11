using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LANForm_TryToConnect : MonoBehaviour
{
    public void Awake()
    {
        if (string.IsNullOrEmpty(ServerAddress.ServerIP) == false)
        {
            void _local_HelloWorldCallback(string message)
            {
                if (message == ServerAddress.HelloWorldMessage)
                {
                    ConnectingDialog.Success();
                    GetComponent<UITweenFormTransition>().Execute();
                }

                else
                {
                    ConnectingDialog.Failed();
                }
            }

            ConnectingDialog.Connecting(true);
            new GameObject(nameof(ConnectToServer)).AddComponent<ConnectToServer>().ConnectToHelloWorld(ServerAddress.HelloWorldAdderss, _local_HelloWorldCallback);
        }
    }

    public void TryToConnectToHelloWorld()
    {
        var helloWorldIP = FindObjectOfType<LANForm_ServerIPInput>().LanIP;
        helloWorldIP = $"http://{helloWorldIP}/HelloWorld.php";
        ConnectingDialog.Connecting();
        ConnectingDialog.OnHide += ConnectingDialog_OnHide;
        new GameObject(nameof(ConnectToServer)).AddComponent<ConnectToServer>().ConnectToHelloWorld(helloWorldIP, HelloWorldCallback);
    }

    private void ConnectingDialog_OnHide(string obj)
    {
        ConnectingDialog.OnHide -= ConnectingDialog_OnHide;

        if (obj != ConnectingDialog.MESSAGE_SUCCEEDED)
        {
            return;
        }

        GetComponent<UITweenFormTransition>().Execute();
    }

    private void HelloWorldCallback(string message)
    {
        if (message == "Hello World")
        {
            ServerAddress.ServerIP = FindObjectOfType<LANForm_ServerIPInput>().LanIP;
            ConnectingDialog.Success();
        }

        else
        {
            ConnectingDialog.Failed();
        }
    }
}
