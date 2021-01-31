using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Sound/SimpleAudio")]
public class SimpleAudioEvent : AudioEvent
{
    public List<AudioClip> clips;
    public float volume, pitch; //min, max and other stuff...
    public override void Play(AudioSource audio)
    {	
        audio.volume = volume;
        audio.pitch = pitch;
        audio.clip = clips[Random.Range(0, clips.Count)];
        audio.Play();
    }
}

