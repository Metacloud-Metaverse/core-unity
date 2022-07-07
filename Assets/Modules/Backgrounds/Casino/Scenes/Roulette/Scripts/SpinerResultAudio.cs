using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinerResultAudio : MonoBehaviour
{
    public float delayBetweenClips = 0.5f;
    public AudioClip redClip;
    public AudioClip blackClip;
    public AudioClip playerWins;
    public List<AudioClip> numbersClip = new List<AudioClip>();

    public List<AudioClip> finalAudio = new List<AudioClip>();
    private float counter;
    private bool waitForCounter = false;
    public AudioSource aSource;
   
    public void AsampleAudio(int number, string color, bool almostOneChipWon)
    {
        if (almostOneChipWon)
            finalAudio.Add(playerWins);       
        if (color == "Black")
            finalAudio.Add(blackClip);
        if (color == "Red")
            finalAudio.Add(redClip);
        finalAudio.Add(numbersClip[number]);
        StartCoroutine(ProccesAudio());
    }

    private IEnumerator ProccesAudio()
    {
        while (finalAudio.Count >0)
        {
            var clip = finalAudio[0];
            finalAudio.RemoveAt(0);
            aSource.PlayOneShot(clip);
            yield return new WaitUntil(() => aSource.isPlaying == false);
        }            
    }


}
