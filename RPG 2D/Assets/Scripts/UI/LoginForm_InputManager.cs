using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;
using TMPro;


public class LoginForm_InputManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField username;
    [SerializeField] private TMP_InputField password;
    [SerializeField] private UITweenFormTransition loginToHomeTransition;

    public void OnClick_Login()
    {
        ConnectingDialog.Connecting(true);
        var loginToServer = new LoginToServer();
        loginToServer.OnLogin += LoginCallback;
        loginToServer.Login(username.text, password.text);
        enabled = true;
    }


    private void LoginCallback(LoginToServer.LoginResult result)
    {
        if (result == LoginToServer.LoginResult.Success)
        {
            loginToHomeTransition.Execute();
            ConnectingDialog.Success();
        }
        else
        {
            ConnectingDialog.Failed();
        }
    }
}
