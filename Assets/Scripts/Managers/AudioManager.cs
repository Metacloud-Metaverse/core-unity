using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class AudioManager : MonoBehaviourSingleton<AudioManager>
{
    public AudioMixer mixer;


    public void SetVolume(float v,string volParameterName)
    {
        var fixedVolu = (v + 80) / 0.81f;
        mixer.SetFloat(volParameterName, fixedVolu);
    }
}
