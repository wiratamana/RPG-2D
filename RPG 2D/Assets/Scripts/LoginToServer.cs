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
    public bool IsGenerateSaltProcessFinished { get; private set; }

    public async Task Login(string username, string password)
    {
        using (var http = new HttpClient())
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("name", username),
            });

            try
            {
                var req = await http.PostAsync(ServerAddress.LoginFormAddress, formContent);
                var result = await req.Content.ReadAsStringAsync();

                var splittedResult = result.Split('_');

                var intResult = Convert.ToInt32(splittedResult[0]);
                if (intResult != (int)LoginResult.Success)
                {
                    OnLogin?.Invoke(LoginResult.Failed_Username);
                    return;
                }

                var hash = splittedResult[1];
                var salt = splittedResult[2];

                var isPasswordValid = HashSalt.VerifyPassword(password, hash, salt);
                if (isPasswordValid == false)
                {
                    OnLogin?.Invoke(LoginResult.Failed_Password);
                    return;
                }

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
