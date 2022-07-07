using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCustomizationButtons : MonoBehaviour
{
    public CustomizationButton[] buttons;

    private void Start()
    {
        SetFirstButtonPressed();
    }

    public void SetFirstButtonPressed()
    {
        SetButtonsUnpressed();
        buttons[0].SetPressed();
    }

    public void SetButtonsUnpressed()
    {
        foreach (var button in buttons)
        {
            button.SetUnpressed();
        }
    }

    public void OnButtonClicked(CustomizationButton clickedButton)
    {
        SetButtonsUnpressed();

        clickedButton.SetPressed();
    }
}
