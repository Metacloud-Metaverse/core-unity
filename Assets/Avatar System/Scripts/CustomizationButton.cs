using UnityEngine;
using UnityEngine.UI;

public class CustomizationButton : MonoBehaviour
{
    public Text text;
    public Image selectedFeedback;
    public Button button;

    public Color pressedColor;
    public Color unpressedColor;

    public void SetPressed()
    {
        text.color = pressedColor;
        selectedFeedback.gameObject.SetActive(true);
        button.interactable = false;
    }

    public void SetUnpressed()
    {
        text.color = unpressedColor;
        selectedFeedback.gameObject.SetActive(false);
        button.interactable = true;
    }
}
