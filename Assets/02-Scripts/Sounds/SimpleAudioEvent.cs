using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Sound/SimpleAudio")]
public class SimpleAudioEvent : AudioEvent
{
    //Strangers by Le Gang https://soundcloud.com/thisislegang Creative Commons — Attribution 3.0 Unported — CC BY 3.0 http://creativecommons.org/licenses/by/3.0/ Music promoted by Audio Library https://youtu.be/IjFa_UDzNrc
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

