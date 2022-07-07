using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EraserSwich : MonoBehaviour
{
    public List<Image> affects = new List<Image>();
    public Color colorON = Color.white; 
    public Color colorOFF = Color.white;
    public AudioClip onClip;
    public AudioClip offClip;
    public Roulette roulette;
    private bool isOn;
    private void Update()
    {
        if (isOn == false && roulette.isManualErasing)
        {
            isOn = true;
            foreach (var item in affects)
            {
                item.color = colorON;
            }
        }
        else if (isOn && roulette.isManualErasing == false)
        {
            isOn = false;
            foreach (var item in affects)
            {
                item.color = colorOFF;
            }
        }
    }

    public void PlayAudio()
    {
        if (isOn == false)
            SoundControllerRoulette.Instance.PlayClip(onClip);
        else
            SoundControllerRoulette.Instance.PlayClip(offClip);
    }
}
