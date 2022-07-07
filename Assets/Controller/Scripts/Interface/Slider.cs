using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Slider : MonoBehaviour
{
    public TextMeshProUGUI textPercet;

    public void SetPercent(float v)
    {
        var valueFixed = (v * 100);
        textPercet.text = valueFixed.ToString("00.00") + "%";
    }
}
