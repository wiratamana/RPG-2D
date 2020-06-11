using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RegisterAccountToServer
{
    public enum RegisterResult
    {
        Success = 0
    }

    private string username;
    private string password;
    private string gender;
    private string birthday;

    public event Action<RegisterResult> RegisterCompleted;

    public RegisterAccountToServer(string username, string password, string gender, string birthday)
    {
        this.username = username;
        this.password = password;
        this.gender = gender;
        this.birthday = birthday;
    }

    public void Register()
    {
        
    }
}
