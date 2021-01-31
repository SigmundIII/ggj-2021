using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour {
    private Animator animator;

    public Action fadeInComplete;
    public Action fadeOutComplete;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    public void FadeIn() {
        animator.Play("FadeIn");
    }
    
    public void FadeOut() {
        animator.Play("FadeOut");
    }

}
