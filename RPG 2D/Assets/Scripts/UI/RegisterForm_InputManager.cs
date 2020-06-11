using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum Gender
{
    Male,
    Female
}

public class RegisterForm_InputManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField username;
    [SerializeField] private TMP_InputField bd_year;
    [SerializeField] private TMP_InputField bd_month;
    [SerializeField] private TMP_InputField bd_day;

    [SerializeField] private TMP_InputField password;
    [SerializeField] private TMP_InputField kakunin;

    [SerializeField] private RegisterForm_GenderInput gender_male;
    [SerializeField] private RegisterForm_GenderInput gender_female;

    private void Awake()
    {
        gender_male.GenderChanged += OnGenderChanged;
        gender_female.GenderChanged += OnGenderChanged;
    }

    public void OnClickRegister()
    {
        // TODO : if username is empty
        // TODO : if username contain symbol
        // TODO : if gender not selected
        // TODO : if birthday date invalid
        // TODO : if password / kakunin is empty
        // TODO : if password and kakunin not same

        var username = this.username.text;
        var password = this.password.text;
        var gender = gender_male.IsActive ? Gender.Male.ToString() : Gender.Female.ToString();
        var birthday = $"{bd_year.text}-{bd_month.text}-{bd_day.text}";
        var account = new RegisterAccountToServer(username, password, gender, birthday);
        account.RegisterCompleted += OnRegisterCompleted;
        account.Register();

        // TODO : if username was already registered
    }

    private void OnGenderChanged(Gender gender)
    {
        if (gender == Gender.Female)
        {
            gender_female.Activate();
            gender_male.Deactivate();
        }
        else if (gender == Gender.Male)
        {
            gender_female.Deactivate();
            gender_male.Activate();
        }
    }

    private void OnRegisterCompleted(RegisterAccountToServer.RegisterResult result)
    {

    }
}
