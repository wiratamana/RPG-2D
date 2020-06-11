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

    private LoginToServer loginToServer;

    private void Start()
    {
        enabled = false;
    }

    public void OnClick_Login()
    {
        ConnectingDialog.Connecting();
        loginToServer = new LoginToServer();
        loginToServer.OnLogin += LoginCallback;
        loginToServer.GenerateSalt(password.text);
        enabled = true;
    }

    private async void Update()
    {
        if (loginToServer.IsGenerateSaltProcessFinished)
        {
            enabled = false;
            await loginToServer.PostToHtml(username.text);
        }
    }

    private void LoginCallback(LoginToServer.LoginResult result)
    {
        if (result == LoginToServer.LoginResult.Success)
        {
            ConnectingDialog.Success();
        }
        else
        {
            ConnectingDialog.Failed();
        }
    }
}
