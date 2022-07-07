using UnityEngine;
using UnityEngine.UI;

public class ScrollBehaviour : MonoBehaviour
{
    public ScrollButton leftButton;
    public ScrollButton rightButton;
    public ScrollRect scrollRect;
    public float speed = 0.5f;
    public RectTransform container;
    public int limitForShow = 16;

    private int _buttonsCount;

    private void Start()
    {
        DecideActive(limitForShow);
        CheckButtonsDisabled();
    }

    private void Update()
    {
        if(leftButton.isDown)
        {
            scrollRect.horizontalNormalizedPosition -= speed * Time.deltaTime / _buttonsCount;
            CheckButtonsDisabled();
        }
        
        if(rightButton.isDown)
        {
            scrollRect.horizontalNormalizedPosition += speed * Time.deltaTime / _buttonsCount;
            CheckButtonsDisabled();
        }
    }

    private void CheckButtonsDisabled()
    {
        if (scrollRect.horizontalNormalizedPosition <= 0)
        {
            if(!leftButton.disabled)     
                leftButton.disabled = true;     
        }
        else
        {
            if (leftButton.disabled)   
                leftButton.disabled = false;           
        }

        if (scrollRect.horizontalNormalizedPosition >= 1)
        {
            if(!rightButton.disabled)
                rightButton.disabled = true;
        }
        else
        {
            if (rightButton.disabled)
                rightButton.disabled = false;
        }

    }

    public void DecideActive(int pLimitForShow)
    {
        if (GetButtonsCount() > pLimitForShow)
            SetButtonsActive(true);
        else
            SetButtonsActive(false);
    }

    private void SetButtonsActive(bool active)
    {
        leftButton.gameObject.SetActive(active);
        rightButton.gameObject.SetActive(active);
    }

    private int GetButtonsCount()
    {
        int count = 0;
        for (int i = 0; i < container.childCount; i++)
        {
            var child = container.GetChild(i);
            if (child.gameObject.activeSelf)
                count++;
        }
        _buttonsCount = count;
        return count;
    }
}
