using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;

using Tamana;

public class LoginToServer
{
    public enum LoginResult
    {
        Success = 0,
        Failed_MySQL = 1,
        Failed_Username = 2,
        Failed_Password = 3,
    }

    public event Action<LoginResult> OnLogin;
    private HashSalt hashSalt;
    public bool IsGenerateSaltProcessFinished { get; private set; }

    public void GenerateSalt(string password)
    {
        new Thread(() =>
        {

            hashSalt = HashSalt.GenerateSaltedHash(password);
            IsGenerateSaltProcessFinished = true;

        }).Start();
    }

    public async Task PostToHtml(string name)
    {
        using (var http = new HttpClient())
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("name", name),
            });

            try
            {
                var req = await http.PostAsync(ServerAddress.LoginFormAddress, formContent);
                var result = await req.Content.ReadAsStringAsync();


                UnityEngine.Debug.Log(result);

                OnLogin?.Invoke(LoginResult.Success);
            }
            catch
            {
                OnLogin?.Invoke(LoginResult.Failed_MySQL);
            }
            

            formContent.Dispose();


        }
    }
}
