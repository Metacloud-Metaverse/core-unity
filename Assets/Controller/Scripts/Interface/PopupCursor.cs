using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupCursor : MonoBehaviour
{
    [SerializeField] private GameObject _root;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _image;
    [SerializeField] private RectTransform _background;
    
    private RectTransform _textRect;

    public static PopupCursor Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        _textRect = _text.GetComponent<RectTransform>();

    }

    public void OpenMousePopup(Sprite sprite, string description)
    {
        _text.text = description +"  <size=0>.</size>";
        _image.sprite = sprite;
        _root.SetActive(true);
        _background.anchorMin = _textRect.anchorMin;
        _background.anchorMax = _textRect.anchorMax;
        _background.anchoredPosition = _textRect.anchoredPosition;
        _background.sizeDelta = _textRect.sizeDelta;
    }

    private void Update()
    {
        if (_background.sizeDelta != _textRect.sizeDelta)
        {
            _background.anchorMin = _textRect.anchorMin;
            _background.anchorMax = _textRect.anchorMax;
            _background.anchoredPosition = _textRect.anchoredPosition;
            _background.sizeDelta = _textRect.sizeDelta;
        }
    }

    public void CloseMousePopup()
    {
        _root.SetActive(false);
    }

  

}
