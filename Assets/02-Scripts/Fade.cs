using System;
using UnityEngine;

public class Fade : MonoBehaviour {
    private Animator animator;
    private CanvasGroup canvasGroup;

    public Action fadeInComplete;
    public Action fadeOutComplete;

    private bool fading;

    private void Awake() {
        animator = GetComponent<Animator>();
        canvasGroup = GetComponentInChildren<CanvasGroup>();
    }

    private void Update() {
        if (fading) {
            if (canvasGroup.alpha == 1) {
                fading = false;
                // Debug.Log("Fade In complete");
                fadeInComplete?.Invoke();
            } else if (canvasGroup.alpha == 0) {
                fading = false;
                // Debug.Log("Fade Out complete");
                fadeOutComplete?.Invoke();
            }
        }
    }

    public void FadeIn() {
        animator.Play("Fade In");
        fading = true;
    }
    
    public void FadeOut() {
        animator.Play("Fade Out");
        fading = true;
    }

}
