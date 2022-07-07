using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetUI : MonoBehaviour
{
    public Vector2 pivot;
    [Range(0f,1f)]
    public float percentSize = 1;

    private RectTransform rectTransform;
    private RectTransform parentRectTransfrom;

    private float posPercentX = 0.0f;
    private float posPercentY = 0.0f;
    public Vector2 deltaSize;

    private Vector2 initSize;
    private Vector2 initParentSize;

    public float currentPercentParentSize;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parentRectTransfrom = transform.parent.GetComponent<RectTransform>();

        initSize = rectTransform.sizeDelta;
        initParentSize = parentRectTransfrom.sizeDelta;

        Vector2 halfRectSize = parentRectTransfrom.sizeDelta * 0.5f;

        posPercentX = Mathf.InverseLerp(-halfRectSize.x, halfRectSize.x, transform.localPosition.x);
        posPercentY = Mathf.InverseLerp(-halfRectSize.y, halfRectSize.y, transform.localPosition.y);
    }
    
    private void Update()
    {
        Vector2 halfRectSize = parentRectTransfrom.sizeDelta * 0.5f;
        deltaSize = parentRectTransfrom.sizeDelta;

        //transform.localPosition = parentRectTransfrom.rect.min;


        currentPercentParentSize = initSize.magnitude * 100 / parentRectTransfrom.sizeDelta.magnitude;

        rectTransform.sizeDelta = Vector2.one * Mathf.Abs(currentPercentParentSize) * percentSize;

        //float posX = Mathf.Lerp(-halfRectSize.x, halfRectSize.x, posPercentX);
        //float posY = Mathf.Lerp(-halfRectSize.y, halfRectSize.y, posPercentY);

        //  transform.localPosition = new Vector2(posX, posY);
    }
}
