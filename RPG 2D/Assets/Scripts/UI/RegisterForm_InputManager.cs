using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

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

    [SerializeField] private UITweenFormTransition cancelTransition;

    private void Awake()
    {
        gender_male.GenderChanged += OnGenderChanged;
        gender_female.GenderChanged += OnGenderChanged;
    }

    public void OnClickRegister()
    {
        var username = this.username.text;
        if (IsUsernameValid(username) == false)
        {
            // TODO : Show username not valid error
            ErrorDialog.Instance.Open("Username is not valid");
            Debug.Log("Username is not valid");
            return;
        }

        if (gender_male.IsActive == false && gender_female.IsActive == false)
        {
            // TODO : Show gender is not currently selected error
            ErrorDialog.Instance.Open("Gender error");
            Debug.Log("Gender error");
            return;
        }

        var birthday = $"{bd_year.text}-{bd_month.text}-{bd_day.text}";
        if (System.DateTime.TryParse(birthday, out System.DateTime result) == false)
        {
            // TODO : Show invalid birthday error;
            ErrorDialog.Instance.Open("Birthday is not valid");
            Debug.Log("Birthday is not valid");
            return;
        }

        var password = this.password.text;
        var kakunin  = this.kakunin.text;
        if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(kakunin) || password != kakunin)
        {
            // TODO : Show password / kakunin error.
            ErrorDialog.Instance.Open("password or kakunin is not valid");
            Debug.Log("password or kakunin is not valid");
            return;
        }

        ConnectingDialog.Connecting();
        var gender = gender_male.IsActive ? Gender.Male.ToString() : Gender.Female.ToString();
        var account = new RegisterAccountToServer(username, password, gender, birthday);
        account.RegisterCompleted += OnRegisterCompleted;
        account.Register();
    }   

    public void OnClickCancel()
    {
        void resetForm()
        {
            ResetForm();
            cancelTransition.OnTransitionCompleted -= resetForm;
        }

        cancelTransition.OnTransitionCompleted += resetForm;
        cancelTransition.Execute();
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
        Debug.Log($"OnRegisterCompleted - Rusult : {result}");

        switch (result)
        {
            case RegisterAccountToServer.RegisterResult.Success:
                void resetRegisterForm()
                {
                    ResetForm();
                    cancelTransition.OnTransitionCompleted -= resetRegisterForm;
                }

                void onHide(string message)
                {
                    cancelTransition.OnTransitionCompleted += resetRegisterForm;
                    cancelTransition.Execute();
                    ConnectingDialog.OnHide -= onHide;
                }

                ConnectingDialog.OnHide += onHide;
                ConnectingDialog.Success();
                
                break;
            case RegisterAccountToServer.RegisterResult.Failed_Mysql:
                break;
            case RegisterAccountToServer.RegisterResult.Failed_Username:
                // TODO : if username was already registered
                break;
            case RegisterAccountToServer.RegisterResult.Failed_Insert:
                break;
        }
    }

    private void ResetForm()
    {
        username.text = null;
        bd_year.text = null;
        bd_month.text = null;
        bd_day.text = null;

        password.text = null;
        kakunin.text = null;

        gender_male.Deactivate();
        gender_female.Deactivate();
    }

    private bool IsUsernameValid(string username)
    {
        if (string.IsNullOrEmpty(username))
        {
            return false;
        }

        return Regex.IsMatch(username, @"^[a-zA-Z0-9_]+$");
    }
}
