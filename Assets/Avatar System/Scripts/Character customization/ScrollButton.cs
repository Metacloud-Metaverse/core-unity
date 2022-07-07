using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public bool isDown { get { return _isDown; } }
    public bool disabled {
        get { return _disabled; }
        set
        {
            _disabled = value;
            image.color = (value) ? disabledColor : normalColor;
        }
    }
    private bool _disabled;
    private bool _isDown;
    public Image image;
    public Color normalColor = new Color(1,1,1);
    public Color highlitedColor = new Color(0.9607844f, 0.9607844f, 0.9607844f);
    public Color pressedColor = new Color(0.7843138f, 0.7843138f, 0.7843138f);
    public Color disabledColor = new Color(0.7843138f, 0.7843138f, 0.7843138f, 0.5f);

    public void OnPointerDown(PointerEventData eventData)
    {
        if (disabled)
        {
            _isDown = false;
            return;
        }
        _isDown = true;
        image.color = pressedColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isDown = false;

        if (disabled) return;

        image.color = normalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (disabled) return;

        image.color = highlitedColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (disabled) return;

        image.color = normalColor;
    }
}
