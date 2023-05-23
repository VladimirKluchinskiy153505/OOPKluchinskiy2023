using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ButtonColorChanger : MonoBehaviour
{
    public Button button;
    public Color normalColor;
    public Color selectedColor;

    private Image buttonImage;
    private bool isSelected = false;

    private void Start()
    {
        //buttonImage = button.
        buttonImage.tintColor = normalColor;
    }

    public void ToggleSelection()
    {
        isSelected = !isSelected;

        if (isSelected)
        {
            buttonImage.tintColor = selectedColor;
        }
        else
        {
            buttonImage.tintColor = normalColor;
        }
    }
}
