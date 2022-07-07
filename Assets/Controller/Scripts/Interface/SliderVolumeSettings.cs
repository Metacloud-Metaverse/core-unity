using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SliderVolumeSettings : MonoBehaviour
{
    public TextMeshProUGUI textPercent;
    public VolTypes volType;

    private void Start()
    {
        SetPercent(-80);
    }
    public void SetPercent(float v)
    {
        //-80 100%
        //-140 = 0;
        //-140 -- 0
        //
        //-80 -- 100
        float fixedPercent = (v + 140) / 0.6f;
        textPercent.text = fixedPercent.ToString("00.00") + "%";
        AudioManager.Instance.SetVolume(v, volType.ToString());
    }
}
