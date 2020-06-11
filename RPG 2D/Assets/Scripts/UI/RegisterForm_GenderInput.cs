using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RegisterForm_GenderInput : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Gender gender;
    [SerializeField] private Color activeColor;
    [SerializeField] private Color inactiveColor;

    private Image image;
    public bool IsActive => image.color == activeColor;
    public event System.Action<Gender> GenderChanged;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GenderChanged?.Invoke(gender);
    }
    
    public void Activate()
    {
        image.color = activeColor;
    }

    public void Deactivate()
    {
        image.color = inactiveColor;
    }
}
