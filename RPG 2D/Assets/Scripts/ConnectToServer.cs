using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using System.Net.Http;

public class ConnectToServer : MonoBehaviour
{
    private UnityAction<string> callback;
    private string ip;

    public void ConnectToHelloWorld(string ip, UnityAction<string> callback)
    {
        this.ip = ip;
        this.callback = callback;

        GetHelloWorldAsync();
    }

    private async void GetHelloWorldAsync()
    {
        using (var http = new HttpClient())
        {
            try
            {
                http.Timeout = new System.TimeSpan(0, 0, 4);

                var response = await http.GetAsync(ip);
                var message = await response.Content.ReadAsStringAsync();

                callback?.Invoke(message);
            }
            catch
            {
                callback?.Invoke(null);
            }            
        }
        
        Destroy(gameObject);
    }
}
