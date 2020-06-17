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

    public event Action<LoginResult,string,string,Gender> OnLogin;
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

                var splittedResult = result.Split('|');

                var intResult = Convert.ToInt32(splittedResult[0]);
                if (intResult != (int)LoginResult.Success)
                {
                    OnLogin?.Invoke(LoginResult.Failed_Username, null, null, Gender.Female);
                    return;
                }

                var hash = splittedResult[1];
                var salt = splittedResult[2];

                var isPasswordValid = HashSalt.VerifyPassword(password, hash, salt);
                if (isPasswordValid == false)
                {
                    OnLogin?.Invoke(LoginResult.Failed_Password, null, null, Gender.Female);
                    return;
                }

                var name = splittedResult[3];
                var gender = (Gender)Enum.Parse(typeof(Gender), splittedResult[4]);
                var birthday = splittedResult[5];

                OnLogin?.Invoke(LoginResult.Success, name, birthday, gender);
            }
            catch
            {
                OnLogin?.Invoke(LoginResult.Failed_MySQL, null, null, Gender.Female);
            }


            formContent.Dispose();
        }
    }
}
