using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIChangeTargetScaleOnMouseStay : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public UnityEvent buttonDownEvent;
    [SerializeField] private Image image;
    [SerializeField] private Color colorON;
    [SerializeField] private Color colorOFF;
    [SerializeField] private Vector3 toScale;
    [SerializeField] private Vector3 fromScale;
    [SerializeField] private float speed = 2;
    
    private Vector3 targetScale;

    private void Awake()
    {
        targetScale = fromScale;
        CheckImageComponent();
    }
    private void CheckImageComponent()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetTargetScaleToOpen();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetTargetScaleToClose();
    }

    public void SetTargetScaleToClose()
    {
        targetScale = fromScale;
        image.color = colorOFF;
    }

    public void SetTargetScaleToOpen()
    {
        targetScale = toScale;
        image.color = colorON;
        
    }

    void Update()
    {
        transform.localScale = Vector3.MoveTowards(transform.localScale, targetScale, speed * Time.deltaTime);      
    }


}
