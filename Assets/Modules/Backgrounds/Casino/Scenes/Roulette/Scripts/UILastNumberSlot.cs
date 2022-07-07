using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UILastNumberSlot : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Image baseImg;

    public void Set(int number)
    {
        baseImg.color = ColorsDict.GetColor(number);
        text.text = number.ToString();
        gameObject.SetActive(true);
    }
}
