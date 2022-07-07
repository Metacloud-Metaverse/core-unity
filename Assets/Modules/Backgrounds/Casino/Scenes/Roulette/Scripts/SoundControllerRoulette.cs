using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundControllerRoulette : MonoBehaviour
{
    [SerializeField] private AudioSource aSource;

    public Queue<InternalQueueData> clipsToPlay = new Queue<InternalQueueData>();
    private InternalQueueData queueInProgrees;
    private float timerQueue = 0;
    public static SoundControllerRoulette Instance;

    private float prevVolume;
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
        if (aSource == null)
        {
            aSource = GetComponent<AudioSource>();
        }
    }
    private void Update()
    {
        ProccesStackClips();
    }
    private void ProccesStackClips()
    {
        if (clipsToPlay.Count > 0)
        {
            if (queueInProgrees == null)
                queueInProgrees = clipsToPlay.Dequeue();
            else
            {
                if (timerQueue >= queueInProgrees.secsToPlayNext)
                {
                    aSource.PlayOneShot(queueInProgrees.clip);
                    queueInProgrees = null;
                    timerQueue = 0;
                }
                timerQueue += Time.deltaTime;
            }
     
        }
    }
    public bool isPlaying
    {
        get {
            return aSource.isPlaying;
        }
    }

    public SoundControllerRoulette PlayClip(AudioClip clip,bool canPlayOverride = true)
    {
        if (canPlayOverride)
        {
            aSource.PlayOneShot(clip);
        }
        else
        {
            if (aSource.isPlaying == false)
            {
                aSource.PlayOneShot(clip);
            }
        }
        return this;
    }
    public SoundControllerRoulette PlayClip(AudioClip clip,float volume, bool canPlayOverride = true)
    {
        if (canPlayOverride)
        {
            prevVolume = aSource.volume;
            aSource.volume = volume;
            aSource.PlayOneShot(clip);
        }
        else
        {
            if (aSource.isPlaying == false)
            {
                prevVolume = aSource.volume;
                aSource.volume = volume;
                aSource.PlayOneShot(clip);
            }
        }
        aSource.volume = prevVolume;
        return this;
    }
    public void PlayQueue(AudioClip clip,float secs)
    {
        clipsToPlay.Enqueue(new InternalQueueData(clip, secs));

    }
}

public class InternalQueueData
{
    public AudioClip clip;
    public float secsToPlayNext = 0.1f;

    public InternalQueueData(AudioClip clip,float secs)
    {
        this.clip = clip;
        secsToPlayNext = secs;
    }
}
