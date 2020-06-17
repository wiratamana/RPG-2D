using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserInfo : MonoBehaviour
{
    [SerializeField] private Image gender;
    [SerializeField] private TextMeshProUGUI username;
    [SerializeField] private TextMeshProUGUI birthday;

    [SerializeField] private Sprite male;
    [SerializeField] private Sprite female;

    [SerializeField] private Color maleColor;
    [SerializeField] private Color femaleColor;

    public void SetValue(string username, string birthday, Gender gender)
    {
        this.gender.sprite = gender == Gender.Female ? female : male;
        this.gender.color = gender == Gender.Female ? femaleColor : maleColor;
        this.username.text = username;
        this.birthday.text = birthday;

        gameObject.SetActive(true);
        GetComponent<RectTransform>().localScale = Vector3.one;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
