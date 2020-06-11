using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ServerAddress
{
    public static string ServerIP
    {
        get => PlayerPrefs.GetString(nameof(ServerAddress), null);
        set => PlayerPrefs.SetString(nameof(ServerAddress), value);
    }

    public static string HelloWorldAdderss => $"http://{ServerIP}/HelloWorld.php";
    public static string HelloWorldMessage => "Hello World";

    public static string LoginFormAddress => $"http://{ServerIP}/LoginForm.php";
    public static string RegisterFormAddress => $"http://{ServerIP}/RegisterForm.php";
}
