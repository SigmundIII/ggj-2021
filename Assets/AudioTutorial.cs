using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTutorial : MonoBehaviour {
    public AudioEvent tutorial;
    public AudioSource audioSource;
    public void PlayTutorial() {
        tutorial.Play(audioSource);
    }
}
