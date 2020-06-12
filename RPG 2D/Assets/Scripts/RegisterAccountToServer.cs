using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Tamana;

public class RegisterAccountToServer
{
    public enum RegisterResult
    {
        Success = 0,
        Failed_Mysql = 1,
        Failed_Username = 2,
        Failed_Insert = 3
    }

    private string name;
    private string hash;
    private string salt;
    private string gender;
    private string birthday;

    public event Action<RegisterResult> RegisterCompleted;

    public RegisterAccountToServer(string name, string password, string gender, string birthday)
    {
        this.name = name;
        this.gender = gender;
        this.birthday = birthday;
        var hashSalt = HashSalt.GenerateSaltedHash(password);
        hash = hashSalt.Hash;
        salt = hashSalt.Salt;

        var hashLength = hash.Length;
        var saltLength = salt.Length;
    }

    public async Task Register()
    {
        using (var http = new HttpClient())
        {
            var formData = new Dictionary<string, string>();
            formData.Add(nameof(name), name);
            formData.Add(nameof(hash), hash);
            formData.Add(nameof(salt), salt);
            formData.Add(nameof(gender), gender);
            formData.Add(nameof(birthday), birthday);

            var registerForm = new FormUrlEncodedContent(formData);

            var response = await http.PostAsync(ServerAddress.RegisterFormAddress, registerForm);
            var result = await response.Content.ReadAsStringAsync();

            var intResult = Convert.ToInt32(result);

            RegisterCompleted.Invoke((RegisterResult)intResult);

            registerForm.Dispose();
        }
    }
}
