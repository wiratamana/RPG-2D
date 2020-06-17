using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Home : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI username;
    [SerializeField] private TextMeshProUGUI birthday;
    [SerializeField] private Image gender;

    [SerializeField] private Sprite spriteMale;
    [SerializeField] private Sprite spriteFemale;

    [SerializeField] private Color colorMale;
    [SerializeField] private Color colorFemale;

    [SerializeField] private UITweenFormTransition logoutTransition;
    [SerializeField] private UITweenFormTransition searchUserTransition;

    public void SetValue(string username, string birthday, Gender gender)
    {
        this.username.text = username;
        this.birthday.text = birthday;
        this.gender.color = gender == Gender.Female ? colorFemale : colorMale;
        this.gender.sprite = gender == Gender.Female ? spriteFemale : spriteMale;
    }

    public void OnLogout()
    {
        logoutTransition.Execute();
    }

    public void OnSearchUser()
    {
        searchUserTransition.Execute();
    }
}
