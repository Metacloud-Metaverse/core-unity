using UnityEngine;
using UnityEngine.UI;

public class SwitchButtonsDisabled : MonoBehaviour
{
    public Button[] buttons;

    public void SetButtonsEnabled()
    {
        foreach (var button in buttons)
        {
            button.interactable = true;
        }
    }

    public void OnButtonClicked(Button clickedButton)
    {
        SetButtonsEnabled();

        clickedButton.interactable = false;
    }
}
